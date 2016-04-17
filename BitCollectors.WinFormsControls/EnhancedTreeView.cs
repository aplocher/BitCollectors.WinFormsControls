using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BitCollectors.WinFormsControls.EnhancedTreeViewComponents;

namespace BitCollectors.WinFormsControls
{
    public class EnhancedTreeView : TreeView
    {
        private string _lastFilter = "";

        private EnhancedTreeNodeList _allNodes = new EnhancedTreeNodeList();
        private bool _enableNodeListChangedEvents = true;
        private TreeView _allNodesTreeView = new TreeView();

        //private List<EnhancedTreeNode> _filteredNodes = new List<EnhancedTreeNode>();

        private string _filterString = null;

        public EnhancedTreeView()
        {
            _allNodes.ListChanged += (sender, args) =>
            {
                if (_enableNodeListChangedEvents)
                {
                    _allNodesTreeView.Nodes.Clear();
                    foreach (var node in _allNodes)
                    {
                        _allNodesTreeView.Nodes.Add(node);
                    }
                }
            };
        }

        public EnhancedTreeNodeList AllNodes
        {
            get
            {
                if (_allNodes == null)
                {
                    _allNodes = ConvertBaseNodesToList();
                }

                return _allNodes;
            }
        }


        private void ConvertListToBaseNodes<T>(List<T> treeNodeList)
        {
            base.BeginUpdate();
            base.Nodes.Clear();
            base.Nodes.AddRange(treeNodeList.Cast<TreeNode>().ToArray());
            base.EndUpdate();
        }

        //private List<TreeNode> ConvertBaseNodesToList()
        //{
        //    return base.Nodes.Cast<TreeNode>().ToList();
        //}

        private EnhancedTreeNodeList ConvertBaseNodesToList()
        {
            var returnValue = new EnhancedTreeNodeList();

            for (int i = 0; i < base.Nodes.Count; i++)
            {
                returnValue.Add(base.Nodes[i] as EnhancedTreeNode);
            }

            return returnValue;
        }

        private void InitializeUnfilteredList()
        {
            if (string.IsNullOrEmpty(_lastFilter))
            {
                _allNodes.Clear();
                foreach (TreeNode node in base.Nodes)
                {
                    _allNodes.Add(EnhancedTreeNode.GetFromTreeNode((TreeNode)node.Clone()));
                }
            }

            if (_allNodes == null)
            {
                throw new InvalidDataException("Root connection nodes not set");
            }
        }

        //private static void FilterInternal(TreeNodeCollection nodes, string filter, int depth = 0)
        //{
        //    var treeNodeList = new List<EnhancedTreeNode>();
        //    foreach (var node in nodes)
        //    {
        //        treeNodeList.Add((EnhancedTreeNode)node);
        //    }

        //    FilterInternal(treeNodeList, filter, depth);
        //}

        private static void FilterInternal(EnhancedTreeNodeList nodes, string filter, int depth = 0)
        {
            foreach (var child in nodes)
            {
                if (child.Nodes.Count > 0)
                {
                    FilterInternal(child.Nodes, filter, depth + 1);
                }

                string treeText = (child.Text ?? "").Trim();
                if (treeText.ToLower().Contains(filter.ToLower()))
                {
                    EnhancedTreeNode node = child;
                    while (node.Parent != null)
                    {
                        node.Expand();
                        //node.Checked = true;
                        node.FilterType = FilterTypes.MatchesCriteria;
                        node = EnhancedTreeNode.GetFromTreeNode(node.Parent);
                    }

                    // Need to recurse through children and re-add them and collapse this item.
                    // If children don't match filter, then collapse the folder
                    // Ex: search for a folder, it should come back but it needs to be useable and children need to be accessible, just collapsed (unless children match filter)
                    child.FilterType = FilterTypes.MatchesCriteria;

                    CollapseChildrenIfNotMatched(child);
                }
            }

            RemoveMarkedNodes(nodes);

            if (nodes.Count > 0)
            {
                nodes[0].Expand();
            }
        }

        private static void CollapseChildrenIfNotMatched(EnhancedTreeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                foreach (EnhancedTreeNode n in node.Nodes)
                {
                    CollapseChildrenIfNotMatched(n);
                }
            }

            if (node.Parent != null)
            {
                node.Parent.Expand();
            }

            node.Collapse();

            if (node.FilterType != FilterTypes.MatchesCriteria)
            {
                node.FilterType = FilterTypes.ContainerMatchesCriteria;
            }

            //if (node.FilterType == FilterTypes.MatchesCriteria)
            //{
            //    //node.Expand();
            //}
            //else
            //{
            //    node.FilterType = FilterTypes.ContainerMatchesCriteria;
            //    //node.Collapse();
            //}
        }

        private static void RemoveMarkedNodes(TreeNodeCollection nodes)
        {
            var treeNodeList = new List<TreeNode>();
            foreach (var node in nodes)
            {
                treeNodeList.Add((TreeNode)node);
            }

            RemoveMarkedNodes(treeNodeList);
        }

        private static void RemoveMarkedNodes(IEnumerable<TreeNode> nodes)
        {
            var enhancedTreeNodeList = new List<EnhancedTreeNode>();
            foreach (EnhancedTreeNode node in nodes)
            {
                enhancedTreeNodeList.Add(node);
            }

            //enhancedTreeNodeList.Add((EnhancedTreeNode)node);
            RemoveMarkedNodes(enhancedTreeNodeList);
        }

        private static void RemoveMarkedNodes(EnhancedTreeNodeList nodes)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].Nodes.Count > 0)
                {
                    RemoveMarkedNodes(nodes[i].Nodes);
                }

                if (nodes[i].FilterType == FilterTypes.Hide || nodes[i].FilterType == FilterTypes.Undefined)
                    nodes.RemoveAt(i);
            }
        }

        public void Filter(string filter, bool forceRefresh = false)
        {
            filter = (filter ?? "").Trim();
            if (!forceRefresh && _lastFilter == filter)
            {
                return;
            }

            //InitializeUnfilteredList();

            string selectedNodeName;
            string selectedNodePath;

            if (this.SelectedNode == null)
            {
                selectedNodeName = null;
                selectedNodePath = null;
            }
            else
            {
                selectedNodeName = this.SelectedNode.Name;
                selectedNodePath = this.SelectedNode.FullPath;
            }

            _enableNodeListChangedEvents = false;
            base.BeginUpdate();
            base.Nodes.Clear();
            if (string.IsNullOrEmpty(filter))
            {
                for (int i = 0; i < _allNodes.Count; i++)
                {
                    base.Nodes.Add((TreeNode)_allNodes[i].Clone());
                }
            }
            else
            {
                var allNodes = CloneTreeNodeList(_allNodes);
                FilterInternal(allNodes, filter);
                for (int i = 0; i < allNodes.Count; i++)
                {
                    if (allNodes[i] != null)
                    {
                        base.Nodes.Add(allNodes[i]);
                    }
                }
            }

            var findResults = base.Nodes.Find(selectedNodeName, true);
            for (int i = 0; i < findResults.Length; i++)
            {
                if (findResults[i].FullPath == selectedNodePath)
                {
                    this.SelectedNode = findResults[i];
                    break;
                }
            }

            _enableNodeListChangedEvents = true;
            base.EndUpdate();

            _lastFilter = filter;
        }

        private void FilterInternal(string filter)
        {

        }

        private EnhancedTreeNodeList CloneTreeNodeList(EnhancedTreeNodeList treeNodeList)
        {
            var returnValue = new EnhancedTreeNodeList();
            foreach (EnhancedTreeNode treeNode in treeNodeList)
            {
                var clone = treeNode.Clone();
                var casted = (TreeNode)clone;
                var converted = EnhancedTreeNode.GetFromTreeNode(casted);

                returnValue.Add(converted);
            }
            return returnValue;
        }
    }

    //public class TreeViewEx : TreeView
    //{
    //    private TreeNodeCollection _allNodes = new TreeNodeCollection();
    //    public new TreeNodeCollection Nodes { get { return _allNodes; } }

    //    public TreeNodeCollection FilteredNodes { get { return base.Nodes; } }

    //    public void Filter(string searchString)
    //    {

    //        base.Nodes.Clear();
    //        foreach (TreeNode node in FilterInternal(searchString))
    //        {
    //            base.Nodes.Add(node);
    //        }
    //    }
    //}
}
