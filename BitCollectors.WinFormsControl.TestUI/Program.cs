﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControl.TestUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
