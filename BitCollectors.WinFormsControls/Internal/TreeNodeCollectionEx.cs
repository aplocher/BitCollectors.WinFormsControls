using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls.Internal
{
    public class TreeNodeCollectionEx : IList<TreeNodeEx>
    {
        private readonly TreeNodeCollection _baseCollection;
        private readonly List<TreeNodeEx> _baseList;
        private readonly bool _useTreeNodeCollection;

        internal ListChangedEventHandler ListChanged;

        public TreeNodeCollectionEx(TreeNodeCollection collection)
        {
            _useTreeNodeCollection = true;
            _baseCollection = collection;
        }

        public TreeNodeCollectionEx(List<TreeNodeEx> collection)
        {
            _useTreeNodeCollection = false;
            _baseList = collection;
            foreach (TreeNodeEx node in _baseList)
            {
                if (node != null)
                {
                    Add((TreeNodeEx)node.Clone());
                }
            }
        }

        public void Add(TreeNodeEx item)
        {
            if (_useTreeNodeCollection)
            {
                _baseCollection.Add(item);
            }
            else
            {
                _baseList.Add(item);
            }

            item.Nodes.ListChanged += (sender, args) =>
            {
                if (ListChanged != null)
                {
                    ListChanged(this, args);
                }
            };

            if (ListChanged != null)
            {
                ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, item.Index));
            }
        }

        public void Clear()
        {
            if (_useTreeNodeCollection)
                _baseCollection.Clear();
            else
                _baseList.Clear();

            if (ListChanged != null)
            {
                ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        public bool Contains(TreeNodeEx item)
        {
            return _useTreeNodeCollection
                ? _baseCollection.Contains(item)
                : _baseList.Contains(item);
        }

        public void CopyTo(TreeNodeEx[] array, int arrayIndex)
        {
            if (_useTreeNodeCollection)
                _baseCollection.CopyTo(array, arrayIndex);
            else
                _baseList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _useTreeNodeCollection
                    ? _baseCollection.Count
                    : _baseList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _useTreeNodeCollection && _baseCollection.IsReadOnly;
            }
        }

        public bool Remove(TreeNodeEx item)
        {
            int indexRemoved = item.Index;
            if (_useTreeNodeCollection)
                _baseCollection.Remove(item);
            else
                return _baseList.Remove(item);

            if (ListChanged != null)
                ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, indexRemoved));

            return true;
        }

        public IEnumerator<TreeNodeEx> GetEnumerator()
        {
            return _useTreeNodeCollection
                ? (IEnumerator<TreeNodeEx>)_baseCollection.GetEnumerator()
                : _baseList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _useTreeNodeCollection
                ? _baseCollection.GetEnumerator()
                : _baseList.GetEnumerator();
        }

        public int IndexOf(TreeNodeEx item)
        {
            return _useTreeNodeCollection
                ? _baseCollection.IndexOf(item)
                : _baseList.IndexOf(item);
        }

        public void Insert(int index, TreeNodeEx item)
        {
            if (_useTreeNodeCollection)
                _baseCollection.Insert(index, item);
            else
                _baseList.Insert(index, item);

            if (ListChanged != null)
                ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        public void RemoveAt(int index)
        {
            if (_useTreeNodeCollection)
                _baseCollection.RemoveAt(index);
            else
                _baseList.RemoveAt(index);

            if (ListChanged != null)
                ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        public TreeNodeEx this[int index]
        {
            get
            {
                if (_useTreeNodeCollection)
                {
                    if (_baseCollection[index] == null)
                        return null;

                    return _baseCollection[index] as TreeNodeEx;
                }
                else
                {
                    if (_baseList[index] == null)
                        return null;

                    return _baseList[index];
                }
            }
            set
            {
                if (_useTreeNodeCollection)
                    _baseCollection[index] = value;
                else
                    _baseList[index] = value;
            }
        }

        public TreeNodeEx[] Find(string key, bool searchAllChildren)
        {
            var nodes = _baseCollection.Find(key, searchAllChildren);
            var returnValue = new TreeNodeEx[nodes.Length];
            for (var i = 0; i < nodes.Length; i++)
            {
                returnValue[i] = nodes[i] as TreeNodeEx;
            }

            return returnValue;
        }
    }

}
