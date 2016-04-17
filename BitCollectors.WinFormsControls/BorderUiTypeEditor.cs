using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using BitCollectors.WinFormsControls.Common;

namespace BitCollectors.WinFormsControls
{
    public class BorderUiTypeEditor : UITypeEditor
    {
        #region Properties
        public override bool IsDropDownResizable
        {
            get { return base.IsDropDownResizable; }
        }
        #endregion

        #region Overrides
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            var borderValue = value as Border;

            if (service == null)
                return value;

            var editor = new BorderEditControl(borderValue);
            service.DropDownControl(editor);

            return editor.BorderValue;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value == null)
                return;

            var border = (Border)e.Value;
            using (var brush = new SolidBrush(border.Color))
            {
                //var rectangle = new Rectangle(1, 1, e.Bounds.Height - 2, 20);
                e.Graphics.FillRectangle(brush, e.Bounds);
                //e.Graphics.DrawRectangle(Pens.Black, rectangle);
                //e.Graphics.DrawString(border.Size +" - "+ border.Color.ToString(), new Font("Arial", 10.0f), Brushes.Black, 24, 2);
            }
            
            base.PaintValue(e);
        }
        #endregion
    }
}