//using System;
//using System.Collections;
//using System.ComponentModel;
//using System.ComponentModel.Design.Serialization;
//using System.Globalization;
//using System.Reflection;
//using System.Windows.Forms;

//namespace BitCollectors.WinFormsControls
//{
//    public class EnhanctedTreeView
//    {
        
//    }

//    [TypeConverter(typeof(TreeNodeExConverter))]
//    public class TreeNodeExCollection : IList
//    {
//        private TreeNode _owner;
//        private TreeNodeCollection _collection;

//        internal TreeNodeExCollection(TreeNode owner)
//        {
//            _owner = owner;
//        }

//        internal TreeNodeCollection Collection
//        {
//            set { _collection = value; }
//        }

//        internal int FixedIndex
//        {
//            get
//            {
//                return (int)_collection.GetType().GetProperty("FixedIndex", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_collection, null);
//            }
//            set
//            {
//                _collection.GetType().GetProperty("FixedIndex", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_collection, value, null);
//            }
//        }

//        public virtual TreeNode this[int index]
//        {
//            get
//            {
//                return _collection[index];
//            }
//            set
//            {
//                _collection[index] = value;
//            }
//        }

//        object IList.this[int index]
//        {
//            get
//            {
//                return this[index];
//            }
//            set
//            {
//                if (value is TreeNode)
//                {
//                    this[index] = (TreeNode)value;
//                }
//                else
//                {
//                    //throw new ArgumentException(SR.GetString(SR.TreeNodeCollectionBadTreeNode), "value");
//                }
//            }
//        }

//        public virtual TreeNode this[string key]
//        {
//            get
//            {
//                return _collection[key];
//            }
//        }
//        [Browsable(false)]
//        public int Count
//        {
//            get
//            {
//                return _collection.Count;
//            }
//        }

//        object ICollection.SyncRoot
//        {
//            get
//            {
//                return _collection;
//            }
//        }

//        bool ICollection.IsSynchronized
//        {
//            get
//            {
//                return false;
//            }
//        }

//        bool IList.IsFixedSize
//        {
//            get
//            {
//                return false;
//            }
//        }

//        public bool IsReadOnly
//        {
//            get
//            {
//                return false;
//            }
//        }

//        public virtual TreeNode Add(string text)
//        {
//            return _collection.Add(text);
//        }

//        public virtual TreeNode Add(string key, string text)
//        {
//            return _collection.Add(key, text);
//        }

//        public virtual TreeNode Add(string key, string text, int imageIndex)
//        {
//            return _collection.Add(key, text, imageIndex);
//        }

//        public virtual TreeNode Add(string key, string text, string imageKey)
//        {
//            return _collection.Add(key, text, imageKey);
//        }

//        public virtual TreeNode Add(string key, string text, int imageIndex, int selectedImageIndex)
//        {
//            return _collection.Add(key, text, imageIndex, selectedImageIndex);
//        }

//        public virtual TreeNode Add(string key, string text, string imageKey, string selectedImageKey)
//        {
//            return _collection.Add(key, text, imageKey, selectedImageKey);
//        }

//        public virtual void AddRange(TreeNode[] nodes)
//        {
//            _collection.AddRange(nodes);
//        }

//        public TreeNode[] Find(string key, bool searchAllChildren)
//        {
//            return _collection.Find(key, searchAllChildren);
//        }

//        private ArrayList FindInternal(string key, bool searchAllChildren, TreeNodeExCollection treeNodeCollectionToLookIn, ArrayList foundTreeNodes)
//        {
//            MethodInfo mi = _collection.GetType().BaseType.GetMethod("FindInternal", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//            if (mi != null)
//            {
//                return (ArrayList)mi.Invoke(_collection, new object[] { key, searchAllChildren, treeNodeCollectionToLookIn, foundTreeNodes });
//            }
//            return null;
//        }

//        private ArrayList FindInternal(string key, bool searchAllChildren, TreeNodeCollection treeNodeCollectionToLookIn, ArrayList foundTreeNodes)
//        {
//            MethodInfo mi = _collection.GetType().BaseType.GetMethod("FindInternal", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//            if (mi != null)
//            {
//                return (ArrayList)mi.Invoke(_collection, new object[] { key, searchAllChildren, treeNodeCollectionToLookIn, foundTreeNodes });
//            }
//            return null;
//        }

//        public virtual int Add(TreeNode node)
//        {
//            return _collection.Add(node);
//        }

//        private int AddInternal(TreeNode node, int delta)
//        {
//            MethodInfo mi = _collection.GetType().BaseType.GetMethod("AddInternal", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//            if (mi != null)
//            {
//                return (int)mi.Invoke(_collection, new object[] { node, delta });
//            }
//            return 0;
//        }

//        int IList.Add(object node)
//        {
//            if (node == null)
//            {
//                throw new ArgumentNullException("node");
//            }
//            else if (node is TreeNode)
//            {
//                return _collection.Add((TreeNode)node);
//            }
//            else
//            {
//                TreeNode tempNode = Add(node.ToString());
//                return _collection.Add(tempNode);
//            }
//        }

//        public bool Contains(TreeNode node)
//        {
//            return _collection.Contains(node);
//        }

//        public virtual bool ContainsKey(string key)
//        {
//            return _collection.ContainsKey(key);
//        }

//        bool IList.Contains(object node)
//        {
//            if (node is TreeNode)
//            {
//                return _collection.Contains((TreeNode)node);
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public int IndexOf(TreeNode node)
//        {
//            for (int index = 0; index < Count; ++index)
//            {
//                if (this[index] == node)
//                {
//                    return index;
//                }
//            }
//            return -1;
//        }

//        int IList.IndexOf(object node)
//        {
//            if (node is TreeNode)
//            {
//                return _collection.IndexOf((TreeNode)node);
//            }
//            else
//            {
//                return -1;
//            }
//        }

//        public virtual int IndexOfKey(String key)
//        {
//            return _collection.IndexOfKey(key);
//        }

//        public virtual void Insert(int index, TreeNode node)
//        {
//            _collection.Insert(index, node);
//        }

//        void IList.Insert(int index, object node)
//        {
//            if (node is TreeNode)
//            {
//                _collection.Insert(index, (TreeNode)node);
//            }
//            else
//            {
//                throw new ArgumentException(/*SR.GetString(SR.TreeNodeCollectionBadTreeNode)*/"Bad TreeNode", "node");
//            }
//        }

//        public virtual TreeNode Insert(int index, string text)
//        {
//            return _collection.Insert(index, text);
//        }

//        public virtual TreeNode Insert(int index, string key, string text)
//        {
//            return _collection.Insert(index, key, text);
//        }

//        public virtual TreeNode Insert(int index, string key, string text, int imageIndex)
//        {
//            return _collection.Insert(index, key, text, imageIndex);
//        }

//        public virtual TreeNode Insert(int index, string key, string text, string imageKey)
//        {
//            return _collection.Insert(index, key, text, imageKey);
//        }

//        public virtual TreeNode Insert(int index, string key, string text, int imageIndex, int selectedImageIndex)
//        {
//            return _collection.Insert(index, key, text, imageIndex, selectedImageIndex);
//        }

//        public virtual TreeNode Insert(int index, string key, string text, string imageKey, string selectedImageKey)
//        {
//            return _collection.Insert(index, key, text, imageKey, selectedImageKey);
//        }

//        private bool IsValidIndex(int index)
//        {
//            return ((index >= 0) && (index < this.Count));
//        }

//        public virtual void Clear()
//        {
//            _collection.Clear();
//        }

//        public void CopyTo(Array dest, int index)
//        {
//            _collection.CopyTo(dest, index);
//        }

//        public void Remove(TreeNode node)
//        {
//            _collection.Remove(node);
//        }

//        void IList.Remove(object node)
//        {
//            if (node is TreeNode)
//            {
//                _collection.Remove((TreeNode)node);
//            }
//        }

//        public virtual void RemoveAt(int index)
//        {
//            this[index].Remove();
//        }

//        public virtual void RemoveByKey(string key)
//        {
//            _collection.RemoveByKey(key);
//        }

//        public IEnumerator GetEnumerator()
//        {
//            return _collection.GetEnumerator();
//        }
//    }

//    public class TreeNodeExConverter : TypeConverter
//    {
//        public override bool CanConvertFrom(ITypeDescriptorContext context, Type type)
//        {
//            if (type == typeof(string))
//            {
//                return true;
//            }

//            return base.CanConvertFrom(context, type);
//        }

//        public override bool CanConvertTo(ITypeDescriptorContext context, Type type)
//        {
//            if (type == typeof(InstanceDescriptor) || type == typeof(string))
//            {
//                return true;
//            }

//            return base.CanConvertTo(context, type);
//        }

//        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
//        {
//            if (value != null && value is string)
//            {
//                //string[] items = ((string)value).Split(',');
//                return new TreeNodeEx(); //items[0], items[1]);
//            }

//            return base.ConvertFrom(context, info, value);
//        }

//        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo info, object value, Type type)
//        {
//            if (type == null)
//            {
//                throw new ArgumentNullException("type");
//            }
//            if ((type == typeof(InstanceDescriptor)) && (value is TreeNodeEx))
//            {
//                TreeNodeEx node = (TreeNodeEx)value;
//                MemberInfo member = null;
//                object[] arguments = null;
//                if ((node.ImageIndex == -1) || (node.SelectedImageIndex == -1))
//                {
//                    if (node.Nodes.Count == 0)
//                    {
//                        member = typeof(TreeNodeEx).GetConstructor(new Type[] { typeof(string) });
//                        arguments = new object[] { node.Text };
//                    }
//                    else
//                    {
//                        member = typeof(TreeNodeEx).GetConstructor(new Type[] { typeof(string), typeof(TreeNodeEx[]) });
//                        TreeNodeEx[] dest = new TreeNodeEx[node.Nodes.Count];
//                        node.Nodes.CopyTo(dest, 0);
//                        arguments = new object[] { node.Text, dest };
//                    }
//                }
//                else if (node.Nodes.Count == 0)
//                {
//                    member = typeof(TreeNodeEx).GetConstructor(new Type[] { typeof(string), typeof(int), typeof(int) });
//                    arguments = new object[] { node.Text, node.ImageIndex, node.SelectedImageIndex };
//                }
//                else
//                {
//                    member = typeof(TreeNodeEx).GetConstructor(new Type[] { typeof(string), typeof(int), typeof(int), typeof(TreeNodeEx[]) });
//                    TreeNodeEx[] nodeArray2 = new TreeNodeEx[node.Nodes.Count];
//                    node.Nodes.CopyTo(nodeArray2, 0);
//                    arguments = new object[] { node.Text, node.ImageIndex, node.SelectedImageIndex, nodeArray2 };
//                }
//                if (member != null)
//                {
//                    return new InstanceDescriptor(member, arguments, false);
//                }
//            }
//            else if ((type == typeof(InstanceDescriptor)) && (value is TreeNodeExCollection))
//            {
//                Type valueType = value.GetType();
//                ConstructorInfo ci = valueType.GetConstructor(System.Type.EmptyTypes);
//                return new InstanceDescriptor(ci, null, false);
//            }
//            else
//            {
//                Type valueType = value.GetType();
//                ConstructorInfo ci = valueType.GetConstructor(System.Type.EmptyTypes);
//                return new InstanceDescriptor(ci, null, false);
//            }
//            return base.ConvertTo(context, info, value, type);
//        }
//    }

//    public class TreeNodeEx : TreeNode
//    {
        
//    }

//    //public class EnhancedTreeView: TreeView
//    //{
//    //    private readonly List<TreeNode> _allNodes;

//    //    public List<TreeNode> AllNodes
//    //    {
//    //        get { return _allNodes; }
//    //    }

//    //    private List<TreeNodeEx> AllNodesInternal
//    //    {
//    //        get { return _allNodes.ConvertAll(TreeNodeEx.FromNode); }
//    //    }

//    //    public EnhancedTreeView()
//    //    {
//    //        _allNodes = new List<TreeNode>(); //new BindingList<TreeNode>());
//    //        //_allNodes[0].Nodes.Add(new TreeNodeEx());
//    //        //_allNodes.ListChanged += AllNodesChanged;
//    //    }

//    //    //private void AllNodesChanged(object sender, ListChangedEventArgs eventArguments)
//    //    //{
//    //    //    base.BeginUpdate();

//    //    //    base.Nodes.Clear();
//    //    //    foreach (var node in _allNodes)
//    //    //    {
//    //    //        base.Nodes.Add(node);
//    //    //    }

//    //    //    base.EndUpdate();
//    //    //}
//    //}

//    //public class TreeNodeEx : TreeNode
//    //{
//    //    //private BindingList<TreeNodeEx> _nodesEx = new BindingList<TreeNodeEx>();
//    //    //public BindingList<TreeNodeEx> NodesEx { get { return _nodesEx; } }
//    //    private TreeNodeEx(TreeNode treeNode)
//    //    {
            
//    //    }

//    //    internal NodeFilterStatuses NodeFilterStatus
//    //    {
//    //        get;
//    //        set;
//    //    }

//    //    internal static TreeNodeEx FromNode(TreeNode treeNode)
//    //    {
            
//    //    }
//    //    //public TreeNode TreeNode { get; set; }
//    //}

//    //public enum NodeFilterStatuses
//    //{
//    //    Test
//    //}
//}
