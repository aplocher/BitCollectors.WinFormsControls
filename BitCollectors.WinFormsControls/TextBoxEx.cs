﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common;
using BitCollectors.WinFormsControls.Common.Win32;
using BitCollectors.WinFormsControls.ControlExtensions;
using BitCollectors.WinFormsControls.Properties;

/*
 * Features:
 * - Watermark
 * - Escape btn clears
 * - Action button
 * - Border customizations
 * - Consider adding extensions for Numeric input (similar to the Color ext control)
 */

// Phase 1:
// TODO 11/6, 11/7, finish this phase 1 implementation 
// TODO: (phase1) Implement TextBoxButtonActions
// TODO: (phase1) Border customizations
// Phase 2:
// TODO: (phase2) consider adding an extension control for numeric input (similar to the color ext control, maybe look at that moca.net thing) - http://stackoverflow.com/questions/463299/how-do-i-make-a-textbox-that-only-accepts-numbers?rq=1
// TODO: (phase2) consider allowing multiple action buttons (like a Clear and a Go), using an internal FlowLayoutPanel?
// TODO: (phase2) Nmeumonics from watermark
// TODO: (phase2) Use ContentAlignment enum for action button position (ButtonPosition property [topright, middleright, etc] adn ButtonMargin)
// TODO: (phase1) Fix designer bug with Text/WatermarkText properties [moved to phase2]
// Breadcrumb navigation (like WIndows Explorer AB)

namespace BitCollectors.WinFormsControls
{
    public class TextBoxEx : TextBox
    {
        #region Fields
        private BorderStyleEx _borderStyle;
        private Image _buttonImage;
        private Size _buttonSize;
        private Border _customBorder;
        private TextBoxBorderPainter _customPaintTextBox;
        private string _designText = "";
        private Color _foreColor;
        private Rectangle _innerRectangle;
        private bool _useCustomBorder;
        private Color _watermarkForeColor;
        private string _watermarkText;
        private readonly ButtonEx _actionButton;
        private Border _customBorderFocus;
        private Border _customBorderMouseOver;

        private bool _allowLayoutExtras;
        #endregion

        public new bool Multiline
        {
            get { return _multiline; }
            set
            {
                _multiline = value;
                base.Multiline = _multiline || _allowLayoutExtras;

                if (!_multiline && _allowLayoutExtras)
                {
                    base.AcceptsReturn = false;
                    base.AcceptsTab = false;
                    base.WordWrap = false;
                    base.ScrollBars = ScrollBars.None;
                }
            }
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_allowLayoutExtras && !_multiline)
                    {
                        e.Handled = true;
                        return;
                    }
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (_allowLayoutExtras && !_multiline)
                    {
                        e.Handled = true;
                        return;
                    }
                    break;
            }

            base.OnKeyUp(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        { // http://www.xtremedotnettalk.com/showthread.php?t=75210
            switch (e.KeyChar)
            {
                case '\r':
                case '\n':
                    if (_allowLayoutExtras && !_multiline)
                    {
                        e.Handled = true;
                        return;
                    }
                    break;
            }

            base.OnKeyPress(e);
        }
        //public override Size MinimumSize
        //{
        //    get { return base.MinimumSize; }
        //    set { base.MinimumSize = value; }
        //}

        //public override Size GetPreferredSize(Size proposedSize)
        //{
        //    return base.GetPreferredSize(proposedSize);
        //}

        //const int RequestedHight = 40;

        //protected override void OnSizeChanged(EventArgs e)
        //{
        //    base.OnSizeChanged(e);
        //    AssureRequestedHight();
        //}

        //protected override void OnCreateControl()
        //{
        //    base.OnCreateControl();
        //    //AssureRequestedHight();
        //}

        //private void AssureRequestedHight()
        //{
        //    if (this.Size.Height != RequestedHight && !this.Multiline)
        //    {
        //        this.Multiline = true;
        //        this.MinimumSize = new Size(0, RequestedHight);
        //        this.Size = new Size(this.Size.Width, RequestedHight);
        //        this.Multiline = false;
        //    }
        //}

        #region Constructor(s)
        public TextBoxEx()
        {
            base.Text = "";

            _borderStyle = BorderStyleEx.Fixed3D;
            _foreColor = base.ForeColor;
            _watermarkText = "";
            _watermarkForeColor = SystemColors.GrayText;

            _actionButton = new ButtonEx {
                Anchor = AnchorStyles.Right,
                Cursor = Cursors.Arrow,
                FlatStyle = FlatStyleEx.FlatEx,
                Font = DefaultFont,
                Image = Resources.ClearButton12x12,
                ImageMouseOver = Resources.ClearButtonHover12x12,
                Margin = new Padding(0),
                MinimumSize = new Size(0, 0),
                Padding = new Padding(0),
                Size = new Size(12, 12),
                TabIndex = 2,
                TabStop = false
            };

            _actionButton.Click += (sender, args) => OnActionButtonClicked(new CancelEventArgs());

            Controls.Add(_actionButton);

            RefreshWatermarkAppearance();
            RefreshInnerControlLayout(true);

            //base.Text += "\r\n\r\n\r\nTest123\r\n\r\n\r\n";

            //_components = new Container();

            //_graphicalOverlay = new GraphicalOverlay(_components);
            //_graphicalOverlay.Paint += (sender, args) => {
            //    args.Graphics.DrawRectangle(Pens.DarkRed, 2, 2, 30, 30);
            //    args.Graphics.DrawRectangle(Pens.DarkRed, 1, 2, 30, 30);
            //    args.Graphics.DrawRectangle(Pens.DarkRed, 3, 3, 30, 30);
            //};

            //_graphicalOverlay.Owner = this;
            //_customPaintTextBox = new TextBoxBorderPainter(Handle, Parent.Handle);
            //SetTextBoxFormatingRectangle(new Rectangle(0, 0, 100, 100));
            //NativeMethods.SendMessage(Handle, NativeMethods.EM_SETRECT, NativeMethods.EC_LEFTMARGIN, ColorSquareDimensions.Width + 4);
        }
        #endregion

        //private void SetTextBoxFormatingRectangle(Rectangle rect)
        //{
        //    NativeMethods.RECT rc = new NativeMethods.RECT(rect);
        //    //NativeMethods.SendMessage(Handle, NativeMethods.EM_SETRECT, 0, ref rc);
        //}

        #region Properties
        [Category("Border")]
        [DefaultValue(typeof(BorderStyleEx), "Fixed3D")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new BorderStyleEx BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;

                switch (value)
                {
                    case BorderStyleEx.None:
                    case BorderStyleEx.Custom:
                        base.BorderStyle = System.Windows.Forms.BorderStyle.None;
                        break;

                    case BorderStyleEx.Fixed3D:
                        base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                        break;

                    case BorderStyleEx.FixedSingle:
                        base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        break;
                }

                _useCustomBorder = (_borderStyle == BorderStyleEx.Custom);

                base.AutoSize = !_useCustomBorder;
                if (_useCustomBorder)
                {
                    this.Padding = new Padding(6);
                    base.Padding = new Padding(6);
                    base.Margin = new Padding(6);
                    RefreshMargin();
                }
            }
        }

        [Category("Action Button")]
        [DefaultValue(typeof(Image), "Resources.ClearButton12x12")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Image ButtonImage
        {
            get { return _actionButton.Image; }
            set { _actionButton.Image = value; }
        }

        [Category("Action Button")]
        [DefaultValue(typeof(Image), "Resources.ClearButtonHover12x12")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Image ButtonImageMouseOver
        {
            get { return _actionButton.ImageMouseOver; }
            set { _actionButton.ImageMouseOver = value; }
        }

        [Category("Action Button")]
        [DefaultValue(typeof(Size), "12, 12")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Size ButtonSize
        {
            get { return _actionButton.Size; }
            set
            {
                _actionButton.Size = value;
                RefreshInnerControlLayout();
            }
        }

        [Category("Action Button")]
        [DefaultValue("")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string ButtonText { get; set; }

        [Category("Action Button")]
        [DefaultValue(typeof(Font), "Control.DefaultFont")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Font ButtonTextFont
        {
            get { return _actionButton.Font; }
            set { _actionButton.Font = value; }
        }

        [Category("Border")]
        [DefaultValue(typeof(Border), null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Editor(typeof(BorderUiTypeEditor), typeof(UITypeEditor))]
        public Border CustomBorder
        {
            get { return _customBorder; }
            set { CustomBorderSetter(_customBorder, value); }
        }

        [Category("Border")]
        [DefaultValue(typeof(Border), null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Border CustomBorderFocus
        {
            get { return _customBorderFocus; }
            set { CustomBorderSetter(_customBorderFocus, value); }
        }

        [Category("Border")]
        [DefaultValue(typeof(Border), null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Border CustomBorderMouseOver
        {
            get { return _customBorderMouseOver; }
            set { CustomBorderSetter(_customBorderMouseOver, value); }
        }

        [Category("Layout Extras")]
        [DefaultValue(false)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [RefreshProperties(RefreshProperties.All)]
        public bool AllowLayoutExtras
        {
            get { return _allowLayoutExtras; }
            set
            {
                _allowLayoutExtras = value;

                try
                {
                    //var desc = this.LayoutExtras.GetType().GetProperties(this.LayoutExtras.GetType())["LayoutExtras"];
                    //var attr = (ReadOnlyAttribute)desc.Attributes[typeof (ReadOnlyAttribute)];

                    var descriptor = TypeDescriptor.GetProperties(this.LayoutExtras.GetType())["Border"];
                    var attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];
                    var isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);

                    isReadOnly?.SetValue(attrib, !value);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        [Category("Layout Extras")]
        [DefaultValue(false)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBoxLayoutExtras LayoutExtras
        {
            get { return _layoutExtras; }
        }

        private void CustomBorderSetter(Border border, Border newValue)
        {
            if (border != newValue)
            {
                _customBorder = newValue;
            }


            RefreshBorderAppearance();

        }

        private void RefreshBorderAppearance()
        {
            InitializeBorderPainter();

            if (ContainsFocus && CustomBorderFocus != null)
            {
                _customPaintTextBox.Border = CustomBorderFocus;
            }
            else if (_isMouseOver && CustomBorderMouseOver != null)
            {
                _customPaintTextBox.Border = CustomBorderMouseOver;
            }
            else if (CustomBorder != null)
            {
                _customPaintTextBox.Border = CustomBorder;
            }
        }

        private bool _isMouseOver = false;
        private bool _multiline;
        private TextBoxLayoutExtras _layoutExtras = new TextBoxLayoutExtras();

        //public bool ShouldSerializeButtonTextFont() { return !Equals(_actionButton.Font, Control.DefaultFont); }
        //public void ResetMyFont() { _actionButton.Font = DefaultFont; }

        [Category("Behavior")]
        [DefaultValue(false)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool EscapeKeyClearsInput { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "WindowText")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new virtual Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                RefreshWatermarkAppearance();
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new string Text
        {
            get
            {
                if (DesignMode)
                {
                    return _designText;
                }
                return TextWithoutWatermark(base.Text);
            }
            set
            {
                if (DesignMode)
                {
                    _designText = value;
                    return;
                }
                base.Text = TextWithWatermark(value);
                RefreshWatermarkAppearance();
            }
        }

        //[Category("Action Button")]
        //[DefaultValue(typeof(TextBoxButtonActions), "Clear")]
        //[Browsable(true)]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        //public TextBoxButtonActions ButtonAction { get; set; }

        [Category("Watermark")]
        [DefaultValue(typeof(Color), "GrayText")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color WatermarkForeColor
        {
            get { return _watermarkForeColor; }
            set
            {
                _watermarkForeColor = value;
                RefreshWatermarkAppearance();
            }
        }

        [Category("Watermark")]
        [DefaultValue("")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string WatermarkText
        {
            get { return _watermarkText; }
            set
            {
                _watermarkText = value;
                RefreshWatermarkAppearance();
            }
        }

        private void InitializeBorderPainter()
        {
            if (BorderStyle != BorderStyleEx.Custom)
            {
                if (_customPaintTextBox != null)
                {
                    _customPaintTextBox.Dispose();
                    _customPaintTextBox = null;
                }
                return;
            }

            if (_customPaintTextBox == null)
            {
                _customPaintTextBox = new TextBoxBorderPainter(this) { Border = CustomBorder };
            }
        }
        #endregion

        #region Overrides
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_customPaintTextBox != null)
            {
                _customPaintTextBox.Dispose();
                _customPaintTextBox = null;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if ((base.Text ?? "").Trim().Equals(WatermarkText, StringComparison.OrdinalIgnoreCase))
            {
                base.Text = "";
                RefreshWatermarkAppearance();
            }

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (string.IsNullOrEmpty(base.Text))
            {
                base.Text = WatermarkText;
                RefreshWatermarkAppearance();
            }

            base.OnLostFocus(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            RefreshInnerControlLayout();

            Rectangle rect = new Rectangle();
            NativeMethods.SendMessage(Handle, NativeMethods.EM_GETRECT, (IntPtr)0, ref rect);

            Console.WriteLine(rect);

            rect.Y = rect.Height/2;
            NativeMethods.SendMessage(Handle, NativeMethods.EM_SETRECT, (IntPtr)0, ref rect);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && Focused && EscapeKeyClearsInput)
            {
                base.Text = string.Empty;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Other methods
        public event CancelEventHandler ActionButtonClicked;

        protected virtual void OnActionButtonClicked(CancelEventArgs e)
        {
            if (ActionButtonClicked != null)
                ActionButtonClicked(this, e);

            if (!e.Cancel)
            {
                Clear();
                Focus();
            }
        }

        private void RefreshInnerControlLayout(bool forceLayout = false)
        {
            // http://msdn.microsoft.com/en-us/library/system.windows.forms.systeminformation.bordersize(v=vs.110).aspx
            // http://msdn.microsoft.com/en-us/library/system.windows.forms.textboxbase.borderstyle(v=vs.110).aspx
            var borderSize = _borderStyle == BorderStyleEx.Custom
                ? (_customPaintTextBox == null || _customPaintTextBox.Border == null ? 0 : _customPaintTextBox.Border.Size)
                : 2;

            var currentInnerRectangle = _innerRectangle;

            _innerRectangle = new Rectangle(
                Padding.Left + (borderSize / 2) + 1,
                Padding.Top + (borderSize / 2) + 1,
                (Width - Padding.Right - Padding.Left - borderSize) - 1,
                (Height - Padding.Bottom - Padding.Top - borderSize) - 1);

            if (!forceLayout && _innerRectangle == currentInnerRectangle)
                return;

            SuspendLayout();

            _actionButton.Location = new Point(_innerRectangle.Right - ButtonSize.Width - 4, _innerRectangle.Y);

            ResumeLayout();
        }

        private void RefreshWatermarkAppearance()
        {
            if (DesignMode)
                return;

            Color newColor;
            string newText;

            if (Focused || ContainsFocus)
            {
                newColor = _foreColor;
                newText = "";
            }
            else
            {
                newColor = WatermarkForeColor;
                newText = WatermarkText;
            }

            base.ForeColor = newColor;
            base.Text = newText;
        }

        private string TextWithoutWatermark(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (string.IsNullOrEmpty(_watermarkText))
                return value;

            return value.Trim().Equals(_watermarkText.Trim(), StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : value;
        }

        private string TextWithWatermark(string value)
        {
            if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(WatermarkText))
                return WatermarkText;

            return value;
        }
        #endregion

        #region Nested Classes
        private class TextBoxBorderPainter : NativeWindow, IDisposable
        {
            #region Fields
            //private Pen _borderPenFocus;
            private Pen _borderPen;
            //private Pen _borderPenMouseOver;
            //private Border _customBorder;
            //private Border _customBorderMouseOver;
            //private Border _customBorderFocus;
            private Border _border;
            #endregion

            #region Constructor(s)
            public TextBoxBorderPainter(TextBoxEx textBoxEx)
            {
                AssignHandle(textBoxEx.Handle);
                TextBoxEx = textBoxEx;
            }
            #endregion

            //private void CustomBorderSetter(Border border, Border value)
            //{
            //    if (border != value)
            //    {
            //        border = value;
            //        if (_borderPen == null)
            //        {
            //            _borderPen = new Pen(value.Color, value.Size);
            //        }
            //        else
            //        {
            //            _borderPen.Color = value.Color;
            //            _borderPen.Width = value.Size;
            //        }
            //    }
            //}

            #region Properties
            public Border Border
            {
                get { return _border; }
                set
                {
                    if (_border != value)
                    {
                        _border = value;
                        _borderPen = new Pen(_border.Color, _border.Size * 2);
                        TextBoxEx.Invalidate();
                    }
                }
            }

            //public Border CustomBorder
            //{
            //    get { return _customBorder; }
            //    set
            //    {
            //        CustomBorderSetter(_customBorder, value);
            //        _customBorder = value;
            //    }
            //}

            //public Border CustomBorderFocus
            //{
            //    get { return _customBorderFocus; }
            //    set { CustomBorderSetter(_customBorderFocus, value); }
            //}

            //public Border CustomBorderMouseOver
            //{
            //    get { return _customBorderMouseOver; }
            //    set { CustomBorderSetter(_customBorderMouseOver, value); }
            //}

            public TextBoxEx TextBoxEx { get; private set; }
            #endregion

            #region
            public void Dispose()
            {
                if (_borderPen != null)
                {
                    _borderPen.Dispose();
                    _borderPen = null;
                }

                //if (_borderPenMouseOver != null)
                //{
                //    _borderPenMouseOver.Dispose();
                //    _borderPenMouseOver = null;
                //}

                //if (_borderPenFocus != null)
                //{
                //    _borderPenFocus.Dispose();
                //    _borderPenFocus = null;
                //}
            }
            #endregion

            #region Overrides
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                switch (m.Msg)
                {
                    case NativeMethods.WM_PAINT:
                        CustomPaint();
                        break;
                }
            }
            #endregion

            #region Other methods
            private void CustomPaint()
            {
                if (_borderPen == null)
                    return;

                var textBoxGraphics = Graphics.FromHwnd(TextBoxEx.Parent.Handle);
                textBoxGraphics.DrawRectangle(_borderPen, new Rectangle(TextBoxEx.Location, TextBoxEx.Size));
            }
            #endregion


            //const int RequestedHight = 40;

            //protected override void OnSizeChanged(EventArgs e)
            //{
            //    base.OnSizeChanged(e);
            //    AssureRequestedHight();
            //}

            //protected override void OnCreateControl()
            //{
            //    base.OnCreateControl();
            //    //AssureRequestedHight();
            //}

            //private void AssureRequestedHight()
            //{
            //    if (this.Size.Height != RequestedHight && !this.Multiline)
            //    {
            //        this.Multiline = true;
            //        this.MinimumSize = new Size(0, RequestedHight);
            //        this.Size = new Size(this.Size.Width, RequestedHight);
            //        this.Multiline = false;
            //    }
            //}
        }
        #endregion

        private void RefreshMargin()
        {
            NativeMethods.SendMessage(Handle, NativeMethods.EM_SETMARGINS, NativeMethods.EC_LEFTMARGIN, 24);
        }
    }
}