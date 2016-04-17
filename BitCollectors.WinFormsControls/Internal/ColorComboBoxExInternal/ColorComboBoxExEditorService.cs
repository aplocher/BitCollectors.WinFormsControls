using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using BitCollectors.WinFormsControls.Common.Win32;

namespace BitCollectors.WinFormsControls.Internal.ColorComboBoxExInternal
{
    internal class ColorComboBoxExEditorService : IWindowsFormsEditorService, IServiceProvider
    {
        #region Fields
        private DropDownForm _dropDownForm;
        private readonly ColorComboBoxEx _colorComboBoxOwner;
        #endregion

        #region Constructor(s)
        public ColorComboBoxExEditorService(ColorComboBoxEx owner)
        {
            _colorComboBoxOwner = owner;
        }
        #endregion

        #region Properties
        public bool Canceled { get; private set; }
        #endregion

        #region
        public object GetService(Type serviceType)
        {
            return serviceType == typeof(IWindowsFormsEditorService) ? this : null;
        }

        public void CloseDropDown()
        {
            if (_dropDownForm != null)
            {
                _dropDownForm.CloseDropDown();
            }
        }

        public void DropDownControl(Control control)
        {
            Canceled = false;

            using (_dropDownForm = new DropDownForm())
            {
                _dropDownForm.Bounds = control.Bounds;
                _dropDownForm.SetControl(control);

                var pickerForm = GetParentForm(_colorComboBoxOwner);
                var form = pickerForm as Form;
                if (form != null)
                {
                    _dropDownForm.Owner = form;
                }

                PositionDropDownForm();
                _dropDownForm.Show();

                DoModalLoop();

                Canceled = _dropDownForm.Canceled;
            }
            _dropDownForm = null;
        }

        public DialogResult ShowDialog(Form dialog)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region Other methods
        private void DoModalLoop()
        {
            while (_dropDownForm.Visible)
            {
                Application.DoEvents();
                NativeMethods.MsgWaitForMultipleObjects(1, IntPtr.Zero, 1, 5, 255);
            }
        }

        private Control GetParentForm(Control control)
        {
            while (control.Parent != null)
            {
                control = control.Parent;
            }

            return control;
        }

        private void PositionDropDownForm()
        {
            var location = _colorComboBoxOwner.Parent.PointToScreen(_colorComboBoxOwner.Location);
            var screenRect = Screen.PrimaryScreen.WorkingArea;

            if (location.X < screenRect.X)
            {
                location.X = screenRect.X;
            }
            else if ((location.X + _dropDownForm.Width) > screenRect.Right)
            {
                location.X = screenRect.Right - _dropDownForm.Width;
            }

            if ((location.Y + _colorComboBoxOwner.Height + _dropDownForm.Height) > screenRect.Bottom)
            {
                location.Offset(0, -_dropDownForm.Height);
            }
            else
            {
                location.Offset(0, _colorComboBoxOwner.Height);
            }
            _dropDownForm.Location = location;
        }
        #endregion
    }
}
