using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace BitCollectors.WinFormsControls
{
    public class ButtonEx : Button
    {
        #region Fields
        private bool _isMouseDown;
        private bool _isMouseMouseOver;
        private VisualStyleRenderer _renderer;
        private bool _useCustomFlatRenderer;
        private FlatStyleEx _flatStyle;
        #endregion

        public new FlatStyleEx FlatStyle
        {
            get { return _flatStyle; }
            set
            {
                _flatStyle = value;

                switch (value)
                {
                    case FlatStyleEx.FlatEx:
                    case FlatStyleEx.Flat:
                        base.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        break;

                    case FlatStyleEx.Popup:
                        base.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                        break;

                    case FlatStyleEx.Standard:
                        base.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
                        break;

                    case FlatStyleEx.System:
                        base.FlatStyle = System.Windows.Forms.FlatStyle.System;
                        break;
                }

                _useCustomFlatRenderer = (value == FlatStyleEx.FlatEx);

                Invalidate();
            }
        }

        public ButtonEx()
        {
            CustomFlatRendererTextPadding = new Padding(0);

            // Taxes: Remote Desktop Connection and painting
            // http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (!SystemInformation.TerminalServerSession)
            {
                DoubleBuffered = true;
            }


        }

        #region Properties
        public Padding CustomFlatRendererTextPadding { get; set; }

        [Category("Appearance (FlatEx)")]
        [DefaultValue(null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Image ImageMouseOver { get; set; }

        [Category("Appearance (FlatEx)")]
        [DefaultValue(null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string TextMouseOver { get; set; }
        #endregion

        #region Overrides
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            _isMouseDown = true;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            _isMouseMouseOver = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _isMouseMouseOver = false;
            _isMouseDown = false;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            _isMouseDown = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_useCustomFlatRenderer)
            {
                DrawClear(e.Graphics);

                if (_isMouseMouseOver && ImageMouseOver != null)
                {
                    DrawButtonImage(e.Graphics, ImageMouseOver);
                }
                else if (Image != null)
                {
                    DrawButtonImage(e.Graphics, Image);
                }

                if (_isMouseMouseOver && !string.IsNullOrWhiteSpace(TextMouseOver))
                {
                    DrawButtonText(e.Graphics, TextMouseOver, Font, ForeColor);
                }
                else if (!string.IsNullOrWhiteSpace(Text))
                {
                    DrawButtonText(e.Graphics, Text, Font, ForeColor);
                }
            }

            if (Focused && Application.RenderWithVisualStyles && FlatStyle == FlatStyleEx.Standard)
            {
                if (_renderer == null)
                {
                    var styleElement = VisualStyleElement.Button.PushButton.Normal;
                    _renderer = new VisualStyleRenderer(styleElement.ClassName, styleElement.Part,
                        (int) PushButtonState.Normal);
                }

                var rectangle = _renderer.GetBackgroundContentRectangle(e.Graphics, ClientRectangle);
                rectangle.Height--;
                rectangle.Width--;

                using (var p = new Pen(Brushes.DarkGray))
                {
                    e.Graphics.DrawRectangle(p, rectangle);
                }
            }
        }
        #endregion

        private void DrawClear(Graphics graphics)
        {
            Color backColor;

            if (_isMouseDown)
                backColor = FlatAppearance.MouseDownBackColor;
            else if (_isMouseMouseOver)
                backColor = FlatAppearance.MouseOverBackColor;
            else
                backColor = BackColor;

            graphics.Clear(backColor);
        }

        private void DrawButtonText(Graphics graphics, string text, Font font, Color foreColor)
        {
            var halfWidth = Width / 2;
            var halfHeight = Height / 2;

            var textSize = graphics.MeasureString(text, font);
            var halfTextWidth = textSize.Width / 2;
            var halfTextHeight = textSize.Height / 2;

            var newPositionX = halfWidth - halfTextWidth;
            var newPositionY = halfHeight - halfTextHeight;

            using (var foreColorBrush = new SolidBrush(foreColor))
            {
                graphics.DrawString(text, font, foreColorBrush, newPositionX, newPositionY);
            }
        }

        private void DrawButtonImage(Graphics graphics, Image image)
        {
            var x = 0;
            var y = 0;

            switch (ImageAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    y = 0;
                    break;

                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    y = (Height / 2) - (Image.Height / 2);
                    break;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    y = Height - Image.Height;
                    break;
            }

            switch (ImageAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    x = 0;
                    break;

                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    x = (Width / 2) - (Image.Width / 2);
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    x = Width - Image.Width;
                    break;
            }

            graphics.DrawImage(image, x, y, Width, Height);
        }
    }
}