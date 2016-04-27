using System;
using System.Collections.Generic;
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
        private TextBoxEx _attachedSearchControl = null;
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

        public new TreeNodeCollectionEx Nodes => this.AllNodes;

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
            }

            base.WndProc(ref m);
        }

        private Object _lockObject = new Object();

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (e?.Node == null || e.Node.Bounds.IsEmpty || !e.Node.IsVisible)
                return;

            if (string.IsNullOrEmpty(_filterToRun))
            {
                e.DrawDefault = true;
            }
            else
            {
                var treeNodeEx = (TreeNodeEx)e.Node;
                if (treeNodeEx.IsMatch)
                {
                    var stringSize = TextRenderer.MeasureText(e.Node.Text.Substring(treeNodeEx.MatchStartIndex, treeNodeEx.MatchEndIndex - treeNodeEx.MatchStartIndex), e.Node.NodeFont);
                    int padding = -1;
                    var startOffset = 0;
                    if (treeNodeEx.MatchStartIndex > 0)
                    {
                        var measurementOffset =
                            TextRenderer.MeasureText(e.Node.Text.Substring(0, treeNodeEx.MatchStartIndex),
                                e.Node.NodeFont);
                        startOffset = measurementOffset.Width;
                        padding = 6;
                    }

                    var curState = e.State;
                    var font = e.Node.NodeFont ?? Font;
                    var color = (((curState & TreeNodeStates.Selected) == TreeNodeStates.Selected) && Focused)
                        ? SystemColors.HighlightText
                        : (e.Node.ForeColor != Color.Empty) ? e.Node.ForeColor : ForeColor;

                    if ((curState & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                        ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, color, SystemColors.Highlight);
                    }
                    else
                    {
                        using (Brush brush = new SolidBrush(BackColor))
                        {
                            e.Graphics.FillRectangle(brush, e.Bounds);
                        }
                    }

                    e.Graphics.FillRectangle(Brushes.Orange, e.Node.Bounds.Left + startOffset - padding, e.Node.Bounds.Top + 1, stringSize.Width - 6, e.Node.Bounds.Height - 4);

                    if ((curState & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                    {
                        TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, color, TextFormatFlags.Default);
                    }
                    else
                    {
                        TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, color, TextFormatFlags.Default);
                    }
                }
                else
                {
                    e.DrawDefault = true;
                }

                //var treeNodeEx = (TreeNodeEx)e.Node;
                //var stringSize = TextRenderer.MeasureText(e.Node.Text.Substring(treeNodeEx.MatchStartIndex, treeNodeEx.MatchEndIndex - treeNodeEx.MatchStartIndex), e.Node.NodeFont);
                //var startOffset = 0;
                //if (treeNodeEx.IsMatch)
                //{
                //    int padding = -1;
                //    if (treeNodeEx.MatchStartIndex > 0)
                //    {
                //        var measurementOffset =
                //            TextRenderer.MeasureText(e.Node.Text.Substring(0, treeNodeEx.MatchStartIndex),
                //                e.Node.NodeFont);
                //        startOffset = measurementOffset.Width;
                //        padding = 6;
                //    }

                //    e.Graphics.FillRectangle(Brushes.Orange, e.Node.Bounds.Left + startOffset - padding, e.Node.Bounds.Top + 3, stringSize.Width - 6, e.Node.Bounds.Height - 6);
                //}

                //var textColor = e.Node.IsSelected && this.Focused ? SystemColors.HighlightText : e.Node.ForeColor;

                //TextRenderer.DrawText(e.Graphics,
                //    e.Node.Text,
                //    e.Node.NodeFont,
                //    //this.Font,
                //    e.Bounds,
                //    textColor,
                //    Color.Empty,
                //    TextFormatFlags.VerticalCenter);


                //Simulate default text drawing here 


                //Font font = (node.NodeFont != null) ? node.NodeFont : node.TreeView.Font;
                //Color color = (((curState & TreeNodeStates.Selected) == TreeNodeStates.Selected) && node.TreeView.Focused) ? SystemColors.HighlightText : (node.ForeColor != Color.Empty) ? node.ForeColor : node.TreeView.ForeColor;

                // Draw the actual node. 
                //if ((curState & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                //{
                //    g.FillRectangle(SystemBrushes.Highlight, bounds);
                //    ControlPaint.DrawFocusRectangle(g, bounds, color, SystemColors.Highlight);
                //    TextRenderer.DrawText(g, e.Node.Text, font, bounds, color, TextFormatFlags.Default);
                //}
                //else
                //{
                //    // For Dev10 bug 478621: We need to draw the BackColor of 
                //    // nodes same as BackColor of treeview.
                //    using (Brush brush = new SolidBrush(BackColor))
                //    {
                //        g.FillRectangle(brush, bounds);
                //    }

                //    TextRenderer.DrawText(g, e.Node.Text, font, bounds, color, TextFormatFlags.Default);
                //}
            }

            base.OnDrawNode(e);

            // see http://stackoverflow.com/questions/9136910/treeview-custom-drawnode-net-3-5-windows-forms
        }

        #endregion

        #region Public methods
        public TextBoxEx AttachedSearchControl
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
        internal bool IsFiltered => !string.IsNullOrEmpty(_lastFilter);
        #endregion

        #region Private methods
        private void Filter(string filter, bool force)
        {
            filter = (filter ?? "").Trim();
            if (_lastFilter == filter && !force)
                return;

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
                    this.SelectedNode = this.TopMatchedNode;
                }
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
            FilterInternal(this.FilteredNodes, filter, false);

            RemoveMarkedNodes();

            this.Invalidate();
        }

        private bool FilterInternal(TreeNodeCollectionEx nodes, string filter, bool hasMatchingAncestor)
        {
            var loweredFilter = filter.ToLower();
            var filterLength = loweredFilter.Length;
            var returnValue = false;

            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var treeText = (node.Text ?? "").Trim().ToLower();
                var index = treeText.IndexOf(loweredFilter, StringComparison.Ordinal);
                var isMatch = (index != -1);

                if (isMatch && TopMatchedNode == null)
                    TopMatchedNode = node;

                var hasMatchingDescendents =
                    node.Nodes.Count > 0 &&
                    FilterInternal(node.Nodes, filter, hasMatchingAncestor || isMatch);

                node.IsMatch = isMatch;
                if (isMatch)
                {
                    node.MatchStartIndex = index;
                    node.MatchEndIndex = index + filterLength;
                }

                node.HasMatchingAncestors = hasMatchingAncestor;
                node.HasMatchingDescendents = hasMatchingDescendents;

                returnValue = returnValue || isMatch || hasMatchingDescendents;
            }

            return returnValue;
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
                    RemoveMarkedNodes(nodes[i].Nodes);

                bool hasAnyMatchInFamily =
                    nodes[i].HasMatchingAncestors ||
                    nodes[i].HasMatchingDescendents ||
                    nodes[i].IsMatch;

                if (!hasAnyMatchInFamily)
                    nodes.RemoveAt(i);
                else if (nodes[i].HasMatchingDescendents)
                    nodes[i].Expand();
                else
                    nodes[i].Collapse();
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
