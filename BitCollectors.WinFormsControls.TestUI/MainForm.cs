using System;
using System.Drawing;
using System.Windows.Forms;
using BitCollectors.WinFormsControls;
using BitCollectors.WinFormsControls.Common;
using BitCollectors.WinFormsControls.Common.Win32;
using Timer = System.Windows.Forms.Timer;

namespace BitCollectors.WinFormsControl.TestUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            string[] names = new string[] { "Adam", "Ale", "Chris", "Ryan", "Matt", "Retchie", "Mikey", "First Last", "2pac Shakur" };

            InitializeComponent();
            var r = new Random();
            for (int i = 0; i < 3; i++)
            {
                var node = new TreeNodeEx("Test" + i);
                for (int j = 0; j < 20; j++)
                {
                    var subNode = new TreeNodeEx("SubTest" + j);

                    for (int k = 0; k < 10; k++)
                    {
                        subNode.Nodes.Add(new TreeNodeEx(names[r.Next(0, names.Length - 1)] + r.Next(0, 100).ToString()));
                    }
                    node.Nodes.Add(subNode);
                }
                treeViewEx1.Nodes.Add(node);
            }

            //DoStuff();
            var t = new Timer { Interval = 700 };
            t.Tick += (sender, args) =>
            {
                if (buttonPressed)
                {
                    textBox1.Text = string.Format("Text: {0}{1}WM: {2}{1}PRESSED", textBoxEx1.Text, Environment.NewLine,
                        textBoxEx1.WatermarkText);
                    buttonPressed = false;
                }
                else
                {

                    textBox1.Text = string.Format("Text: {0}{1}WM: {2}", textBoxEx1.Text, Environment.NewLine,
                        textBoxEx1.WatermarkText);
                }
            };
            t.Start();

            //if (textBoxTest1 is TextBox)
            //{

            //}

            //if (textBoxTest21 is TextBox)
            //{

            //}
        }

        private void textBoxTest21_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void textBoxTest2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonPressed = true;

        }

        bool buttonPressed = false;

    }

    public class TextBoxTestUc : UserControl
    {
        private TextBox _textBox;

        public override string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }

        public TextBoxTestUc()
        {
            _textBox = new TextBox
            {
                WordWrap = false,
                AutoSize = false,
                Dock = DockStyle.Fill
            };

            this.Controls.Add(_textBox);
        }
    }

    public class TextBoxTestTb : TextBox
    {
        private TextBoxBorderPainter _painter;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Rectangle rect = new Rectangle();
            NativeMethods.SendMessage(Handle, NativeMethods.EM_GETRECT, (IntPtr)0, ref rect);

            Console.WriteLine(rect);

            rect.Y = 50;
            NativeMethods.SendMessage(Handle, NativeMethods.EM_SETRECT, (IntPtr)0, ref rect);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.Handled = true;
                    return;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.Handled = true;
                    return;
            }

            base.OnKeyUp(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        { // http://www.xtremedotnettalk.com/showthread.php?t=75210
            switch (e.KeyChar)
            {
                case '\r':
                case '\n':
                    e.Handled = true;
                    return;
            }

            base.OnKeyPress(e);
        }

        protected override bool ProcessKeyMessage(ref Message m)
        {
            return base.ProcessKeyMessage(ref m);
        }

        public TextBoxTestTb()
        {
            base.AutoSize = false;
            base.Height = 55;
            base.Width = 150;
            base.Text = "This is a test";
            base.Padding = new Padding(10);
            base.Margin = new Padding(10);
            base.BorderStyle = BorderStyle.None;
            base.Multiline = true;

            //?
            base.AcceptsReturn = false;

            _painter = new TextBoxBorderPainter(this);
            _painter.Border = new Border { Color = Color.Crimson, Size = 3 };

            this.Height = 100;

            //var rect = new RECT(); // = RECT.FromXYWH(20, 40, 100, 50);
            //NativeMethods.SendMessage(Handle, NativeMethods.EM_GETRECT, (IntPtr)0, rect);

            Rectangle rect = new Rectangle();
            NativeMethods.SendMessage(Handle, NativeMethods.EM_GETRECT, (IntPtr)0, ref rect);

            Console.WriteLine(rect);

            rect.Y = 50;
            NativeMethods.SendMessage(Handle, NativeMethods.EM_SETRECT, (IntPtr)0, ref rect);

            //NativeMethods.SendMessage()
            //NativeMethods.SendMessage(Handle, NativeMethods.EM_SETMARGINS, NativeMethods.EC_LEFTMARGIN, 20);
            //NativeMethods.SendMessage(Handle, NativeMethods.EM_SETRECT, 0, ref rect);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_painter != null)
                _painter.Dispose();
        }

        private class TextBoxBorderPainter : NativeWindow, IDisposable
        {
            #region Fields
            private Pen _borderPen;
            private Border _border;
            #endregion

            #region Constructor(s)
            public TextBoxBorderPainter(TextBoxTestTb textBoxEx)
            {
                AssignHandle(textBoxEx.Handle);
                TextBoxEx = textBoxEx;
            }
            #endregion

            #region Properties
            public Border Border
            {
                get { return _border; }
                set
                {
                    if (_border != value)
                    {
                        _border = value;
                        _borderPen = new Pen(_border.Color, _border.Size * 2);
                        TextBoxEx.Invalidate();
                    }
                }
            }

            public TextBoxTestTb TextBoxEx { get; private set; }
            #endregion

            #region
            public void Dispose()
            {
                if (_borderPen != null)
                {
                    _borderPen.Dispose();
                    _borderPen = null;
                }
            }
            #endregion

            #region Overrides
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                switch (m.Msg)
                {
                    case NativeMethods.WM_PAINT:
                        CustomPaint();
                        break;
                }
            }
            #endregion

            #region Other methods
            private void CustomPaint()
            {
                if (_borderPen == null)
                    return;

                var textBoxGraphics = Graphics.FromHwnd(TextBoxEx.Parent.Handle);
                textBoxGraphics.DrawRectangle(_borderPen, new Rectangle(TextBoxEx.Location, TextBoxEx.Size));

                //var textBoxGraphics2 = Graphics.FromHwnd(TextBoxEx.Handle);
                //textBoxGraphics2.DrawRectangle(_borderPen, new Rectangle(TextBoxEx.Location, TextBoxEx.Size));
            }
            #endregion
        }
    }
}

