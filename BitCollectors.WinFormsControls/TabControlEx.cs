using System.ComponentModel;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls
{
    public enum TabReorderingMode
    {
        DragAndSlidePositionAnimated,
        DragAndSlidePosition,
        DragAndShowNewPositionWithArrow
    }

    public class TabControlEx : TabControl
    {
        private bool _enableTabReordering = false;

        /// <summary>
        /// Set to true to allow the user to drag and drop tab headers in order to reposition them
        /// </summary>
        [DefaultValue(false)]
        public bool AllowTabReordering
        {
            get { return _enableTabReordering; }
            set
            {
                if (_enableTabReordering != value)
                {
                    _enableTabReordering = value;
                    this.AllowDrop = true;
                }
            }
        }

        public int TabReorderingMode { get; set; }

        public override bool AllowDrop
        {
            get { return base.AllowDrop; }
            set { base.AllowDrop = (value && !_enableTabReordering); }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool AllowTabsMovedToNewWindow { get; set; }

        /// <summary>
        /// When true, you can move tabs across multiple TabControlEx controls that have the same TabGroupName
        /// </summary>
        [DefaultValue(false)]
        public bool AllowTabsMovedToOtherWindow { get; set; }

        /// <summary>
        /// Use a TabGroupName to identify TabControlEx's that tabs can be moved to. Only needed when AllowTabMovedToOtherWindow is true
        /// </summary>
        [DefaultValue("")]
        public string TabGroupName { get; set; }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                this.DoDragDrop(this.SelectedTab, DragDropEffects.All);
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            e.Effect = e.Data.GetDataPresent(typeof(TabPage))
                ? DragDropEffects.Move
                : DragDropEffects.None;
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            var dropTab = (TabPage)e.Data.GetData(typeof(TabPage));
            TabPages.Add(dropTab);
        }
    }
}
