using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common;
using BitCollectors.WinFormsControls.Common.Win32;

namespace BitCollectors.WinFormsControls
{
    public class ComboBoxEx : TextBox
    {
        private const bool backward = false;
        private const bool forward = true;
        private const string nullMask = "<>";

        private static readonly object EVENT_ISOVERWRITEMODECHANGED = new object();
        private static readonly object EVENT_MASKCHANGED = new object();
        private static readonly object EVENT_MASKINPUTREJECTED = new object();
        private static readonly object EVENT_TEXTALIGNCHANGED = new object();
        private static readonly object EVENT_VALIDATIONCOMPLETED = new object();
        private static readonly int BEEP_ON_ERROR = BitVector32.CreateMask(HIDE_PROMPT_ON_LEAVE);
        private static readonly int CUTCOPYINCLUDELITERALS = BitVector32.CreateMask(CUTCOPYINCLUDEPROMPT);
        private static readonly int CUTCOPYINCLUDEPROMPT = BitVector32.CreateMask(INSERT_TOGGLED);
        private static readonly int HANDLE_KEY_PRESS = BitVector32.CreateMask(IME_COMPLETING);
        private static readonly int HIDE_PROMPT_ON_LEAVE = BitVector32.CreateMask(REJECT_INPUT_ON_FIRST_FAILURE);
        private static readonly int IME_COMPLETING = BitVector32.CreateMask(IME_ENDING_COMPOSITION);
        private static readonly int IME_ENDING_COMPOSITION = BitVector32.CreateMask();
        private static readonly int INSERT_TOGGLED = BitVector32.CreateMask(USE_SYSTEM_PASSWORD_CHAR);
        private static readonly int IS_NULL_MASK = BitVector32.CreateMask(HANDLE_KEY_PRESS);
        private static readonly int QUERY_BASE_TEXT = BitVector32.CreateMask(IS_NULL_MASK);
        private static readonly int REJECT_INPUT_ON_FIRST_FAILURE = BitVector32.CreateMask(QUERY_BASE_TEXT);
        private static char systemPwdChar;
        private static readonly int USE_SYSTEM_PASSWORD_CHAR = BitVector32.CreateMask(BEEP_ON_ERROR);

        private int caretTestPos;
        private BitVector32 flagState;
        private InsertKeyMode insertMode;
        private MaskedTextProvider maskedTextProvider;
        private char passwordChar;

        private HorizontalAlignment textAlign;
        private Type validatingType;

        public ComboBoxEx() { }

        public ComboBoxEx(int lastSelLength)
        {
            var maskedTextProvider = new MaskedTextProvider(nullMask, CultureInfo.CurrentCulture);
            flagState[IS_NULL_MASK] = true;
            Initialize(maskedTextProvider);
        }

        public ComboBoxEx(string mask, int lastSelLength)
        {
            if (mask == null)
            {
                throw new ArgumentNullException();
            }

            var maskedTextProvider = new MaskedTextProvider(mask, CultureInfo.CurrentCulture);
            flagState[IS_NULL_MASK] = false;
            Initialize(maskedTextProvider);
        }

        public ComboBoxEx(MaskedTextProvider maskedTextProvider, int lastSelLength)
        {
            if (maskedTextProvider == null)
            {
                throw new ArgumentNullException();
            }

            flagState[IS_NULL_MASK] = false;
            Initialize(maskedTextProvider);
        }

        [
            Browsable(false),
            EditorBrowsable(EditorBrowsableState.Never),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public bool AcceptsTab
        {
            get { return false; }
            set { }
        }

        [DefaultValue(true)]
        public bool AllowPromptAsInput
        {
            get { return maskedTextProvider.AllowPromptAsInput; }
            set
            {
                if (value == maskedTextProvider.AllowPromptAsInput)
                    return;

                var newProvider = new MaskedTextProvider(
                    maskedTextProvider.Mask,
                    maskedTextProvider.Culture,
                    value,
                    maskedTextProvider.PromptChar,
                    maskedTextProvider.PasswordChar,
                    maskedTextProvider.AsciiOnly);

                SetMaskedTextProvider(newProvider);
            }
        }

        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(false)]
        public bool AsciiOnly
        {
            get { return maskedTextProvider.AsciiOnly; }

            set
            {
                if (value != maskedTextProvider.AsciiOnly)
                {
                    var newProvider = new MaskedTextProvider(
                        maskedTextProvider.Mask,
                        maskedTextProvider.Culture,
                        maskedTextProvider.AllowPromptAsInput,
                        maskedTextProvider.PromptChar,
                        maskedTextProvider.PasswordChar,
                        value);

                    SetMaskedTextProvider(newProvider);
                }
            }
        }

        [DefaultValue(false)]
        public bool BeepOnError
        {
            get { return flagState[BEEP_ON_ERROR]; }
            set { flagState[BEEP_ON_ERROR] = value; }
        }

        [Browsable(false),
         EditorBrowsable(EditorBrowsableState.Never),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanUndo
        {
            get { return false; }
        }

        [RefreshProperties(RefreshProperties.Repaint),]
        public CultureInfo Culture
        {
            get { return maskedTextProvider.Culture; }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                if (maskedTextProvider.Culture.Equals(value))
                    return;

                var newProvider = new MaskedTextProvider(
                    maskedTextProvider.Mask,
                    value,
                    maskedTextProvider.AllowPromptAsInput,
                    maskedTextProvider.PromptChar,
                    maskedTextProvider.PasswordChar,
                    maskedTextProvider.AsciiOnly);

                SetMaskedTextProvider(newProvider);
            }
        }

        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(MaskFormat.IncludeLiterals)]
        public MaskFormat CutCopyMaskFormat
        {
            get
            {
                if (flagState[CUTCOPYINCLUDEPROMPT])
                {
                    if (flagState[CUTCOPYINCLUDELITERALS])
                    {
                        return MaskFormat.IncludePromptAndLiterals;
                    }

                    return MaskFormat.IncludePrompt;
                }

                if (flagState[CUTCOPYINCLUDELITERALS])
                {
                    return MaskFormat.IncludeLiterals;
                }

                return MaskFormat.ExcludePromptAndLiterals;
            }

            set
            {
                if (value == MaskFormat.IncludePrompt)
                {
                    flagState[CUTCOPYINCLUDEPROMPT] = true;
                    flagState[CUTCOPYINCLUDELITERALS] = false;
                }
                else if (value == MaskFormat.IncludeLiterals)
                {
                    flagState[CUTCOPYINCLUDEPROMPT] = false;
                    flagState[CUTCOPYINCLUDELITERALS] = true;
                }
                else
                {
                    bool include = value == MaskFormat.IncludePromptAndLiterals;
                    flagState[CUTCOPYINCLUDEPROMPT] = include;
                    flagState[CUTCOPYINCLUDELITERALS] = include;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFormatProvider FormatProvider { get; set; }

        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(false)]
        public bool HidePromptOnLeave
        {
            get { return flagState[HIDE_PROMPT_ON_LEAVE]; }
            set
            {
                if (flagState[HIDE_PROMPT_ON_LEAVE] != value)
                {
                    flagState[HIDE_PROMPT_ON_LEAVE] = value;

                    if (!flagState[IS_NULL_MASK] && !Focused && !MaskFull && !DesignMode)
                    {
                        SetWindowText();
                    }
                }
            }
        }

        [
            DefaultValue(InsertKeyMode.Default)
        ]
        public InsertKeyMode InsertKeyMode
        {
            get { return insertMode; }
            set
            {
                if (insertMode != value)
                {
                    bool isOverwrite = IsOverwriteMode;
                    insertMode = value;

                    if (isOverwrite != IsOverwriteMode)
                    {
                        OnIsOverwriteModeChanged(EventArgs.Empty);
                    }
                }
            }
        }

        [Browsable(false)]
        public bool IsOverwriteMode
        {
            get
            {
                if (flagState[IS_NULL_MASK])
                {
                    return false;
                }

                switch (insertMode)
                {
                    case InsertKeyMode.Overwrite:
                        return true;

                    case InsertKeyMode.Insert:
                        return false;

                    case InsertKeyMode.Default:

                        return flagState[INSERT_TOGGLED];

                    default:
                        Debug.Fail("Invalid InsertKeyMode.  This code path should have never been executed.");
                        return false;
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(""), MergableProperty(false), Localizable(true)]
        public string Mask
        {
            get { return flagState[IS_NULL_MASK] ? string.Empty : maskedTextProvider.Mask; }
            set
            {
                if (flagState[IS_NULL_MASK] == string.IsNullOrEmpty(value) &&
                    (flagState[IS_NULL_MASK] || value == maskedTextProvider.Mask))
                {
                    return;
                }

                string text = null;
                string newMask = value;

                if (string.IsNullOrEmpty(value))
                {
                    flagState[IS_NULL_MASK] = true;

                    if (maskedTextProvider.IsPassword)
                    {
                        SetEditControlPasswordChar(maskedTextProvider.PasswordChar);
                    }
                }
                else
                {
                    if (value.Any(c => !MaskedTextProvider.IsValidMaskChar(c)))
                    {
                        throw new Exception();
                    }

                    if (flagState[IS_NULL_MASK])
                    {
                        text = Text;
                    }
                }

                var newProvider = new MaskedTextProvider(
                    newMask,
                    maskedTextProvider.Culture,
                    maskedTextProvider.AllowPromptAsInput,
                    maskedTextProvider.PromptChar,
                    maskedTextProvider.PasswordChar,
                    maskedTextProvider.AsciiOnly);

                SetMaskedTextProvider(newProvider, text);
            }
        }

        [Browsable(false)]
        public bool MaskCompleted
        {
            get { return maskedTextProvider.MaskCompleted; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MaskedTextProvider MaskedTextProvider
        {
            get { return flagState[IS_NULL_MASK] ? null : (MaskedTextProvider)maskedTextProvider.Clone(); }
        }

        [Browsable(false)]
        public bool MaskFull
        {
            get { return maskedTextProvider.MaskFull; }
        }

        [RefreshProperties(RefreshProperties.Repaint), DefaultValue('\0')]
        public char PasswordChar
        {
            get
            {
                return maskedTextProvider.PasswordChar;
            }
            set
            {
                if (!MaskedTextProvider.IsValidPasswordChar(value))
                {
                    throw new Exception();
                }

                if (passwordChar != value)
                {
                    if (value == maskedTextProvider.PromptChar)
                    {
                        throw new Exception();
                    }

                    passwordChar = value;

                    if (!UseSystemPasswordChar)
                    {
                        maskedTextProvider.PasswordChar = value;

                        if (flagState[IS_NULL_MASK])
                        {
                            SetEditControlPasswordChar(value);
                        }
                        else
                        {
                            SetWindowText();
                        }
                    }
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint), Localizable(true), DefaultValue('_')]
        public char PromptChar
        {
            get { return maskedTextProvider.PromptChar; }
            set
            {
                if (!MaskedTextProvider.IsValidInputChar(value))
                    throw new Exception();

                if (maskedTextProvider.PromptChar == value)
                    return;

                if (value == passwordChar || value == maskedTextProvider.PasswordChar)
                {
                    throw new Exception();
                }

                var newProvider = new MaskedTextProvider(
                    maskedTextProvider.Mask, maskedTextProvider.Culture,
                    maskedTextProvider.AllowPromptAsInput, value,
                    maskedTextProvider.PasswordChar, maskedTextProvider.AsciiOnly);

                SetMaskedTextProvider(newProvider);
            }
        }

        [
            DefaultValue(false)
        ]
        public bool RejectInputOnFirstFailure
        {
            get { return flagState[REJECT_INPUT_ON_FIRST_FAILURE]; }
            set { flagState[REJECT_INPUT_ON_FIRST_FAILURE] = value; }
        }

        [
            DefaultValue(true)
        ]
        public bool ResetOnPrompt
        {
            get { return maskedTextProvider.ResetOnPrompt; }
            set { maskedTextProvider.ResetOnPrompt = value; }
        }

        [DefaultValue(true)]
        public bool ResetOnSpace
        {
            get { return maskedTextProvider.ResetOnSpace; }
            set { maskedTextProvider.ResetOnSpace = value; }
        }

        [DefaultValue(true)]
        public bool SkipLiterals
        {
            get { return maskedTextProvider.SkipLiterals; }
            set { maskedTextProvider.SkipLiterals = value; }
        }

        [RefreshProperties(RefreshProperties.Repaint), Bindable(true), DefaultValue(""), Localizable(true)]
        public override string Text
        {
            get
            {
                if (flagState[IS_NULL_MASK] || flagState[QUERY_BASE_TEXT])
                {
                    return base.Text;
                }

                return TextOutput;
            }
            set
            {
                if (flagState[IS_NULL_MASK])
                {
                    base.Text = value;
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    Delete(Keys.Delete, 0, maskedTextProvider.Length);
                }
                else
                {
                    if (RejectInputOnFirstFailure)
                    {
                        MaskedTextResultHint hint;
                        string oldText = TextOutput;
                        if (maskedTextProvider.Set(value, out caretTestPos, out hint))
                        {
                            if (TextOutput != oldText)
                            {
                                SetText();
                            }
                            SelectionStart = ++caretTestPos;
                        }
                        else
                        {
                            OnMaskInputRejected(new MaskInputRejectedEventArgs(caretTestPos, hint));
                        }
                    }
                    else
                    {
                        Replace(value, /*startPosition*/ 0, /*selectionLen*/ maskedTextProvider.Length);
                    }
                }
            }
        }

        [Localizable(true), DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get { return textAlign; }
            set
            {
                if (textAlign != value)
                {
                    textAlign = value;
                    RecreateHandle();
                    OnTextAlignChanged(EventArgs.Empty);
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(MaskFormat.IncludeLiterals)]
        public MaskFormat TextMaskFormat
        {
            get
            {
                if (IncludePrompt)
                {
                    return IncludeLiterals ? MaskFormat.IncludePromptAndLiterals : MaskFormat.IncludePrompt;
                }

                return IncludeLiterals
                    ? MaskFormat.IncludeLiterals
                    : MaskFormat.ExcludePromptAndLiterals;
            }
            set
            {
                if (TextMaskFormat == value)
                {
                    return;
                }

                var oldText = flagState[IS_NULL_MASK] ? null : TextOutput;

                switch (value)
                {
                    case MaskFormat.IncludePrompt:
                        IncludePrompt = true;
                        IncludeLiterals = false;
                        break;
                    case MaskFormat.IncludeLiterals:
                        IncludePrompt = false;
                        IncludeLiterals = true;
                        break;
                    default:
                        {
                            bool include = value == MaskFormat.IncludePromptAndLiterals;
                            IncludePrompt = include;
                            IncludeLiterals = include;
                        }
                        break;
                }

                if (oldText != null && oldText != TextOutput)
                {
                    OnTextChanged(EventArgs.Empty);
                }
            }
        }

        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(false)]
        public bool UseSystemPasswordChar
        {
            get { return flagState[USE_SYSTEM_PASSWORD_CHAR]; }
            set
            {
                if (value == flagState[USE_SYSTEM_PASSWORD_CHAR])
                    return;

                if (value)
                {
                    if (SystemPasswordChar == PromptChar)
                    {
                        throw new Exception();
                    }

                    maskedTextProvider.PasswordChar = SystemPasswordChar;
                }
                else
                {
                    maskedTextProvider.PasswordChar = passwordChar;
                }

                flagState[USE_SYSTEM_PASSWORD_CHAR] = value;
                if (flagState[IS_NULL_MASK])
                {
                    SetEditControlPasswordChar(maskedTextProvider.PasswordChar);
                }
                else
                {
                    SetWindowText();
                }
            }
        }

        [Browsable(false), DefaultValue(null)]
        public Type ValidatingType
        {
            get { return validatingType; }
            set
            {
                if (validatingType != value)
                {
                    validatingType = value;
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool WordWrap
        {
            get { return false; }
            set { }
        }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams cp = base.CreateParams;

                HorizontalAlignment align = RtlTranslateHorizontal(textAlign);
                cp.ExStyle &= ~NativeMethods.WS_EX_RIGHT;
                switch (align)
                {
                    case HorizontalAlignment.Left:
                        cp.Style |= NativeMethods.ES_LEFT;
                        break;

                    case HorizontalAlignment.Center:
                        cp.Style |= NativeMethods.ES_CENTER;
                        break;

                    case HorizontalAlignment.Right:
                        cp.Style |= NativeMethods.ES_RIGHT;
                        break;
                }

                return cp;
            }
        }

        private bool IncludeLiterals
        {
            get { return maskedTextProvider.IncludeLiterals; }
            set { maskedTextProvider.IncludeLiterals = value; }
        }

        private bool IncludePrompt
        {
            get { return maskedTextProvider.IncludePrompt; }
            set { maskedTextProvider.IncludePrompt = value; }
        }

        private char SystemPasswordChar
        {
            get
            {
                if (systemPwdChar == '\0')
                {
                    var txtBox = new TextBox();
                    txtBox.UseSystemPasswordChar = true;

                    systemPwdChar = txtBox.PasswordChar;

                    txtBox.Dispose();
                }

                return systemPwdChar;
            }
        }

        private string TextOutput
        {
            get
            {
                Debug.Assert(!flagState[IS_NULL_MASK], "This method must be called when a Mask is provided.");
                return maskedTextProvider.ToString();
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public event EventHandler AcceptsTabChanged
        {
            add { }
            remove { }
        }

        public event EventHandler IsOverwriteModeChanged
        {
            add { Events.AddHandler(EVENT_ISOVERWRITEMODECHANGED, value); }
            remove { Events.RemoveHandler(EVENT_ISOVERWRITEMODECHANGED, value); }
        }

        public event EventHandler MaskChanged
        {
            add { Events.AddHandler(EVENT_MASKCHANGED, value); }
            remove { Events.RemoveHandler(EVENT_MASKCHANGED, value); }
        }

        public event MaskInputRejectedEventHandler MaskInputRejected
        {
            add { Events.AddHandler(EVENT_MASKINPUTREJECTED, value); }
            remove { Events.RemoveHandler(EVENT_MASKINPUTREJECTED, value); }
        }

        [
            Browsable(false),
            EditorBrowsable(EditorBrowsableState.Never),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public event EventHandler MultilineChanged
        {
            add { }
            remove { }
        }

        public event EventHandler TextAlignChanged
        {
            add { Events.AddHandler(EVENT_TEXTALIGNCHANGED, value); }

            remove { Events.RemoveHandler(EVENT_TEXTALIGNCHANGED, value); }
        }

        public event TypeValidationEventHandler TypeValidationCompleted
        {
            add { Events.AddHandler(EVENT_VALIDATIONCOMPLETED, value); }
            remove { Events.RemoveHandler(EVENT_VALIDATIONCOMPLETED, value); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ClearUndo()
        {
        }

        [
            EditorBrowsable(EditorBrowsableState.Never)
        ]
        public int GetFirstCharIndexFromLine(int lineNumber)
        {
            return 0;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int GetFirstCharIndexOfCurrentLine()
        {
            return 0;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ScrollToCaret()
        {
        }

        public override string ToString()
        {
            if (flagState[IS_NULL_MASK])
            {
                return base.ToString();
            }

            bool includePrompt = IncludePrompt;
            bool includeLits = IncludeLiterals;
            string str;
            try
            {
                IncludePrompt = IncludeLiterals = true;
                str = base.ToString();
            }
            finally
            {
                IncludePrompt = includePrompt;
                IncludeLiterals = includeLits;
            }

            return str;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Undo()
        {
        }

        public object ValidateText()
        {
            return PerformTypeValidation(null);
        }

        [
            EditorBrowsable(EditorBrowsableState.Advanced),
            UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)
        ]
        protected override void CreateHandle()
        {
            if (!flagState[IS_NULL_MASK] && RecreatingHandle)
            {
            }

            base.CreateHandle();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.KeyCode) == Keys.Return)
            {
                return false;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (flagState[IS_NULL_MASK] && maskedTextProvider.IsPassword)
            {
                SetEditControlPasswordChar(maskedTextProvider.PasswordChar);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnIsOverwriteModeChanged(EventArgs e)
        {
            var eh = Events[EVENT_ISOVERWRITEMODECHANGED] as EventHandler;

            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (flagState[IS_NULL_MASK])
            {
                return;
            }

            Keys keyCode = e.KeyCode;

            if (keyCode == Keys.Return || keyCode == Keys.Escape)
            {
                flagState[HANDLE_KEY_PRESS] = false;
            }

            if (keyCode == Keys.Insert && e.Modifiers == Keys.None && insertMode == InsertKeyMode.Default)
            {
                flagState[INSERT_TOGGLED] = !flagState[INSERT_TOGGLED];
                OnIsOverwriteModeChanged(EventArgs.Empty);
                return;
            }

            if (e.Control && char.IsLetter((char)keyCode))
            {
                switch (keyCode)
                {
                    case Keys.H:
                        keyCode = Keys.Back;
                        break;

                    default:

                        flagState[HANDLE_KEY_PRESS] = false;
                        return;
                }
            }

            if (keyCode == Keys.Delete || keyCode == Keys.Back)
            {
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (flagState[IS_NULL_MASK])
            {
                return;
            }

            if (!flagState[HANDLE_KEY_PRESS])
            {
                flagState[HANDLE_KEY_PRESS] = true;

                if (!char.IsLetter(e.KeyChar))
                {
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (flagState[IME_COMPLETING])
            {
                flagState[IME_COMPLETING] = false;
            }

            if (flagState[IME_ENDING_COMPOSITION])
            {
                flagState[IME_ENDING_COMPOSITION] = false;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnMaskChanged(EventArgs e)
        {
            var eh = Events[EVENT_MASKCHANGED] as EventHandler;

            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected virtual void OnTextAlignChanged(EventArgs e)
        {
            var eh = Events[EVENT_TEXTALIGNCHANGED] as EventHandler;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            bool queryBaseText = flagState[QUERY_BASE_TEXT];
            flagState[QUERY_BASE_TEXT] = false;
            try
            {
                base.OnTextChanged(e);
            }
            finally
            {
                flagState[QUERY_BASE_TEXT] = queryBaseText;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected override void OnValidating(CancelEventArgs e)
        {
            PerformTypeValidation(e);
            base.OnValidating(e);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool msgProcessed = base.ProcessCmdKey(ref msg, keyData);

            if (!msgProcessed)
            {
                if ((int)keyData == (int)Shortcut.CtrlA)
                {
                    base.SelectAll();
                    msgProcessed = true;
                }
            }

            return msgProcessed;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_CONTEXTMENU:
                case NativeMethods.EM_CANUNDO:

                    base.WndProc(ref m);
                    return;

                case NativeMethods.EM_SCROLLCARET:
                case NativeMethods.EM_LIMITTEXT:
                case NativeMethods.EM_UNDO:
                case NativeMethods.WM_UNDO:
                    return;

                default:
                    break;
            }

            if (flagState[IS_NULL_MASK])
            {
                base.WndProc(ref m);
                return;
            }

            switch (m.Msg)
            {
                case NativeMethods.WM_IME_STARTCOMPOSITION:

                    goto default;

                case NativeMethods.WM_IME_ENDCOMPOSITION:
                    flagState[IME_ENDING_COMPOSITION] = true;
                    goto default;

                case NativeMethods.WM_IME_COMPOSITION:

                    goto default;

                case NativeMethods.WM_CUT:

                    break;

                case NativeMethods.WM_COPY:

                    break;

                case NativeMethods.WM_PASTE:

                    break;

                case NativeMethods.WM_CLEAR:

                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void Delete(Keys keyCode, int startPosition, int selectionLen)
        {
            Debug.Assert(!flagState[IS_NULL_MASK], "This method must be called when a Mask is provided.");
            Debug.Assert(keyCode == Keys.Delete || keyCode == Keys.Back, "Delete called with keyCode == " + keyCode);
            Debug.Assert(startPosition >= 0 && ((startPosition + selectionLen) <= maskedTextProvider.Length),
                "Invalid position range.");

            caretTestPos = startPosition;

            if (selectionLen == 0)
            {
                if (keyCode == Keys.Back)
                {
                    if (startPosition == 0)
                    {
                        return;
                    }

                    startPosition--;
                }
                else
                {
                    if ((startPosition + selectionLen) == maskedTextProvider.Length)
                    {
                        return;
                    }
                }
            }

            int tempPos;
            int endPos = selectionLen > 0 ? startPosition + selectionLen - 1 : startPosition;
            MaskedTextResultHint hint;

            string oldText = TextOutput;
            if (maskedTextProvider.RemoveAt(startPosition, endPos, out tempPos, out hint))
            {
                if (TextOutput != oldText)
                {
                    SetText();
                    caretTestPos = startPosition;
                }
                else
                {
                    if (selectionLen > 0)
                    {
                        caretTestPos = startPosition;
                    }
                    else
                    {
                        if (hint == MaskedTextResultHint.NoEffect)
                        {
                            if (keyCode == Keys.Delete)
                            {
                                caretTestPos = maskedTextProvider.FindEditPositionFrom(startPosition, forward);
                            }
                            else
                            {
                                if (maskedTextProvider.FindAssignedEditPositionFrom(startPosition, forward) ==
                                    MaskedTextProvider.InvalidIndex)
                                {
                                    caretTestPos = maskedTextProvider.FindAssignedEditPositionFrom(startPosition,
                                        backward);
                                }
                                else
                                {
                                    caretTestPos = maskedTextProvider.FindEditPositionFrom(startPosition, backward);
                                }

                                if (caretTestPos != MaskedTextProvider.InvalidIndex)
                                {
                                    caretTestPos++;
                                }
                            }

                            if (caretTestPos == MaskedTextProvider.InvalidIndex)
                            {
                                caretTestPos = startPosition;
                            }
                        }
                        else
                        {
                            if (keyCode == Keys.Back)
                            {
                                caretTestPos = startPosition;
                            }
                        }
                    }
                }
            }
            else
            {
                OnMaskInputRejected(new MaskInputRejectedEventArgs(tempPos, hint));
            }
        }

        private void Initialize(MaskedTextProvider maskedTextProvider)
        {
            Debug.Assert(maskedTextProvider != null, "Initializing from a null MaskProvider ref.");

            this.maskedTextProvider = maskedTextProvider;

            if (!flagState[IS_NULL_MASK])
            {
                SetWindowText();
            }

            passwordChar = this.maskedTextProvider.PasswordChar;
            insertMode = InsertKeyMode.Default;

            flagState[HIDE_PROMPT_ON_LEAVE] = false;
            flagState[BEEP_ON_ERROR] = false;
            flagState[USE_SYSTEM_PASSWORD_CHAR] = false;
            flagState[REJECT_INPUT_ON_FIRST_FAILURE] = false;

            flagState[CUTCOPYINCLUDEPROMPT] = this.maskedTextProvider.IncludePrompt;
            flagState[CUTCOPYINCLUDELITERALS] = this.maskedTextProvider.IncludeLiterals;

            flagState[HANDLE_KEY_PRESS] = true;
            caretTestPos = 0;
        }

        private void OnMaskInputRejected(MaskInputRejectedEventArgs e)
        {
            Debug.Assert(!flagState[IS_NULL_MASK], "This method must be called when a Mask is provided.");

            if (BeepOnError)
            {
                var sp = new SoundPlayer();
                sp.Play();
            }

            var eh = Events[EVENT_MASKINPUTREJECTED] as MaskInputRejectedEventHandler;

            if (eh != null)
            {
                eh(this, e);
            }
        }

        private void OnTypeValidationCompleted(TypeValidationEventArgs e)
        {
            var eh = Events[EVENT_VALIDATIONCOMPLETED] as TypeValidationEventHandler;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        private object PerformTypeValidation(CancelEventArgs e)
        {
            if (validatingType == null)
                return null;

            var tve = new TypeValidationEventArgs(validatingType, true, null, null);

            OnTypeValidationCompleted(tve);

            if (e != null)
            {
                e.Cancel = tve.Cancel;
            }

            return null;
        }

        private bool PlaceChar(MaskedTextProvider provider, char ch, int startPosition, int length, bool overwrite,
            out MaskedTextResultHint hint)
        {
            Debug.Assert(!flagState[IS_NULL_MASK], "This method must be called when a Mask is provided.");

            caretTestPos = startPosition;

            if (startPosition < maskedTextProvider.Length)
            {
                if (length > 0)
                {
                    int endPos = startPosition + length - 1;
                    return provider.Replace(ch, startPosition, endPos, out caretTestPos, out hint);
                }
                if (overwrite)
                {
                    return provider.Replace(ch, startPosition, out caretTestPos, out hint);
                }
                return provider.InsertAt(ch, startPosition, out caretTestPos, out hint);
            }

            hint = MaskedTextResultHint.UnavailableEditPosition;
            return false;
        }

        private void Replace(string text, int startPosition, int selectionLen)
        {
            Debug.Assert(!flagState[IS_NULL_MASK], "This method must be called when a Mask is provided.");
            Debug.Assert(text != null, "text is null.");

            var clonedProvider = (MaskedTextProvider)maskedTextProvider.Clone();

            int currentCaretPos = caretTestPos;

            var hint = MaskedTextResultHint.NoEffect;
            int endPos = startPosition + selectionLen - 1;

            if (RejectInputOnFirstFailure)
            {
                bool succeeded = (startPosition > endPos)
                    ? clonedProvider.InsertAt(text, startPosition, out caretTestPos, out hint)
                    : clonedProvider.Replace(text, startPosition, endPos, out caretTestPos, out hint);

                if (!succeeded)
                {
                    OnMaskInputRejected(new MaskInputRejectedEventArgs(caretTestPos, hint));
                }
            }
            else
            {
                MaskedTextResultHint tempHint = hint;

                foreach (char ch in text)
                {
                    if (!maskedTextProvider.VerifyEscapeChar(ch, startPosition))
                    {
                        int testPos = clonedProvider.FindEditPositionFrom(startPosition, forward);

                        if (testPos == MaskedTextProvider.InvalidIndex)
                        {
                            OnMaskInputRejected(new MaskInputRejectedEventArgs(startPosition,
                                MaskedTextResultHint.UnavailableEditPosition));
                            continue;
                        }

                        startPosition = testPos;
                    }

                    int length = endPos >= startPosition ? 1 : 0;
                    bool replace = length > 0;

                    if (PlaceChar(clonedProvider, ch, startPosition, length, replace, out tempHint))
                    {
                        startPosition = caretTestPos + 1;

                        if (tempHint == MaskedTextResultHint.Success && hint != tempHint)
                        {
                            hint = tempHint;
                        }
                    }
                    else
                    {
                        OnMaskInputRejected(new MaskInputRejectedEventArgs(startPosition, tempHint));
                    }
                }

                if (selectionLen > 0)
                {
                    if (startPosition <= endPos)
                    {
                        if (!clonedProvider.RemoveAt(startPosition, endPos, out caretTestPos, out tempHint))
                        {
                            OnMaskInputRejected(new MaskInputRejectedEventArgs(caretTestPos, tempHint));
                        }

                        if (hint == MaskedTextResultHint.NoEffect && hint != tempHint)
                        {
                            hint = tempHint;
                        }
                    }
                }
            }

            bool updateText = TextOutput != clonedProvider.ToString();
            maskedTextProvider = clonedProvider;
            if (updateText)
            {
                SetText();
                caretTestPos = startPosition;
            }
            else
            {
                caretTestPos = currentCaretPos;
            }
        }

        private void ResetCulture()
        {
            Culture = CultureInfo.CurrentCulture;
        }

        private void SetEditControlPasswordChar(char pwdChar)
        {
            if (IsHandleCreated)
            {
                Invalidate();
            }
        }

        private void SetMaskedTextProvider(MaskedTextProvider newProvider)
        {
            SetMaskedTextProvider(newProvider, null);
        }

        private void SetMaskedTextProvider(MaskedTextProvider newProvider, string textOnInitializingMask)
        {
            Debug.Assert(newProvider != null, "Initializing from a null MaskProvider ref.");

            newProvider.IncludePrompt = maskedTextProvider.IncludePrompt;
            newProvider.IncludeLiterals = maskedTextProvider.IncludeLiterals;
            newProvider.SkipLiterals = maskedTextProvider.SkipLiterals;
            newProvider.ResetOnPrompt = maskedTextProvider.ResetOnPrompt;
            newProvider.ResetOnSpace = maskedTextProvider.ResetOnSpace;

            if (flagState[IS_NULL_MASK] && textOnInitializingMask == null)
            {
                maskedTextProvider = newProvider;
                return;
            }

            int testPos = 0;
            bool raiseOnMaskInputRejected = false;
            var hint = MaskedTextResultHint.NoEffect;
            MaskedTextProvider oldProvider = maskedTextProvider;

            bool preserveCharPos = oldProvider.Mask == newProvider.Mask;

            if (textOnInitializingMask != null)
            {
                raiseOnMaskInputRejected = !newProvider.Set(textOnInitializingMask, out testPos, out hint);
            }
            else
            {
                int assignedCount = oldProvider.AssignedEditPositionCount;
                int srcPos = 0;
                int dstPos = 0;

                while (assignedCount > 0)
                {
                    srcPos = oldProvider.FindAssignedEditPositionFrom(srcPos, forward);
                    Debug.Assert(srcPos != MaskedTextProvider.InvalidIndex, "InvalidIndex unexpected at this time.");

                    if (preserveCharPos)
                    {
                        dstPos = srcPos;
                    }
                    else
                    {
                        dstPos = newProvider.FindEditPositionFrom(dstPos, forward);

                        if (dstPos == MaskedTextProvider.InvalidIndex)
                        {
                            newProvider.Clear();

                            testPos = newProvider.Length;
                            hint = MaskedTextResultHint.UnavailableEditPosition;
                            break;
                        }
                    }

                    if (!newProvider.Replace(oldProvider[srcPos], dstPos, out testPos, out hint))
                    {
                        newProvider.Clear();
                        break;
                    }

                    srcPos++;
                    dstPos++;
                    assignedCount--;
                }

                raiseOnMaskInputRejected = !MaskedTextProvider.GetOperationResultFromHint(hint);
            }

            maskedTextProvider = newProvider;

            if (flagState[IS_NULL_MASK])
            {
                flagState[IS_NULL_MASK] = false;
            }

            if (raiseOnMaskInputRejected)
            {
                OnMaskInputRejected(new MaskInputRejectedEventArgs(testPos, hint));
            }

            if (newProvider.IsPassword)
            {
                SetEditControlPasswordChar('\0');
            }

            EventArgs e = EventArgs.Empty;

            if (textOnInitializingMask != null || oldProvider.Mask != newProvider.Mask)
            {
                OnMaskChanged(e);
            }
        }

        private void SetText() { }

        private void SetWindowText() { }

        private bool ShouldSerializeCulture() { return !CultureInfo.CurrentCulture.Equals(Culture); }
    }

}