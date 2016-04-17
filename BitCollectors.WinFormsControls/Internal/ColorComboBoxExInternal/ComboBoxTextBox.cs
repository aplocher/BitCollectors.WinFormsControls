using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common.Win32;

namespace BitCollectors.WinFormsControls.Internal.ColorComboBoxExInternal
{
    internal sealed class ComboBoxTextBox : NativeWindow
    {
        #region Fields
        private readonly NativeMethods.ComboBoxInfo _comboBoxInfo;
        private readonly ComboBox _comboBoxOwner;
        #endregion

        #region Constructor(s)
        public ComboBoxTextBox(ComboBox owner, Rectangle colorSquareDimensions)
        {
            SetColor(Color.Black);
            _comboBoxOwner = owner;
            ColorSquareDimensions = colorSquareDimensions;
            _comboBoxInfo.cbSize = Marshal.SizeOf(_comboBoxInfo);
            NativeMethods.GetComboBoxInfo(_comboBoxOwner.Handle, ref _comboBoxInfo);

            //if (!Handle.Equals(IntPtr.Zero))
            if (Handle != IntPtr.Zero)
            {
                ReleaseHandle();
            }

            AssignHandle(_comboBoxInfo.hwndEdit);
            RefreshMargin();
        }
        #endregion

        #region Properties
        public Color Color { get; set; }
        public Rectangle ColorSquareDimensions { get; set; }
        #endregion

        #region Overrides
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case NativeMethods.WM_PAINT:
                case NativeMethods.WM_LBUTTONDOWN:
                case NativeMethods.WM_RBUTTONDOWN:
                case NativeMethods.WM_MBUTTONDOWN:
                case NativeMethods.WM_LBUTTONUP:
                case NativeMethods.WM_RBUTTONUP:
                case NativeMethods.WM_MBUTTONUP:
                case NativeMethods.WM_LBUTTONDBLCLK:
                case NativeMethods.WM_RBUTTONDBLCLK:
                case NativeMethods.WM_MBUTTONDBLCLK:
                case NativeMethods.WM_KEYDOWN:
                case NativeMethods.WM_KEYUP:
                case NativeMethods.WM_CHAR:
                case NativeMethods.WM_GETTEXTLENGTH:
                case NativeMethods.WM_GETTEXT:
                case NativeMethods.WM_SETFOCUS:
                case NativeMethods.WM_KILLFOCUS:
                    DrawColorInTextBox();
                    break;

                case NativeMethods.WM_MOUSEMOVE:
                    // Only process if mouse button is down:
                    if (!m.WParam.Equals(IntPtr.Zero))
                    {
                        DrawColorInTextBox();
                    }
                    break;
            }
        }
        //http://www.vbaccelerator.com/home/NET/Code/Controls/ListBox_and_ComboBox/TextBox_Icon/article.asp
        #endregion

        #region Other methods
        private void DrawColorInTextBox()
        {
            using (var graphics = Graphics.FromHwnd(Handle))
            {
                graphics.FillRectangle(new SolidBrush(Color), ColorSquareDimensions);
                graphics.DrawRectangle(Pens.Black, ColorSquareDimensions);
            }
        }

        private void RefreshMargin()
        {
            if (_comboBoxOwner == null)
                return;

            NativeMethods.SendMessage(Handle, NativeMethods.EM_SETMARGINS, NativeMethods.EC_LEFTMARGIN, ColorSquareDimensions.Width + 4);
        }

        public void SetColor(Color color)
        {
            if (_comboBoxOwner == null)
                return;

            Color = color;
        }
        #endregion
    }

}
