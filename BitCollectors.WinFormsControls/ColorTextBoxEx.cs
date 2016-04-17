using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common.Win32;

namespace BitCollectors.WinFormsControls
{
    public class ColorTextBoxEx : TextBoxEx
    {
        #region Constants (non-public)
        private const string DefaultColorName = "Black";
        #endregion

        #region Fields
        private Color _color;
        private Image _moreColorsImage;
        private Image _moreColorsImageMouseOver;
        private bool _showColorPreview;
        #endregion

        #region Constructor(s)
        public ColorTextBoxEx()
            : this(Color.FromName(DefaultColorName))
        {
        }

        public ColorTextBoxEx(Color color)
        {
            //this.SetStyle(ControlStyles.UserPaint, true);
            Color = color;
            //ButtonImage = null;
            //ButtonImageMouseOver = null;
            //ButtonText = "...";
            //UNDO
            //base.Padding = new Padding(40, 0, 0, 0);

            var colorPreview = new PictureBox
            {
                Size = new Size(21, Height - 6),
                Location = new Point(1, 1)
            };

            colorPreview.Paint += (sender, args) =>
            {
                args.Graphics.FillRectangle(new SolidBrush(Color), ColorSquareDimensions);
                args.Graphics.DrawRectangle(Pens.Black, ColorSquareDimensions);
            };

            Controls.Add(colorPreview);


            RefreshMargin();
            RefreshColor();
        }
        #endregion

        #region Properties
        [Description("The currently selected color.")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), DefaultColorName)]
        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                    return;

                _color = value;
                InnerColor = value;
                RefreshColor();

                if (ColorChanged != null)
                    ColorChanged(this, new EventArgs());
            }
        }

        public Image MoreColorsImage
        {
            get { return _moreColorsImage; }
            set
            {
                if (_moreColorsImage == value)
                    return;

                _moreColorsImage = value;
                Invalidate();
            }
        }

        public Image MoreColorsImageMouseOver
        {
            get { return _moreColorsImageMouseOver; }
            set
            {
                if (_moreColorsImageMouseOver == value)
                    return;

                _moreColorsImageMouseOver = value;
                Invalidate();
            }
        }

        public bool ShowColorPreview
        {
            get { return _showColorPreview; }
            set
            {
                if (_showColorPreview == value)
                    return;

                _showColorPreview = value;
                Invalidate();
            }
        }

        private Rectangle ColorSquareDimensions { get; set; }

        private Color InnerColor
        {
            get
            {
                // Format (whitespace ok): {255, 255, 255}
                const string rgbRegex = @"^\s*\{?\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\}?\s*$";

                // Format (whitespace ok): {r:255, g:255, b:255} 
                const string rgbRegex2 =
                    @"^\s*\{?\s*r\s*\:\s*([0-9]{1,3})\s*,\s*g\s*\:\s*([0-9]{1,3})\s*,\s*b\s*\:\s*([0-9]{1,3})\s*\}?\s*$";

                Match match = null;
                if (Regex.IsMatch(Text, rgbRegex))
                    match = Regex.Match(Text, rgbRegex);
                else if (Regex.IsMatch(Text, rgbRegex2))
                    match = Regex.Match(Text, rgbRegex2);

                if (match == null)
                    return Color.Transparent;

                var red = int.Parse(match.Groups[1].Value);
                var green = int.Parse(match.Groups[2].Value);
                var blue = int.Parse(match.Groups[3].Value);

                return Color.FromArgb(red, green, blue);
            }
            set
            {
                if (value.IsKnownColor)
                {

                }
                else if (value.IsNamedColor)
                {

                }
                else if (value.IsSystemColor)
                {

                }
                else
                {
                    Text = string.Format("{{r:{0}, g:{1}, b:{2}}}", value.R, value.G, value.B);
                }

                Text += "hi";
            }
        }
        #endregion
        // Undo
        //#region Overrides
        //protected override void OnActionButtonClicked()
        //{
        //    base.OnActionButtonClicked();

        //    var form = new ColorDialog();
        //    if (form.ShowDialog() == DialogResult.OK)
        //    {
        //        Color = form.Color;
        //    }
        //}
        //#endregion

        #region Other methods
        public event EventHandler ColorChanged;

        private void RefreshColor()
        {
            ColorSquareDimensions = new Rectangle(0, 0, 20, Height - 7);
            Text = _color.ToString();
            Invalidate();
        }

        private void RefreshMargin()
        {
            NativeMethods.SendMessage(Handle, NativeMethods.EM_SETMARGINS, NativeMethods.EC_LEFTMARGIN, 26);
        }
        #endregion
    }
}