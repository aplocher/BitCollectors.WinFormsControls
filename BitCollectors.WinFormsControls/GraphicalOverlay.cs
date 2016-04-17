using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls
{
    // Blatanly stolen from: http://www.codeproject.com/Articles/26071/Draw-Over-WinForms-Controls
    // For the following reason: [add url to backup reason for overlay on textbox]
    public class GraphicalOverlay : Component
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            components = new Container();
        }
        #endregion

        public event EventHandler<PaintEventArgs> Paint;
        private Control _control;

        public GraphicalOverlay()
        {
            InitializeComponent();
        }

        public GraphicalOverlay(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control Owner
        {
            get { return _control; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if (_control != null)
                    throw new InvalidOperationException();

                _control = value;
                _control.Resize += new EventHandler(Form_Resize);

                ConnectPaintEventHandlers(_control);
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            _control.Invalidate(true);
        }

        private void ConnectPaintEventHandlers(Control control)
        {
            control.Paint -= new PaintEventHandler(Control_Paint);
            control.Paint += new PaintEventHandler(Control_Paint);

            control.ControlAdded -= new ControlEventHandler(Control_ControlAdded);
            control.ControlAdded += new ControlEventHandler(Control_ControlAdded);

            foreach (Control child in control.Controls)
            {
                ConnectPaintEventHandlers(child);
            }
        }

        private void Control_ControlAdded(object sender, ControlEventArgs e)
        {
            ConnectPaintEventHandlers(e.Control);
        }

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            Control control = sender as Control;
            Point location;

            if (control == _control)
            {
                location = control.Location;
            }
            else
            {
                location = _control.PointToClient(control.Parent.PointToScreen(control.Location));
                location += new Size((control.Width - control.ClientSize.Width)/2,
                    (control.Height - control.ClientSize.Height)/2);
            }

            if (control != _control)
                e.Graphics.TranslateTransform(-location.X, -location.Y);

            OnPaint(sender, e);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (Paint != null)
                Paint(sender, e);
        }
    }
}
