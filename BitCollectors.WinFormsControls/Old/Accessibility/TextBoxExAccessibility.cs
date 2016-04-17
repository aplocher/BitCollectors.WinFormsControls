using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls.Accessibility
{
    //
    // http://msdn.microsoft.com/en-us/library/hh552522.aspx
    //


    public class TextBoxExAccessibility : Control.ControlAccessibleObject
    {
        private TextBoxEx _control;
        public TextBoxExAccessibility(Control ownerControl) : base(ownerControl) { _control = ownerControl as TextBoxEx; }

        public TextBoxExAccessibility(TextBoxEx ownerControl) : base(ownerControl) { _control = ownerControl; }
    }
}
