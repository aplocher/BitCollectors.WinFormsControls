using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.Common.Win32;
using BitCollectors.WinFormsControls.Internal;

namespace BitCollectors.WinFormsControls
{
    public class TreeViewEx : System.Windows.Forms.TreeView
    {
        private const int _skipMessageCount = 5;

        #region Private fields
        private readonly List<TreeNodeEx> _allNodesLow = new List<TreeNodeEx>();
        private bool _suppressKillFocusCheckOnce;
        private bool _suppressSetFocusCheckOnce;
        private int _currentScrollMessageCount;
        private bool _useAttachedSearchControl = false;
        private string _lastFilter = null;
        private System.Windows.Forms.TextBox _attachedSearchControl = null;
        private TreeNodeCollectionEx _allNodes;
        private TreeNodeCollectionEx _filteredNodes;
        private SearchControlWrapper _searchControlWrapper = null;

        #endregion

        protected override void CreateHandle()
        {
            base.CreateHandle();
        }

        public bool AllowVerticalScrollbars { get; set; }
        public bool AllowHorizontalScrollbars { get; set; }

        private TreeViewExThemeStyles _themeStyle = TreeViewExThemeStyles.Default;

        public TreeViewExThemeStyles ThemeStyle
        {
            get { return _themeStyle; }
            set
            {
                if (Environment.OSVersion.Platform != PlatformID.Win32NT ||
                    Environment.OSVersion.Version.Major < 6 ||
                    _themeStyle == value)
                    return;

                _themeStyle = value;

                switch (_themeStyle)
                {
                    case TreeViewExThemeStyles.Default:
                        NativeMethods.SetWindowTheme(this.Handle, null, null);
                        break;

                    case TreeViewExThemeStyles.Explorer:
                        NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
                        break;
                }
            }
        }

        public enum TreeViewExThemeStyles
        {
            /// <summary>
            /// Use default appearance of TreeView with + and - buttons and dotted lines to connect the nodes
            /// </summary>
            Default,
            /// <summary>
            /// Use the (typically) more modern appearance of the TreeView in Explorer
            /// </summary>
            Explorer
        }

        public bool EnterKeyCyclesMatches { get; set; }

        public TreeNodeEx TopMatchedNode
        {
            get;
            private set;
        }

        public TreeViewEx()
        {
            base.DrawMode = TreeViewDrawMode.OwnerDrawText;
        }

        #region Redefined members ('new' keyword)
        public new TreeNodeEx SelectedNode
        {
            get { return base.SelectedNode as TreeNodeEx; }
            set { base.SelectedNode = value; }
        }

        public new TreeNodeCollectionEx Nodes
        {
            get
            {
                return this.AllNodes;
            }
        }
        #endregion

        #region Overriden members ('override' keyword)
        protected override void OnHandleCreated(EventArgs e)
        {
            NativeMethods.SendMessage(this.Handle, NativeMethods.TVM_SETEXTENDEDSTYLE, (IntPtr)NativeMethods.TVS_EX_DOUBLEBUFFER, (IntPtr)NativeMethods.TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // Verify this, per http://stackoverflow.com/questions/9136910/treeview-custom-drawnode-net-3-5-windows-forms
                // -AP
                //
                // Removes all the flickering of repainting node's
                // This is the only thing I found that works properly for doublebuffering a treeview.
                var param = base.CreateParams;
                param.ExStyle |= 0x02000000; // WS_CLIPCHILDREN
                return param;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (EnterKeyCyclesMatches && !string.IsNullOrEmpty(_lastFilter))
                    {
                        this.SelectedNode = this.SelectedNode.NextMatchedNode;
                        e.Handled = true;
                    }
                    break;
            }

            base.OnKeyUp(e);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_VSCROLL:
                case NativeMethods.WM_HSCROLL:
                    var wParam = m.WParam.ToInt32() & 0xFFFF;
                    if (wParam == NativeMethods.SB_THUMBTRACK)
                    {
                        _currentScrollMessageCount++;
                        if (_currentScrollMessageCount % _skipMessageCount == 0)
                        {
                            base.WndProc(ref m);
                        }

                        return;
                    }

                    if (wParam == NativeMethods.SB_ENDSCROLL)
                    {
                        _currentScrollMessageCount = 0;
                    }
                    break;

                    //case NativeMethods.WM_SETFOCUS:
                    //    if (!_useAttachedSearchControl)
                    //    {
                    //        base.WndProc(ref m);
                    //        return;
                    //    }

                    //    if (_suppressSetFocusCheckOnce)
                    //    {
                    //        _suppressSetFocusCheckOnce = false;
                    //    }
                    //    else if (this.Focused || _attachedSearchControl.Focused ||
                    //        m.WParam.ToString() == this.Handle.ToString() ||
                    //        m.WParam.ToString() == _attachedSearchControl.Handle.ToString())
                    //    {
                    //        _suppressSetFocusCheckOnce = true;
                    //        NativeMethods.PostMessage(_attachedSearchControl.Handle, NativeMethods.WM_SETFOCUS, m.WParam, m.LParam);
                    //    }
                    //    break;

                    //case NativeMethods.WM_KILLFOCUS:
                    //    if (!_useAttachedSearchControl)
                    //    {
                    //        base.WndProc(ref m);
                    //        return;
                    //    }

                    //    if (_useAttachedSearchControl)
                    //    {
                    //        if (_suppressKillFocusCheckOnce)
                    //        {
                    //            _suppressKillFocusCheckOnce = false;
                    //        }
                    //        else if (!_attachedSearchControl.Focused && !this.Focused &&
                    //                 m.WParam.ToString() != this.Handle.ToString() &&
                    //                 m.WParam.ToString() != _attachedSearchControl.Handle.ToString())
                    //        {
                    //            _suppressKillFocusCheckOnce = true;
                    //            NativeMethods.PostMessage(_attachedSearchControl.Handle, m.Msg, m.WParam, m.LParam);
                    //        }
                    //        else
                    //        {
                    //            return;
                    //        }
                    //    }
                    //    break;
            }

            base.WndProc(ref m);
        }

        private Object _lockObject = new Object();

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (e == null || e.Node == null || e.Node.Bounds.IsEmpty || !e.Node.IsVisible)
                return;

            if (string.IsNullOrEmpty(_filterToRun))
            {
                e.DrawDefault = true;
            }
            else
            {
                var treeNodeEx = (TreeNodeEx)e.Node;
                var stringSize = TextRenderer.MeasureText(e.Node.Text.Substring(treeNodeEx.MatchStartIndex, treeNodeEx.MatchEndIndex - treeNodeEx.MatchStartIndex), e.Node.NodeFont);
                var startOffset = 0;
                if (treeNodeEx.FilterType == FilterTypes.DirectMatch)
                {
                    int padding = -1;
                    if (treeNodeEx.MatchStartIndex > 0)
                    {
                        var measurementOffset =
                            TextRenderer.MeasureText(e.Node.Text.Substring(0, treeNodeEx.MatchStartIndex),
                                e.Node.NodeFont);
                        startOffset = measurementOffset.Width;
                        padding = 6;
                    }

                    e.Graphics.FillRectangle(Brushes.Orange, e.Node.Bounds.Left + startOffset - padding, e.Node.Bounds.Bottom - 2, stringSize.Width - 6, e.Node.IsSelected ? 1 : 2);
                }

                var textColor = e.Node.IsSelected && this.Focused ? Color.White : e.Node.ForeColor;

                TextRenderer.DrawText(e.Graphics,
                    e.Node.Text,
                    e.Node.NodeFont,
                    //this.Font,
                    e.Bounds,
                    textColor,
                    Color.Empty,
                    TextFormatFlags.VerticalCenter);
            }



            base.OnDrawNode(e);

            // see http://stackoverflow.com/questions/9136910/treeview-custom-drawnode-net-3-5-windows-forms
        }

        #endregion

        #region Public methods
        public System.Windows.Forms.TextBox AttachedSearchControl
        {
            get { return _attachedSearchControl; }
            set
            {
                _attachedSearchControl = value;
                if (_useAttachedSearchControl == (_attachedSearchControl != null))
                    return;

                _useAttachedSearchControl = _attachedSearchControl != null;
                if (_useAttachedSearchControl)
                {
                    _searchControlWrapper = new SearchControlWrapper();
                    _searchControlWrapper.WireUpAttachedSearchControl(value, this);
                }
                else if (_searchControlWrapper != null)
                {
                    // TODO unwire
                }
            }
        }

        public TreeNodeCollectionEx FilteredNodes
        {
            get
            {
                InitializeNodes();
                return _filteredNodes;
            }
        }

        public TreeNodeCollectionEx AllNodes
        {
            get
            {
                InitializeNodes();

                return _allNodes;
            }
        }

        private volatile bool _filterWaiting = false;
        private string _filterToRun = string.Empty;
        private Timer _filterWaitTimer = null;


        public void Filter(string filter)
        {
            if (filter == _filterToRun)
                return;

            _filterToRun = filter;

            if (_filterWaiting)
                return;

            _filterWaiting = true;

            if (_filterWaitTimer == null)
            {
                _filterWaitTimer = new Timer { Interval = 200 };
                _filterWaitTimer.Tick += (sender, args) =>
                {
                    _filterWaitTimer.Stop();

                    try
                    {
                        Filter(_filterToRun, false);
                    }
                    finally
                    {
                        _filterWaiting = false;
                    }
                };
            }

            _filterWaitTimer.Start();
        }
        #endregion

        #region Internal members
        internal bool IsFiltered
        {
            get { return !string.IsNullOrEmpty(_lastFilter); }
        }
        #endregion

        #region Private methods
        private string[] EncodeNodePath(TreeNodeEx treeNode)
        {
            if (treeNode == null)
                return null;

            var node = treeNode;
            var returnValue = new string[node.Level + 1];
            do
            {
                returnValue[node.Level] = string.Format(@"{0}|{1}|{2}", node.Level, node.Index, node.Text);
                node = node.Parent;
            } while (node != null);

            return returnValue;
        }

        private string EncodeNodePathString(TreeNodeEx treeNode)
        {
            return string.Join(this.PathSeparator, EncodeNodePath(treeNode));
        }

        private void Filter(string filter, bool force)
        {
            filter = (filter ?? "").Trim();
            if (_lastFilter == filter && !force)
            {
                return;
            }

            string[] selectedNodePath = this.SelectedNode == null ? null : EncodeNodePath(this.SelectedNode);
            // http://stackoverflow.com/questions/332788/maintain-scroll-position-of-treeview
            base.BeginUpdate();
            base.Nodes.Clear();
            Console.WriteLine(filter);
            if (string.IsNullOrEmpty(filter))
            {
                for (var i = 0; i < this.AllNodes.Count; i++)
                {
                    base.Nodes.Add((TreeNode)this.AllNodes[i].Clone());
                }
            }
            else
            {
                lock (_lockObject)
                {
                    FilteredNodes.Clear();

                    for (var i = 0; i < this.AllNodes.Count; i++)
                    {
                        FilteredNodes.Add(this.AllNodes[i].Clone() as TreeNodeEx);
                    }

                    FilterInternal(filter);
                    this.ExpandAll();
                    this.SelectedNode = this.TopMatchedNode;
                }
            }

            // TODO maybe revisit this?  Or, maybe not
            if (1 == 0) //selectedNodePath != null)
            {
                var collection = this.FilteredNodes;
                TreeNodeEx selectedNode = null;
                foreach (var pathElement in selectedNodePath)
                {
                    selectedNode = Find(collection, int.Parse(pathElement.Split('|')[0]), pathElement.Split('|')[2]);
                    if (selectedNode == null)
                        break;

                    collection = selectedNode.Nodes;
                }

                if (selectedNode != null)
                    this.SelectedNode = selectedNode;
            }

            if (base.Nodes.Count > 0 && base.Nodes[0] != null)
                base.Nodes[0].EnsureVisible();

            base.EndUpdate();

            _lastFilter = filter;
        }

        private TreeNodeEx Find(int level, string text)
        {
            return Find(this.Nodes, level, text);
        }

        private TreeNodeEx Find(TreeNodeCollectionEx collection, int level, string text)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Text == text && collection[i].Level == level)
                    return collection[i];
            }

            return null;
        }

        private void FilterInternal(string filter)
        {
            this.TopMatchedNode = null;
            FilterInternal(this.FilteredNodes, filter);

            RemoveMarkedNodes();

            this.Invalidate();
        }

        private void FilterInternal(TreeNodeCollectionEx nodes, string filter)
        {
            var loweredFilter = filter.ToLower();
            var filterLength = loweredFilter.Length;

            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Nodes.Count > 0)
                {
                    FilterInternal(nodes[i].Nodes, filter);
                }

                string treeText = (nodes[i].Text ?? "").Trim().ToLower();
                var index = treeText.IndexOf(loweredFilter, StringComparison.Ordinal);
                //if (treeText.ToLower().Contains(filter.ToLower()))
                if (index != -1)
                {
                    TreeNodeEx node = nodes[i];
                    do
                    {
                        //node.Expand();
                        node.FilterType = FilterTypes.DescendentOfMatch;
                        node = node.Parent;
                    } while (node != null);

                    // Need to recurse through children and re-add them and collapse this item.
                    // If children don't match filter, then collapse the    
                    // Ex: search for a folder, it should come back but it needs to be useable and children need to be accessible, just collapsed (unless children match filter)
                    if (this.TopMatchedNode == null)
                    {
                        this.TopMatchedNode = nodes[i];
                        Debug.WriteLine(this.TopMatchedNode.Text);
                    }

                    nodes[i].FilterType = FilterTypes.DirectMatch;
                    nodes[i].MatchStartIndex = index;
                    nodes[i].MatchEndIndex = index + filterLength;
                }
            }

            //if (nodes.Count > 0)
            //{
            //    nodes[0].Expand();
            //}
        }

        private void RemoveMarkedNodes()
        {
            RemoveMarkedNodes(this.FilteredNodes);
        }

        private void RemoveMarkedNodes(TreeNodeCollectionEx nodes)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].Nodes.Count > 0)
                {
                    RemoveMarkedNodes(nodes[i].Nodes);
                }

                if (nodes[i].FilterType == FilterTypes.NoMatch || nodes[i].FilterType == FilterTypes.Undefined)
                    nodes.RemoveAt(i);
            }
        }

        private void RefreshFilter()
        {
            Filter(_lastFilter, true);
        }

        private void InitializeNodes(bool forceRefresh = false)
        {
            if (forceRefresh || _filteredNodes == null)
            {
                _filteredNodes = new TreeNodeCollectionEx(base.Nodes);
            }

            if (forceRefresh || _allNodes == null)
            {
                _allNodes = new TreeNodeCollectionEx(_allNodesLow);
                _allNodes.ListChanged += (sender, args) => RefreshFilter();
            }
        }
        #endregion
    }
}
