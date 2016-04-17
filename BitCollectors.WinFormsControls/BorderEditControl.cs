using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common;

namespace BitCollectors.WinFormsControls
{
    public partial class BorderEditControl : UserControl
    {
        #region Constants (public)
        public enum ColorControlTypes
        {
            SystemAndCustomColorPicker,
            SimpleColorPicker
        }
        #endregion

        #region Fields
        private ColorControlTypes _colorPickerType;
        private bool _isMouseOverPreview;
        #endregion

        #region Constructor(s)
        public BorderEditControl(Border borderValue)
        {
            InitializeComponent();

            ColorPickerType = ColorControlTypes.SystemAndCustomColorPicker;
            ReloadColorInputControl();

            if (borderValue != null)
                BorderValue = borderValue;

            InitSystemCheckboxBinding(_penSizeTextbox);
            InitSystemCheckboxBinding(_colorTextbox);
            //InitSystemCheckboxBinding(_moreColorsButton);
            InitSystemCheckboxBinding(_colorComboBox);
            InitSystemCheckboxBinding(PreviewPicBox, "Visible");
            InitSystemCheckboxBinding(label3, "Visible");

            PreviewPicBox.Paint += pictureBox1_Paint;
        }
        #endregion

        #region Properties
        public Border BorderValue
        {
            get
            {
                var color = Color.White;

                switch (ColorPickerType)
                {
                    case ColorControlTypes.SimpleColorPicker:
                        color = _colorTextbox.Color;
                        break;

                    case ColorControlTypes.SystemAndCustomColorPicker:
                        color = _colorComboBox.Color;
                        break;
                }

                var returnValue = new Border
                {
                    Size = (int)_penSizeTextbox.Value,
                    Color = color
                };

                return returnValue;
            }
            set
            {
                if (value == null)
                    return;

                _penSizeTextbox.Value = value.Size;
                _colorComboBox.Color = value.Color;
                _colorTextbox.Color = value.Color;
            }
        }

        public ColorControlTypes ColorPickerType
        {
            get { return _colorPickerType; }
            set
            {
                if (_colorPickerType == value)
                    return;

                _colorPickerType = value;
                ReloadColorInputControl();
            }
        }
        #endregion

        private void ReloadColorInputControl()
        {
            _colorTextbox.Visible = false;
            _colorComboBox.Visible = false;

            switch (_colorPickerType)
            {
                case ColorControlTypes.SystemAndCustomColorPicker:
                    _colorComboBox.Visible = true;
                    break;

                case ColorControlTypes.SimpleColorPicker:
                    _colorTextbox.Visible = true;
                    break;
            }

        }

        #region Other methods
        private void ColorTextboxTextChanged(object sender, EventArgs e)
        {
            PreviewPicBox.Invalidate();
        }

        //private string GetColorString(Color color)
        //{
        //    return string.Format("{0}, {1}, {2}", color.R, color.G, color.B);
        //}

        private void InitSystemCheckboxBinding(Control controlToDisableWhenChecked, string property = "Enabled")
        {
            var binding = new Binding(property, _systemDefaultCheckbox, "Checked");
            binding.Format += (sender, args) =>
            {
                if (args.Value != null)
                    args.Value = !(bool)args.Value;
            };
            controlToDisableWhenChecked.DataBindings.Add(binding);
        }

        //private void MoreColorsButtonClick(object sender, EventArgs e)
        //{
        //    var dialog = new ColorDialog();
        //    if (dialog.ShowDialog() == DialogResult.OK)
        //    {
        //        _colorTextbox.Text = GetColorString(dialog.Color);
        //    }
        //}

        private void PenSizeNumTextboxValueChanged(object sender, EventArgs e)
        {
            PreviewPicBox.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(BorderValue.Color, BorderValue.Size))
            {
                Debug.WriteLine("Paint " + _isMouseOverPreview);
                if (_isMouseOverPreview)
                {
                    const int squareSize = 16;
                    var iterateHeightCount = PreviewPicBox.Height / squareSize + 1;
                    var iterateWidthCount = PreviewPicBox.Width / squareSize + 1;
                    for (var i = 1; i <= iterateWidthCount; i++)
                    {
                        for (var j = 1; j <= iterateHeightCount; j++)
                        {
                            var backColor = i % 2 != j % 2 ? Brushes.DarkGray : Brushes.White;
                            e.Graphics.FillRectangle(backColor,
                                new Rectangle(squareSize * (i - 1), squareSize * (j - 1), squareSize, squareSize));
                        }
                    }
                }
                e.Graphics.DrawRectangle(pen, 5, 5, PreviewPicBox.Width - 10, PreviewPicBox.Height - 10);
            }
        }

        private void PreviewPicBoxMouseEnter(object sender, EventArgs e)
        {
            label3.Visible = false;
            _isMouseOverPreview = true;
            PreviewPicBox.Invalidate();
        }

        private void PreviewPicBoxMouseLeave(object sender, EventArgs e)
        {
            label3.Visible = true;
            _isMouseOverPreview = false;
            PreviewPicBox.Invalidate();
        }
        #endregion

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}