using System;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls.Internal.ColorComboBoxExInternal
{
    internal class DropDownForm : Form
    {
        #region Fields
        private bool _closeDropDownCalled;
        #endregion

        #region Constructor(s)
        public DropDownForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            KeyPreview = true;

            StartPosition = FormStartPosition.Manual;

            var panel = new Panel { BorderStyle = BorderStyle.FixedSingle, Dock = DockStyle.Fill };
            Controls.Add(panel);
        }
        #endregion

        #region Properties
        public bool Canceled { get; private set; }
        #endregion

        #region Overrides
        protected override void OnDeactivate(EventArgs e)
        {
            Owner = null;

            base.OnDeactivate(e);

            if (!_closeDropDownCalled)
            {
                Canceled = true;
            }

            Hide();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if ((e.Modifiers == 0) && (e.KeyCode == Keys.Escape))
            {
                Hide();
            }
        }
        #endregion

        #region Other methods
        public void CloseDropDown()
        {
            _closeDropDownCalled = true;
            Hide();
        }

        public void SetControl(Control control)
        {
            ((Panel)Controls[0]).Controls.Add(control);
        }
        #endregion
    }
}
