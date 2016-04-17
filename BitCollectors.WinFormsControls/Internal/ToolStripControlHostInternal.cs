using System.Windows.Forms;

namespace BitCollectors.WinFormsControls.Internal
{
    public class ToolStripControlHostInternal : ToolStripControlHost
    {
        internal ToolStripControlHostInternal()
            : base(new Control())
        {
        }
        public ToolStripControlHostInternal(Control c)
            : base(c)
        {
        }
    }
}
