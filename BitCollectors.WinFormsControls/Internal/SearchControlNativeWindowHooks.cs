using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common;
using BitCollectors.WinFormsControls.Common.Win32;

namespace BitCollectors.WinFormsControls.Internal
{
    internal class SearchControlNativeWindowHooks : NativeWindow
    {
        private bool _suppressKillFocusCheckOnce;

        internal TreeViewEx TreeViewControl { get; set; }

        private bool _suppressSetFocusCheckOnce;

        protected override void WndProc(ref Message m)
        {
            if (TreeViewControl == null)
            {
                base.WndProc(ref m);
                return;
            }

            switch (m.Msg)
            {
                case NativeMethods.WM_KILLFOCUS:
                    if (_suppressKillFocusCheckOnce)
                    {
                        _suppressKillFocusCheckOnce = false;
                    }
                    else if (!TreeViewControl.Focused && !TreeViewControl.AttachedSearchControl.Focused &&
                        m.WParam.ToString() != TreeViewControl.Handle.ToString() &&
                        m.WParam.ToString() != TreeViewControl.AttachedSearchControl.Handle.ToString())
                    {
                        _suppressKillFocusCheckOnce = true;
                        NativeMethods.PostMessage(TreeViewControl.Handle, m.Msg, m.WParam, m.LParam);
                    }
                    else
                        return;

                    break;

                case NativeMethods.WM_SETFOCUS:
                    if (_suppressSetFocusCheckOnce)
                    {
                        _suppressSetFocusCheckOnce = false;
                    }
                    else if (TreeViewControl.Focused || TreeViewControl.AttachedSearchControl.Focused ||
                        m.WParam.ToString() == TreeViewControl.Handle.ToString() ||
                        m.WParam.ToString() == TreeViewControl.AttachedSearchControl.Handle.ToString())
                    {
                        _suppressSetFocusCheckOnce = true;
                        NativeMethods.PostMessage(TreeViewControl.Handle, NativeMethods.WM_SETFOCUS, m.WParam, m.LParam);
                    }

                    break;

                case NativeMethods.WM_NCDESTROY:
                    this.ReleaseHandle();
                    break;
            }
            //if (m.Msg == Win32Native.WM_KILLFOCUS && TreeViewControl != null)
            //{
            //    if (_suppressKillFocusCheckOnce)
            //    {
            //        _suppressKillFocusCheckOnce = false;
            //    }
            //    else if (!TreeViewControl.Focused && !TreeViewControl.AttachedSearchControl.Focused &&
            //        m.WParam.ToString() != TreeViewControl.Handle.ToString() &&
            //        m.WParam.ToString() != TreeViewControl.AttachedSearchControl.Handle.ToString())
            //    {
            //        _suppressKillFocusCheckOnce = true;
            //        Win32Native.PostMessage(TreeViewControl.Handle, m.Msg, m.WParam, m.LParam);
            //    }
            //    else
            //        return;
            //}
            //else
            //{
            //    if (m.Msg == Win32Native.WM_NCDESTROY)
            //        this.ReleaseHandle();
            //}

            base.WndProc(ref m);

            
        }

    }
}
