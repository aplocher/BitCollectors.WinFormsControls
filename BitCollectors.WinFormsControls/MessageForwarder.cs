using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls
{
    public class MessageForwarder : NativeWindow, IMessageFilter
    {
        #region Fields
        private Control _control;
        private Control _previousParent;
        private HashSet<int> _messages;
        private bool _isMouseOverControl;
        #endregion

        #region Constructors
        public MessageForwarder(Control control, int message) : this(control, new int[] { message }) { }

        public MessageForwarder(Control control, IEnumerable<int> messages)
        {
            _control = control;
            AssignHandle(control.Handle);
            _messages = new HashSet<int>(messages);
            _previousParent = control.Parent;
            _isMouseOverControl = false;

            control.ParentChanged += control_ParentChanged;
            control.MouseEnter += control_MouseEnter;
            control.MouseLeave += control_MouseLeave;
            control.Leave += control_Leave;
            
            if (control.Parent != null)
                Application.AddMessageFilter(this);
        }

        #endregion // Constructors

        #region IMessageFilter members

        public bool PreFilterMessage(ref Message m)
        {
            if (_messages.Contains(m.Msg) && _control.CanFocus && !_control.Focused && _isMouseOverControl)
            {
                m.HWnd = _control.Handle;
                WndProc(ref m);
                return true;
            }

            return false;
        }

        #endregion // IMessageFilter

        #region Event handlers

        void control_ParentChanged(object sender, EventArgs e)
        {
            if (_control.Parent == null)
                Application.RemoveMessageFilter(this);
            else
            {
                if (_previousParent == null)
                    Application.AddMessageFilter(this);
            }
            _previousParent = _control.Parent;
        }

        void control_MouseEnter(object sender, EventArgs e)
        {
            _isMouseOverControl = true;
        }

        void control_MouseLeave(object sender, EventArgs e)
        {
            _isMouseOverControl = false;
        }

        void control_Leave(object sender, EventArgs e)
        {
            _isMouseOverControl = false;
        }

        #endregion // Event handlers
    }
}
