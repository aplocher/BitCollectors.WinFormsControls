
namespace BitCollectors.WinFormsControls.Internal
{
    class ToolStripManagerInternal
    {

        //internal static Font DefaultFont
        //{
        //    get
        //    {
        //        Font sysFont = null;
        //        Font retFont = defaultFont;  // threadsafe local reference

        //        if (retFont == null)
        //        {
        //            lock (internalSyncObject)
        //            {
        //                // double check the defaultFont after the lock.
        //                retFont = defaultFont;

        //                if (retFont == null)
        //                {
        //                    // default to menu font
        //                    sysFont = SystemFonts.MenuFont;
        //                    if (sysFont == null)
        //                    {
        //                        // ...or to control font if menu font unavailable
        //                        sysFont = Control.DefaultFont;
        //                    }
        //                    if (sysFont != null)
        //                    {
        //                        // ensure font is in pixels so it displays properly in the property grid at design time.
        //                        if (sysFont.Unit != GraphicsUnit.Point)
        //                        {
        //                            defaultFont = ControlPaint.FontInPoints(sysFont);
        //                            retFont = defaultFont;
        //                            sysFont.Dispose();
        //                        }
        //                        else
        //                        {
        //                            defaultFont = sysFont;
        //                            retFont = defaultFont;
        //                        }
        //                    }
        //                    return retFont;
        //                }
        //            }
        //        }
        //        return retFont;
        //    }
        //}



    }
}
