using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common.Win32;
using BitCollectors.WinFormsControls.Internal.ColorComboBoxExInternal;

namespace BitCollectors.WinFormsControls
{
    [DefaultProperty("Color")]
    [DefaultEvent("ColorChanged")]
    public class ColorComboBoxEx : ComboBox
    {
        #region Constants (non-public)
        private const string DefaultColorName = "Black";
        #endregion

        #region Fields
        private Color _color = Color.Transparent;
        private ComboBoxTextBox _comboBoxTextBox;
        private bool _showColorPreview;
        private readonly ColorComboBoxExEditorService _editorService;
        #endregion

        #region Constructor(s)
        public ColorComboBoxEx(Color color)
        {
            Color = color;
            _editorService = new ColorComboBoxExEditorService(this);
        }

        public ColorComboBoxEx()
            : this(Color.FromName(DefaultColorName))
        {
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
                RefreshColor();

                if (ColorChanged != null)
                    ColorChanged(this, new EventArgs());
            }
        }

        public bool ShowColorPreview
        {
            get { return _showColorPreview; }
            set
            {
                if (_showColorPreview != value)
                {
                    _showColorPreview = value;
                    Invalidate();
                }
            }
        }
        #endregion

        #region Overrides
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if ((DropDownStyle == ComboBoxStyle.DropDown) || (DropDownStyle == ComboBoxStyle.Simple))
                _comboBoxTextBox = new ComboBoxTextBox(this, new Rectangle(0, 0, 20, Height - 7));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Down || keyData == Keys.Up)
            {
                Debug.WriteLine("KDOWN");
                ShowDropDown();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_LBUTTONDOWN:
                    Debug.WriteLine("WM_LBUTTONDOWN");
                    ShowDropDown();
                    return;

                case NativeMethods.WM_REFLECT + NativeMethods.WM_COMMAND:
                    switch (NativeMethods.HIWORD((int)m.WParam))
                    {
                        case NativeMethods.CBN_DROPDOWN:
                            Debug.WriteLine("CBN_DROPDOWN");
                            ShowDropDown();
                            return;

                        case NativeMethods.CBN_CLOSEUP:
                            Debug.WriteLine("CBN_CLOSEUP");
                            return;
                    }
                    break;

                case NativeMethods.WM_NCACTIVATE:
                    Debug.WriteLine("WM_NCACTIVATE");
                    break;
            }

            base.WndProc(ref m);
        }
        #endregion

        #region Other methods
        public event EventHandler ColorChanged;

        private void RefreshColor()
        {
            if (_comboBoxTextBox == null)
                return;

            _comboBoxTextBox.SetColor(_color);
            Text = _color.ToString();
            Invalidate();
        }

        private void ShowDropDown()
        {
            try
            {
                var editor = new ColorEditor();

                var newValue = editor.EditValue(_editorService, Color);
                if (!_editorService.Canceled)
                {
                    Color = (Color)newValue;
                }

                DroppedDown = false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
        #endregion
    }
}