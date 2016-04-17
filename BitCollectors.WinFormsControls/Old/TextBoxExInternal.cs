using System.Windows.Forms;

namespace BitCollectors.WinFormsControls.Internal
{
    internal class EnhancedTextBoxInternal : TextBox
    {
        public bool EscapeClearsInput { get; set; }

        internal TextBoxEx ParentContainer { get; private set; }

        internal EnhancedTextBoxInternal(TextBoxEx parent)
        {
            ParentContainer = parent;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && this.Focused && EscapeClearsInput)
            {
                this.Text = string.Empty;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
