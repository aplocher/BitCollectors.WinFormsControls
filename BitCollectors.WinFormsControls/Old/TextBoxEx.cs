using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Internal;

namespace BitCollectors.WinFormsControls
{
    public class TextBoxEx : UserControl
    {
        //public static implicit operator System.Windows.Forms.TextBox(TextBoxEx d)
        //{
        //    // look into typeconverters
        //    return d.InnerTextBox;
        //}

        //public static implicit operator TextBoxEx(System.Windows.Forms.TextBox d)
        //{
        //    // look into typeconverters
        //    if (typeof (TextBoxEx) == d.GetType())
        //    {
        //        return (d as EnhancedTextBoxInternal).ParentContainer;
        //    }

        //    throw new InvalidCastException();
        //}

        #region Private Fields
        private const int _height = 23;
        private readonly EnhancedTextBoxInternal _textBox;
        private readonly EnhancedTextBoxButtonInternal _actionButton;
        private string _watermarkText = "";
        private Color _watermarkForeColor = Color.Red;
        private Color _borderColor = Color.FromArgb(171, 173, 179);
        private Color _borderColorFocus = Color.FromArgb(51, 153, 255);
        private Color _borderColorHover = Color.FromArgb(51, 153, 255);
        private int _borderSize = 1;
        private Padding _innerPadding = new Padding(2);
        private int _buttonWidth = 12;
        private int _buttonHeight = 12;
        private Color _buttonBackColor = Color.LightGray;
        private Color _buttonBackColorHover = Color.Teal;
        private Color _buttonForeColor = Color.Teal;
        private Color _buttonForeColorHover = Color.LightGray;
        private Image _buttonImage = null;
        private Image _buttonImageHover = null;
        private bool _isButtonHoverWiredUp = false;
        private Rectangle _innerRectangle = new Rectangle();
        private Color _foreColor = Color.Orange;
        private bool _mouseHovering = false;
        private string _text = "";
        #endregion

        public TextBoxEx()
        {
            // For more info on this: http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (!SystemInformation.TerminalServerSession)
            {
                this.DoubleBuffered = true;
            }

            _textBox = new EnhancedTextBoxInternal(this);
            _actionButton = new EnhancedTextBoxButtonInternal();

            this.SuspendLayout();
            this.BackColor = SystemColors.Window;

            _textBox.BackColor = SystemColors.Window;
            _textBox.TabIndex = 1;

            _actionButton.UseCustomFlatRenderer = true;
            _actionButton.TabStop = false;
            _actionButton.TabIndex = 2;

            RestyleTextBox(false);

            this.Controls.Add(_textBox);
            this.Controls.Add(_actionButton);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #region Forwarded Events
        public new event KeyEventHandler KeyDown
        {
            add { _textBox.KeyDown += value; }
            remove { _textBox.KeyDown -= value; }
        }

        public new event KeyEventHandler KeyUp
        {
            add { _textBox.KeyUp += value; }
            remove { _textBox.KeyUp -= value; }
        }

        public new event KeyPressEventHandler KeyPress
        {
            add { _textBox.KeyPress += value; }
            remove { _textBox.KeyPress -= value; }
        }

        public event EventHandler ActionButtonClick
        {
            add { _actionButton.Click += value; }
            remove { _actionButton.Click -= value; }
        }

        public new event EventHandler GotFocus
        {
            add { _textBox.GotFocus += value; }
            remove { _textBox.GotFocus -= value; }
        }

        public new event EventHandler LostFocus
        {
            add { _textBox.LostFocus += value; }
            remove { _textBox.LostFocus -= value; }
        }
        #endregion

        #region Restyling properties (Border)
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                RestyleTextBox();
            }
        }

        public Color BorderColorFocus
        {
            get { return _borderColorFocus; }
            set
            {
                _borderColorFocus = value;
                RestyleTextBox();
            }
        }

        public Color BorderColorHover
        {
            get { return _borderColorHover; }
            set
            {
                _borderColorHover = value;
                RestyleTextBox();
            }
        }

        [DefaultValue(1)]
        public int BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = value;
                RestyleTextBox();
            }
        }
        #endregion

        #region Restyling properties (Background)
        [DefaultValue("Window")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                if (_textBox != null)
                    _textBox.BackColor = value;
            }
        }
        #endregion

        #region Restyling properties (Button Stuff)
        public string ButtonText
        {
            get { return _actionButton.Text; }
            set { _actionButton.Text = value; }
        }

        public Color ButtonBackColor
        {
            get { return _buttonBackColor; }
            set
            {
                _buttonBackColor = value;
                _actionButton.BackColor = value;
            }
        }

        public Color ButtonBackColorHover
        {
            get { return _buttonBackColorHover; }
            set
            {
                _buttonBackColorHover = value;
                _actionButton.FlatAppearance.MouseOverBackColor = value;
            }
        }

        public Color ButtonForeColor
        {
            get { return _buttonForeColor; }
            set
            {
                _buttonForeColor = value;
                _actionButton.ForeColor = value;
            }
        }

        public Color ButtonForeColorHover
        {
            get { return _buttonForeColorHover; }
            set
            {
                _buttonForeColorHover = value;
            }
        }

        public ContentAlignment ButtonTextAlign
        {
            get { return _actionButton.TextAlign; }
            set { _actionButton.TextAlign = value; }
        }

        public ContentAlignment ButtonImageAlign
        {
            get { return _actionButton.ImageAlign; }
            set { _actionButton.ImageAlign = value; }
        }

        public bool ButtonUseVisualStyleBackColor
        {
            get { return _actionButton.UseVisualStyleBackColor; }
            set { _actionButton.UseVisualStyleBackColor = value; }
        }

        [DefaultValue(80)]
        public int ButtonWidth
        {
            get { return _buttonWidth; }
            set
            {
                _buttonWidth = value;
                RestyleTextBox();
            }
        }

        public Image ButtonImage
        {
            get { return _buttonImage; }
            set
            {
                _buttonImage = value;
                _actionButton.Image = value;
            }
        }

        public Image ButtonImageHover
        {
            get { return _buttonImageHover; }
            set
            {
                _buttonImageHover = value;
                _actionButton.ImageMouseOver = value;
            }
        }

        [DefaultValue(false)]
        public bool ButtonIsTabStop
        {
            get { return _actionButton.TabStop; }
            set { _actionButton.TabStop = value; }
        }

        public bool TextBoxEnterKeyClicksButton { get; set; }
        #endregion

        #region Watermark Properties
        public string WatermarkText
        {
            get { return _watermarkText; }
            set
            {
                _watermarkText = value;
                ShowWatermarkedValue();
            }
        }

        public Color WatermarkTextColor
        {
            get { return _watermarkForeColor; }
            set
            {
                _watermarkForeColor = value;
                ShowWatermarkedValue();
            }
        }
        #endregion

        #region Misc properties
        [DefaultValue(2)]
        public new Padding Padding
        {
            get { return _innerPadding; }
            set
            {
                base.Padding = new Padding(0);
                _innerPadding = value;
                RestyleTextBox();
            }
        }

        public bool EscapeClearsInput
        {
            get { return _textBox.EscapeClearsInput; }
            set { _textBox.EscapeClearsInput = value; }
        }

        public int SelectionStart
        {
            get { return InnerTextBox.SelectionStart; }
            set { InnerTextBox.SelectionStart = value; }
        }

        public int TextLength
        {
            get { return InnerTextBox.TextLength; }
        }
        #endregion

        public bool IsWatermarkDisplayed
        {
            get
            {
                return _watermarkText != null &&
                       (_textBox.Text.Trim().Equals(_watermarkText.Trim(), StringComparison.InvariantCultureIgnoreCase));
            }
        }

        #region Exposed child controls
        [Browsable(false)]
        public System.Windows.Forms.TextBox InnerTextBox
        {
            get { return (System.Windows.Forms.TextBox)_textBox; }
        }

        [Browsable(false)]
        public Button InnerActionButton
        {
            get { return _actionButton; }
        }


        #endregion

        #region Overrides and Inner Events
        public override bool Focused
        {
            // See http://stackoverflow.com/questions/24602727/toolstrip-with-custom-toolstripcontrolhost-makes-tab-order-focus-act-weird/24722497
            get { return _textBox.Focused; }
        }

        public override Font Font
        {
            get
            {
                // Must return base.Font since it cascades down. Will cause VS to freeze if 
                // we return _textBox.Font. 
                return base.Font;
            }
            set
            {
                base.Font = value;
                _textBox.Font = value;
            }
        }

        [DefaultValue("WindowText")]
        public override Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            WireUpInternalEvents();
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get
            {
                return WatermarkParsedString(_text);
            }
            set
            {
                _text = value;

                if (WatermarkParsedString(value) == string.Empty)
                {
                    ShowWatermarkedValue();
                }
                else
                {
                    HideWatermarkedValue();
                }
            }
        }

        private string WatermarkParsedString(string value)
        {
            if (value == null)
                return string.Empty;

            if (_watermarkText == null)
                return value;

            if (value.Trim().Equals(_watermarkText.Trim(), StringComparison.InvariantCultureIgnoreCase))
                return string.Empty;

            return value;
        }

        private void ShowWatermarkedValue()
        {
            if (_textBox.Focused || this.Text.Trim() != "")
                return;

            _textBox.ForeColor = WatermarkTextColor;
            _textBox.Text = WatermarkText;
        }

        private void HideWatermarkedValue()
        {
            _textBox.ForeColor = _foreColor;

            if (IsWatermarkDisplayed)
            {
                _textBox.Text = string.Empty;
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var returnValue = base.GetPreferredSize(proposedSize);

            returnValue.Height = _height;

            return returnValue;
        }

        public override Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = new Size(value.Width, _height); }
        }

        protected override Size DefaultMaximumSize
        {
            get { return new Size(0, _height); }
        }

        protected override Size DefaultSize
        {
            get { return new Size(100, _height); }
        }

        protected override Size DefaultMinimumSize
        {
            get { return new Size(20, _height); }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            RefreshInnerRectangleSize();
            this.Invalidate(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_borderSize > 0)
            {
                var graphics = e.Graphics;

                graphics.Clear(_textBox.BackColor);

                var safeRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

                Color borderColor;
                if (_mouseHovering)
                {
                    borderColor = _borderColorHover;
                }
                else
                {
                    borderColor = _textBox.Focused || _actionButton.Focused
                        ? _borderColorFocus
                        : _borderColor;
                }

                using (var borderPen = new Pen(borderColor, (float)this.BorderSize))
                {
                    graphics.DrawRectangle(borderPen, safeRectangle);
                }
            }

            base.OnPaint(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            GotFocusInternal();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            LostFocusInternal();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            CheckMouseHoverState();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            CheckMouseHoverState();
        }
        #endregion

        #region Private methods
        private void GotFocusInternal()
        {
            if (_textBox.Focused)
            {
                HideWatermarkedValue();
            }

            Invalidate(false);
        }

        private void LostFocusInternal()
        {
            if (!_textBox.Focused)
            {
                ShowWatermarkedValue();
            }

            if (!_textBox.Focused && !_actionButton.Focused && !Focused)
            {
                this.Refresh();
            }
        }

        private void WireUpInternalEvents()
        {
            if (_isButtonHoverWiredUp)
                return;

            _actionButton.MouseEnter += (sender, args) => CheckMouseHoverState();
            _actionButton.MouseLeave += (sender, args) => CheckMouseHoverState();

            _textBox.MouseEnter += (sender, args) => CheckMouseHoverState();
            _textBox.MouseLeave += (sender, args) => CheckMouseHoverState();

            _textBox.GotFocus += (sender, args) => GotFocusInternal();
            _textBox.LostFocus += (sender, args) => LostFocusInternal();

            _actionButton.GotFocus += (sender, args) => GotFocusInternal();
            _actionButton.LostFocus += (sender, args) => LostFocusInternal();

            _isButtonHoverWiredUp = true;
        }

        private void CheckMouseHoverState()
        {
            bool hovering = (GetChildAtPoint(PointToClient(MousePosition)) != null);

            if (_mouseHovering != hovering)
            {
                this.Invalidate();
                _mouseHovering = hovering;
            }
        }

        private void RestyleTextBox(bool suspend = true)
        {
            if (suspend)
            {
                this.SuspendLayout();
            }

            base.Padding = new Padding(0);

            _textBox.BorderStyle = BorderStyle.None;
            _textBox.Multiline = false;
            _textBox.Padding = new Padding(0);
            _textBox.Margin = new Padding(0);
            _textBox.MinimumSize = new Size(10, 5);
            _textBox.ForeColor = _foreColor;
            _textBox.BackColor = this.BackColor;
            _textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            _actionButton.FlatStyle = FlatStyle.Flat;
            _actionButton.FlatAppearance.BorderSize = 0;
            _actionButton.BackColor = _buttonBackColor;
            _actionButton.ForeColor = _buttonForeColor;

            _actionButton.Padding = new Padding(0);
            _actionButton.Margin = new Padding(0);
            _actionButton.MinimumSize = new Size(5, 5);
            _actionButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right;

            RefreshInnerRectangleSize();

            _textBox.Size = new Size(_innerRectangle.Width - _buttonWidth, _innerRectangle.Height);
            _textBox.Location = new Point(_innerRectangle.Left, (this.Height / 2) - (_textBox.Height / 2));

            _actionButton.Size = new Size(_buttonWidth, _buttonHeight);
            _actionButton.Location = new Point(_innerRectangle.Right - _buttonWidth, (int)Math.Ceiling(((decimal)this.Height / 2) - ((decimal)_buttonHeight / 2)));

            this.SetStyle(ControlStyles.FixedHeight, true);
            this.Invalidate();

            if (suspend)
            {
                this.ResumeLayout(false);
                this.PerformLayout();
            }
        }

        private void RefreshInnerRectangleSize()
        {
            _innerRectangle = new Rectangle(
                _innerPadding.Left + (_borderSize / 2) + 1,
                _innerPadding.Top + (_borderSize / 2) + 1,
                (this.Width - _innerPadding.Right - _innerPadding.Left - _borderSize) - 1,
                (this.Height - _innerPadding.Bottom - _innerPadding.Top - _borderSize) - 1);
        }
        #endregion
    }
}
