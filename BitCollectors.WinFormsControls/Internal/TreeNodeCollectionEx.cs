using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public int Count => _useTreeNodeCollection
            ? _baseCollection.Count
            : _baseList.Count;

        public bool IsReadOnly => _useTreeNodeCollection && _baseCollection.IsReadOnly;

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
                ? GetEnumeratorConverted()
                : _baseList.GetEnumerator();
        }

        private IEnumerator<TreeNodeEx> GetEnumeratorConverted()
        {
            foreach (var item in _baseCollection)
            {
                yield return item as TreeNodeEx;
            }
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

            ListChanged?.Invoke(this, new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        public void RemoveAt(int index)
        {
            if (_useTreeNodeCollection)
                _baseCollection.RemoveAt(index);
            else
                _baseList.RemoveAt(index);

            ListChanged?.Invoke(this, new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        public TreeNodeEx this[int index]
        {
            get
            {
                if (_useTreeNodeCollection)
                {
                    return _baseCollection[index] == null ? null : _baseCollection[index] as TreeNodeEx;
                }
                else
                {
                    return _baseList[index] ?? null;
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

        public virtual TreeNode this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                    return null;

                if (_useTreeNodeCollection)
                    return _baseCollection[key];

                int index = IndexOfKey(key);
                return this[index];
            }
        }

        public virtual int IndexOfKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return -1;
            }

            for (var i = 0; i < this.Count; i++)
            {
                if (this[i].Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }

        public TreeNodeEx[] Find(string key, bool searchAllChildren)
        {
            if (_useTreeNodeCollection)
            {
                var nodes = _baseCollection.Find(key, searchAllChildren);
                return nodes.Cast<TreeNodeEx>().ToArray();
            }
            else
            {
                var array = _baseList.ToArray();
                var foundArray = new ArrayList();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i]?.Name == null)
                        continue;

                    if (array[i].Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                        foundArray.Add(array[i]);
                }

                var returnValue = new TreeNodeEx[foundArray.Count];
                foundArray.CopyTo(returnValue);

                return returnValue;
            }
        }

        public void AddRange(TreeNodeEx[] nodes)
        {
            foreach (var node in nodes)
            {
                this.Add(node);
            }
        }

        public virtual bool ContainsKey(string key)
        {
            if (_useTreeNodeCollection)
            {
                return _baseCollection.ContainsKey(key);
            }
            else
            {
                for (var i = 0; i < this.Count; i++)
                {
                    if (this[i]?.Name == null)
                        continue;

                    if (this[i].Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                        return true;
                }

                return false;
            }
        }
    }

}
