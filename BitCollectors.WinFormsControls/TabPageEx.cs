using System.ComponentModel;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls
{
    public class TabPageEx: TabPage
    {
        
        public TabPageEx()
        {
            base.UseVisualStyleBackColor = true;
        }

        [DefaultValue(true)]
        public new bool UseVisualStyleBackColor
        {
            get { return base.UseVisualStyleBackColor; }
            set { base.UseVisualStyleBackColor = value; }
        }
    }
}
