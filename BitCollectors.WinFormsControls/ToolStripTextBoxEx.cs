using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using BitCollectors.WinFormsControls.Internal;

namespace BitCollectors.WinFormsControls
{
    [ToolStripItemDesignerAvailability(
        ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ToolStrip |
        ToolStripItemDesignerAvailability.ContextMenuStrip)]
    internal class ToolStripTextBoxEx : ToolStripControlHostInternal
    {
        internal static readonly object EventAcceptsTabChanged = new object();
        internal static readonly object EventBorderStyleChanged = new object();
        internal static readonly object EventHideSelectionChanged = new object();
        internal static readonly object EventModifiedChanged = new object();
        internal static readonly object EventMultilineChanged = new object();
        internal static readonly object EventReadOnlyChanged = new object();
        internal static readonly object EventTextBoxTextAlignChanged = new object();

        #region Constructor(s)
        public ToolStripTextBoxEx()
            : this(CreateControlInstance())
        {
            var textBox = Control as ToolStripTextBoxExInternal;
            textBox.Owner = this;
        }

        public ToolStripTextBoxEx(Control control)
            : base(control)
        {
        }
        #endregion

        #region Properties
        public bool AcceptsReturn
        {
            get { return InnerTextBox.AcceptsReturn; }
            set { InnerTextBox.AcceptsReturn = value; }
        }

        public bool AcceptsTab
        {
            get { return InnerTextBox.AcceptsTab; }
            set { InnerTextBox.AcceptsTab = value; }
        }

        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get { return InnerTextBox.AutoCompleteCustomSource; }
            set { InnerTextBox.AutoCompleteCustomSource = value; }
        }

        public AutoCompleteMode AutoCompleteMode
        {
            get { return InnerTextBox.AutoCompleteMode; }
            set { InnerTextBox.AutoCompleteMode = value; }
        }

        public AutoCompleteSource AutoCompleteSource
        {
            get { return InnerTextBox.AutoCompleteSource; }
            set { InnerTextBox.AutoCompleteSource = value; }
        }

        public BorderStyleEx BorderStyle
        {
            get { return InnerTextBox.BorderStyle; }
            set { InnerTextBox.BorderStyle = value; }
        }

        public bool CanUndo
        {
            get { return InnerTextBox.CanUndo; }
        }

        public CharacterCasing CharacterCasing
        {
            get { return InnerTextBox.CharacterCasing; }
            set { InnerTextBox.CharacterCasing = value; }
        }

        //public bool EscapeClearsInput
        //{
        //    get { return InnerTextBox.EscapeClearsInput; }
        //    set { InnerTextBox.EscapeClearsInput = value; }
        //}

        public override bool Focused
        {
            get { return base.Focused || InnerTextBox.Focused; }
        }

        public bool HideSelection
        {
            get { return InnerTextBox.HideSelection; }
            set { InnerTextBox.HideSelection = value; }
        }

        public TextBoxEx InnerTextBox
        {
            get { return Control as TextBoxEx; }
        }

        public string[] Lines
        {
            get { return InnerTextBox.Lines; }
            set { InnerTextBox.Lines = value; }
        }

        public int MaxLength
        {
            get { return InnerTextBox.MaxLength; }
            set { InnerTextBox.MaxLength = value; }
        }

        public bool Modified
        {
            get { return InnerTextBox.Modified; }
            set { InnerTextBox.Modified = value; }
        }

        public bool Multiline
        {
            get { return InnerTextBox.Multiline; }
            set { InnerTextBox.Multiline = value; }
        }

        public bool ReadOnly
        {
            get { return InnerTextBox.ReadOnly; }
            set { InnerTextBox.ReadOnly = value; }
        }

        public string SelectedText
        {
            get { return InnerTextBox.SelectedText; }
            set { InnerTextBox.SelectedText = value; }
        }

        public int SelectionLength
        {
            get { return InnerTextBox.SelectionLength; }
            set { InnerTextBox.SelectionLength = value; }
        }

        public int SelectionStart
        {
            get { return InnerTextBox.SelectionStart; }
            set { InnerTextBox.SelectionStart = value; }
        }

        public bool ShortcutsEnabled
        {
            get { return InnerTextBox.ShortcutsEnabled; }
            set { InnerTextBox.ShortcutsEnabled = value; }
        }

        public new string Text
        {
            get { return InnerTextBox.Text; }
            set { InnerTextBox.Text = value; }
        }

        public HorizontalAlignment TextBoxTextAlign
        {
            get { return InnerTextBox.TextAlign; }
            set { InnerTextBox.TextAlign = value; }
        }

        public int TextLength
        {
            get { return InnerTextBox.TextLength; }
        }

        public string WatermarkText
        {
            get { return InnerTextBox.WatermarkText; }
            set { InnerTextBox.WatermarkText = value; }
        }

        public bool WordWrap
        {
            get { return InnerTextBox.WordWrap; }
            set { InnerTextBox.WordWrap = value; }
        }

        protected override Padding DefaultMargin
        {
            get
            {
                if (IsOnDropDown)
                {
                    return new Padding(1);
                }
                return new Padding(1, 0, 1, 0);
            }
        }

        protected override Size DefaultSize
        {
            get { return new Size(100, 22); }
        }
        #endregion

        #region Overrides
        public override Size GetPreferredSize(Size constrainingSize)
        {
            var bounds = Bounds;
            return new Size(bounds.Width, InnerTextBox.PreferredHeight);
        }

        protected override void OnSubscribeControlEvents(Control control)
        {
            var textBox = control as TextBox;
            if (textBox != null)
            {
                textBox.AcceptsTabChanged += HandleAcceptsTabChanged;
                textBox.BorderStyleChanged += HandleBorderStyleChanged;
                textBox.HideSelectionChanged += HandleHideSelectionChanged;
                textBox.ModifiedChanged += HandleModifiedChanged;
                textBox.MultilineChanged += HandleMultilineChanged;
                textBox.ReadOnlyChanged += HandleReadOnlyChanged;
                textBox.TextAlignChanged += HandleTextBoxTextAlignChanged;
            }

            base.OnSubscribeControlEvents(control);
        }

        protected override void OnUnsubscribeControlEvents(Control control)
        {
            var textBox = control as TextBox;
            if (textBox != null)
            {
                textBox.AcceptsTabChanged -= HandleAcceptsTabChanged;
                textBox.BorderStyleChanged -= HandleBorderStyleChanged;
                textBox.HideSelectionChanged -= HandleHideSelectionChanged;
                textBox.ModifiedChanged -= HandleModifiedChanged;
                textBox.MultilineChanged -= HandleMultilineChanged;
                textBox.ReadOnlyChanged -= HandleReadOnlyChanged;
                textBox.TextAlignChanged -= HandleTextBoxTextAlignChanged;
            }
            base.OnUnsubscribeControlEvents(control);
        }
        #endregion

        #region Other methods
        public event EventHandler ActionButtonClick
        {
            add { InnerTextBox.Click += value; }
            remove { InnerTextBox.Click -= value; }
        }

        public new event EventHandler GotFocus
        {
            add { InnerTextBox.GotFocus += value; }
            remove { InnerTextBox.GotFocus -= value; }
        }

        public new event KeyEventHandler KeyDown
        {
            add { InnerTextBox.KeyDown += value; }
            remove { InnerTextBox.KeyDown -= value; }
        }

        public new event KeyPressEventHandler KeyPress
        {
            add { InnerTextBox.KeyPress += value; }
            remove { InnerTextBox.KeyPress -= value; }
        }

        public new event KeyEventHandler KeyUp
        {
            add { InnerTextBox.KeyUp += value; }
            remove { InnerTextBox.KeyUp -= value; }
        }

        public new event EventHandler LostFocus
        {
            add { InnerTextBox.LostFocus += value; }
            remove { InnerTextBox.LostFocus -= value; }
        }

        public void AppendText(string text)
        {
            InnerTextBox.AppendText(text);
        }

        public void Clear()
        {
            InnerTextBox.Clear();
        }

        public void ClearUndo()
        {
            InnerTextBox.ClearUndo();
        }

        public void Copy()
        {
            InnerTextBox.Copy();
        }

        private static Control CreateControlInstance()
        {
            TextBoxEx control = new ToolStripTextBoxExInternal();
            control.BorderStyle = BorderStyleEx.Fixed3D;
            control.AutoSize = true;
            return control;
        }

        public void Cut()
        {
            InnerTextBox.Copy();
        }

        public void DeselectAll()
        {
            InnerTextBox.DeselectAll();
        }

        public char GetCharFromPosition(Point pt)
        {
            return InnerTextBox.GetCharFromPosition(pt);
        }

        public int GetCharIndexFromPosition(Point pt)
        {
            return InnerTextBox.GetCharIndexFromPosition(pt);
        }

        public int GetFirstCharIndexFromLine(int lineNumber)
        {
            return InnerTextBox.GetFirstCharIndexFromLine(lineNumber);
        }

        public int GetFirstCharIndexOfCurrentLine()
        {
            return InnerTextBox.GetFirstCharIndexOfCurrentLine();
        }

        public int GetLineFromCharIndex(int index)
        {
            return InnerTextBox.GetLineFromCharIndex(index);
        }

        public Point GetPositionFromCharIndex(int index)
        {
            return InnerTextBox.GetPositionFromCharIndex(index);
        }

        private void HandleAcceptsTabChanged(object sender, EventArgs e)
        {
            OnAcceptsTabChanged(e);
        }

        private void HandleBorderStyleChanged(object sender, EventArgs e)
        {
            OnBorderStyleChanged(e);
        }

        private void HandleHideSelectionChanged(object sender, EventArgs e)
        {
            OnHideSelectionChanged(e);
        }

        private void HandleModifiedChanged(object sender, EventArgs e)
        {
            OnModifiedChanged(e);
        }

        private void HandleMultilineChanged(object sender, EventArgs e)
        {
            OnMultilineChanged(e);
        }

        private void HandleReadOnlyChanged(object sender, EventArgs e)
        {
            OnReadOnlyChanged(e);
        }

        private void HandleTextBoxTextAlignChanged(object sender, EventArgs e)
        {
            RaiseEvent(EventTextBoxTextAlignChanged, e);
        }

        protected virtual void OnAcceptsTabChanged(EventArgs e)
        {
            RaiseEvent(EventAcceptsTabChanged, e);
        }

        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            RaiseEvent(EventBorderStyleChanged, e);
        }

        protected virtual void OnHideSelectionChanged(EventArgs e)
        {
            RaiseEvent(EventHideSelectionChanged, e);
        }

        protected virtual void OnModifiedChanged(EventArgs e)
        {
            RaiseEvent(EventModifiedChanged, e);
        }

        protected virtual void OnMultilineChanged(EventArgs e)
        {
            RaiseEvent(EventMultilineChanged, e);
        }

        protected virtual void OnReadOnlyChanged(EventArgs e)
        {
            RaiseEvent(EventReadOnlyChanged, e);
        }

        public void Paste()
        {
            InnerTextBox.Paste();
        }

        internal void RaiseEvent(object key, EventArgs e)
        {
            var handler = (EventHandler)Events[key];
            if (handler != null) handler(this, e);
        }

        public void ScrollToCaret()
        {
            InnerTextBox.ScrollToCaret();
        }

        public void Select(int start, int length)
        {
            InnerTextBox.Select(start, length);
        }

        public void SelectAll()
        {
            InnerTextBox.SelectAll();
        }

        public void Undo()
        {
            InnerTextBox.Undo();
        }
        #endregion
    }
}