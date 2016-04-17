using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common;
using BitCollectors.WinFormsControls.Common.Win32;
using Microsoft.Win32;

namespace BitCollectors.WinFormsControls.Internal
{
    internal class ToolStripTextBoxExInternal : TextBoxEx
    {
        private bool mouseIsOver = false;
        private bool isFontSet = true;
        private bool alreadyHooked;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily")]
        public ToolStripTextBoxExInternal()
        {
            // required to make the text box height match the combo.
            this.Font = DefaultFont;
            isFontSet = false;
        }

        internal virtual bool HasMenu
        {
            get
            {
                return false;
            }
        }

        // returns the distance from the client rect to the upper left hand corner of the control
        private NativeMethods.RECT AbsoluteClientRECT
        {
            get
            {
                NativeMethods.RECT rect = new NativeMethods.RECT();
                CreateParams cp = CreateParams;

                NativeMethods.AdjustWindowRectEx(ref rect, cp.Style, HasMenu, cp.ExStyle);

                // the coordinates we get back are negative, we need to translate this back to positive.
                int offsetX = -rect.left; // one to get back to 0,0, another to translate
                int offsetY = -rect.top;

                // fetch the client rect, then apply the offset.
                NativeMethods.GetClientRect(new HandleRef(this, this.Handle), ref rect);

                rect.left += offsetX;
                rect.right += offsetX;
                rect.top += offsetY;
                rect.bottom += offsetY;

                return rect;
            }
        }

        private Rectangle AbsoluteClientRectangle
        {
            get
            {
                NativeMethods.RECT rect = AbsoluteClientRECT;
                return Rectangle.FromLTRB(rect.top, rect.top, rect.right, rect.bottom);
            }
        }


        //private ProfessionalColorTable ColorTable
        //{
        //    get
        //    {
        //        if (Owner != null)
        //        {
        //            ToolStripProfessionalRenderer renderer = Owner.Renderer as ToolStripProfessionalRenderer;
        //            if (renderer != null)
        //            {
        //                return renderer.ColorTable;
        //            }
        //        }
        //        return ProfessionalColors.ColorTable;
        //    }
        //}

        private bool IsPopupTextBox
        {
            get
            {
                return (BorderStyle == BorderStyleEx.Fixed3D) && Owner != null && Owner.Owner != null && GetRenderer() is ToolStripProfessionalRenderer;
                //(Owner != null && (Owner.Renderer is ToolStripProfessionalRenderer)));
            }
        }

        internal bool MouseIsOver
        {
            get { return mouseIsOver; }
            set
            {
                if (mouseIsOver != value)
                {
                    mouseIsOver = value;
                    if (!Focused)
                    {
                        InvalidateNonClient();
                    }
                }
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                isFontSet = ShouldSerializeFont();
            }
        }

        internal bool ShouldSerializeFont()
        {
            return Font != DefaultFont;
        }

        internal new static Font DefaultFont
        {
            get
            {
                Font sysFont = null;
                Font retFont = defaultFont; // threadsafe local reference

                if (retFont == null)
                {
                    lock (internalSyncObject)
                    {
                        // double check the defaultFont after the lock.
                        retFont = defaultFont;

                        if (retFont == null)
                        {
                            // default to menu font
                            sysFont = SystemFonts.MenuFont;

                            // ensure font is in pixels so it displays properly in the property grid at design time.
                            if (sysFont.Unit != GraphicsUnit.Point)
                            {
                                defaultFont = FontInPoints(sysFont);
                                retFont = defaultFont;
                                sysFont.Dispose();
                            }
                            else
                            {
                                defaultFont = sysFont;
                                retFont = defaultFont;
                            }
                            return retFont;
                        }
                    }
                }

                return retFont;
            }
        }

        internal static Font FontInPoints(Font font)
        {
            return new Font(font.FontFamily, font.SizeInPoints, font.Style, GraphicsUnit.Point, font.GdiCharSet, font.GdiVerticalFont);
        }

        private static Font defaultFont;
        private static readonly object internalSyncObject = new object();


        public ToolStripTextBoxEx Owner { get; set; }

        private void InvalidateNonClient()
        {
            if (!IsPopupTextBox)
            {
                return;
            }
            NativeMethods.RECT absoluteClientRectangle = AbsoluteClientRECT;
            HandleRef hNonClientRegion = NativeMethods.NullHandleRef;
            HandleRef hClientRegion = NativeMethods.NullHandleRef;
            HandleRef hTotalRegion = NativeMethods.NullHandleRef;

            try
            {
                // get the total client area, then exclude the client by using XOR
                hTotalRegion = new HandleRef(this, NativeMethods.CreateRectRgn(0, 0, this.Width, this.Height));
                hClientRegion = new HandleRef(this,
                    NativeMethods.CreateRectRgn(absoluteClientRectangle.left, absoluteClientRectangle.top,
                        absoluteClientRectangle.right, absoluteClientRectangle.bottom));
                hNonClientRegion = new HandleRef(this, NativeMethods.CreateRectRgn(0, 0, 0, 0));

                NativeMethods.CombineRgn(hNonClientRegion, hTotalRegion, hClientRegion, NativeMethods.RGN_XOR);

                // Call RedrawWindow with the region.
                NativeMethods.RECT ignored = new NativeMethods.RECT();
                NativeMethods.RedrawWindow(new HandleRef(this, Handle),
                    ref ignored, hNonClientRegion,
                    NativeMethods.RDW_INVALIDATE | NativeMethods.RDW_ERASE |
                    NativeMethods.RDW_UPDATENOW | NativeMethods.RDW_ERASENOW |
                    NativeMethods.RDW_FRAME);
            }
            finally
            {
                // clean up our regions.
                try
                {
                    if (hNonClientRegion.Handle != IntPtr.Zero)
                    {
                        NativeMethods.DeleteObject(hNonClientRegion);
                    }
                }
                finally
                {
                    try
                    {
                        if (hClientRegion.Handle != IntPtr.Zero)
                        {
                            NativeMethods.DeleteObject(hClientRegion);
                        }
                    }
                    finally
                    {
                        if (hTotalRegion.Handle != IntPtr.Zero)
                        {
                            NativeMethods.DeleteObject(hTotalRegion);
                        }
                    }
                }

            }

        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            InvalidateNonClient();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            InvalidateNonClient();

        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseIsOver = true;

        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseIsOver = false;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Disposing && !IsDisposed)
            {
                HookStaticEvents(Visible);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                HookStaticEvents(false);
            }
            base.Dispose(disposing);
        }

        private void WmNCPaint(ref Message m)
        {
            if (!IsPopupTextBox)
            {
                base.WndProc(ref m);
                return;
            }

            // Paint over the edges of the text box.

            // Using GetWindowDC instead of GetDCEx as GetDCEx seems to return a null handle and a last error of 
            // the operation succeeded.  We're not going to use the clipping rect anyways - so it's not
            // that bigga deal.
            HandleRef hdc = new HandleRef(this, NativeMethods.GetWindowDC(new HandleRef(this, m.HWnd)));
            if (hdc.Handle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            try
            {
                // Dont set the clipping region based on the WParam - windows seems to hack out the two pixels
                // intended for the non-client border.

                Color outerBorderColor = (MouseIsOver || Focused)
                    ? ColorTable.ButtonSelectedHighlightBorder
                    : this.BackColor;
                Color innerBorderColor = this.BackColor;

                if (!Enabled)
                {
                    outerBorderColor = SystemColors.ControlDark;
                    innerBorderColor = SystemColors.Control;
                }
                using (Graphics g = Graphics.FromHdcInternal(hdc.Handle))
                {

                    Rectangle clientRect = AbsoluteClientRectangle;

                    // could have set up a clip and fill-rectangled, thought this would be faster.
                    using (Brush b = new SolidBrush(innerBorderColor))
                    {
                        g.FillRectangle(b, 0, 0, this.Width, clientRect.Top); // top border
                        g.FillRectangle(b, 0, 0, clientRect.Left, this.Height); // left border
                        g.FillRectangle(b, 0, clientRect.Bottom, this.Width, this.Height - clientRect.Height);
                        // bottom border
                        g.FillRectangle(b, clientRect.Right, 0, this.Width - clientRect.Right, this.Height);
                        // right border
                    }

                    // paint the outside rect.
                    using (Pen p = new Pen(outerBorderColor))
                    {
                        g.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
            finally
            {
                NativeMethods.ReleaseDC(new HandleRef(this, this.Handle), hdc);
            }

            // we've handled WM_NCPAINT.
            m.Result = IntPtr.Zero;
        }

        private ProfessionalColorTable ColorTable
        {
            get
            {
                if (Owner != null)
                {
                    ToolStripProfessionalRenderer renderer = GetRenderer() as ToolStripProfessionalRenderer;
                    if (renderer != null)
                    {
                        return renderer.ColorTable;
                    }
                }
                return new ProfessionalColorTable();
            }
        }

        private ToolStripRenderer GetRenderer()
        {
            if (Owner != null && Owner.Owner != null)
            {
                return Owner.Owner.Renderer;
            }

            return null;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_NCPAINT)
            {
                WmNCPaint(ref m);
                return;
            }

            base.WndProc(ref m);
        }

        private void HookStaticEvents(bool hook)
        {
            if (hook)
            {
                if (!alreadyHooked)
                {
                    try
                    {
                        SystemEvents.UserPreferenceChanged +=
                            new UserPreferenceChangedEventHandler(OnUserPreferenceChanged);
                    }
                    finally
                    {
                        alreadyHooked = true;
                    }
                }
            }
            else if (alreadyHooked)
            {
                try
                {
                    SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(OnUserPreferenceChanged);
                }
                finally
                {
                    alreadyHooked = false;
                }
            }

        }


        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Window)
            {
                if (!isFontSet)
                {
                    this.Font = DefaultFont;
                }
            }
        }
    }
}

