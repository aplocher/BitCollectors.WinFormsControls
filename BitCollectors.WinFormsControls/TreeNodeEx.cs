using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Internal;

namespace BitCollectors.WinFormsControls
{
    [Serializable]
    public class TreeNodeEx : TreeNode
    {
        private TreeNodeCollectionEx _collection;

        public TreeNodeEx() { }
        public TreeNodeEx(string text) : base(text) { }
        public TreeNodeEx(string text, TreeNode[] children) : base(text, children) { }
        public TreeNodeEx(string text, int imageIndex, int selectedImageIndex) : base(text, imageIndex, selectedImageIndex) { }
        public TreeNodeEx(string text, int imageIndex, int selectedImageIndex, TreeNode[] children) : base(text, imageIndex, selectedImageIndex, children) { }
        protected TreeNodeEx(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context) { }

        public new TreeNodeCollectionEx Nodes
        {
            get
            {
                if (_collection == null)
                    _collection = new TreeNodeCollectionEx(base.Nodes);

                return _collection;
            }
        }

        public new TreeNodeEx Parent => (TreeNodeEx)base.Parent;

        public bool IsFilteredOut => this.FilterType != FilterTypes.DirectMatch && !this.TreeView.IsFiltered;

        public override object Clone()
        {
            var node = (TreeNodeEx)base.Clone();
            node.Cloned = true;
            return node;
        }

        public new TreeViewEx TreeView => (TreeViewEx)base.TreeView;

        private bool Cloned { get; set; }

        internal bool IsFiltering { get; set; }

        internal FilterTypes FilterType { get; set; }

        internal int MatchStartIndex { get; set; }

        internal int MatchEndIndex { get; set; }

        public new TreeNodeEx NextNode => base.NextNode as TreeNodeEx;

        public new TreeNodeEx PrevNode => base.PrevNode as TreeNodeEx;

        public new TreeNodeEx PrevVisibleNode => base.PrevVisibleNode as TreeNodeEx;

        public new TreeNodeEx NextVisibleNode => base.NextVisibleNode as TreeNodeEx;


        public TreeNodeEx NextMatchedNode 
        {
            get
            {
                if (IsFiltering)
                {
                   
                }

                return this.NextNode;
            }
        }
    }

}
