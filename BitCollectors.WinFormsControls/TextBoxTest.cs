using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common.Win32;

namespace BitCollectors.WinFormsControls
{
    public class TextBoxTest : TextBox
    {
        public TextBoxTest()
        {
            base.AutoSize = false;
            NativeMethods.SendMessage(Handle, NativeMethods.EM_SETMARGINS, NativeMethods.EC_LEFTMARGIN, 20);

            base.Height = 55;
            base.Width = 150;
            base.Text = "This is a test";
            base.Padding = new Padding(10);
            base.Margin = new Padding(10);
        }
    }
}
