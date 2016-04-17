using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls.Internal
{
    internal class SearchControlWrapper
    {
        private TextBox _searchInputControl;
        private TreeViewEx _treeViewControl;

        private void AssignSearchControlWndProcHook(TreeViewEx treeView)
        {
            var action = new Action(() =>
            {
                var searchHook = new SearchControlNativeWindowHooks { TreeViewControl = treeView };
                searchHook.AssignHandle(_searchInputControl.Handle);
            });

            _searchInputControl.HandleCreated += (sender, args) => action();

            if (_searchInputControl.IsHandleCreated)
            {
                action();
            }
        }

        private void HandleSearchInputKeyDown(KeyEventArgs keyArguments)
        {
            switch (keyArguments.KeyCode)
            {
                case Keys.Down:
                    keyArguments.Handled = true;
                    keyArguments.SuppressKeyPress = true;
                    _treeViewControl.SelectedNode = _treeViewControl.SelectedNode == null
                        ? _treeViewControl.Nodes[0]
                        : (TreeNodeEx)_treeViewControl.SelectedNode.NextNode;
                    _treeViewControl.Focus();
                    break;

                case Keys.Up:
                    keyArguments.Handled = true;
                    keyArguments.SuppressKeyPress = true;
                    _treeViewControl.SelectedNode = _treeViewControl.SelectedNode == null
                        ? _treeViewControl.Nodes.Last()
                        : (TreeNodeEx)_treeViewControl.SelectedNode.PrevNode;
                    _treeViewControl.Focus();
                    break;
            }
        }

        private Task _performSearchTask;
        private long _executeSearchAt = 0;
        private TaskScheduler _mainContext = TaskScheduler.Current;
        private volatile bool _keyWaitRunning = false;

        private void HandleSearchInputKeyUp(KeyEventArgs keyArguments)
        {
            _treeViewControl.Filter(_searchInputControl.Text);

            //if (string.IsNullOrEmpty(_searchInputControl.Text))
            //{
            //    _treeViewControl.Filter(string.Empty);
            //}
            //else
            //{
            //    int sleepTime = 1000;
            //    _executeSearchAt = DateTime.Now.AddMilliseconds(sleepTime).Ticks;

            //    if (_keyWaitRunning)
            //        return;

            //    _keyWaitRunning = true;
            //    //if (_performSearchTask == null)
            //    //{
            //    _performSearchTask = Task.Factory.StartNew(() =>
            //    {
            //        int looped = 0;
            //        do
            //        {
            //            Thread.Sleep(sleepTime);
            //            looped++;
            //        } while (DateTime.Now.Ticks < _executeSearchAt && looped < 5);
            //    }).ContinueWith(t =>
            //        {
            //            _keyWaitRunning = false;
            //            _treeViewControl.Filter(_searchInputControl.Text);
            //        },
            //            CancellationToken.None,
            //            TaskContinuationOptions.None,
            //            _mainContext);
            //    //}
            //    //else
            //    //{
            //    //    _performSearchTask.Start();
            //    //}
            //}
        }

        private void HandleTreeViewKeyDown(KeyEventArgs keyArguments)
        {
            if (_searchInputControl.Focused)
                return;

            var inputChar = (char)keyArguments.KeyCode;
            if (char.IsLetterOrDigit(inputChar))
            {
                _searchInputControl.Text += GetKeyCharWithProperCasing(keyArguments.Shift, inputChar);
                _searchInputControl.Focus();
                _searchInputControl.SelectionStart = _searchInputControl.Text.Length;
                keyArguments.Handled = true;
            }
            else
            {
                switch (keyArguments.KeyCode)
                {
                    case Keys.Escape:
                        _searchInputControl.Text = string.Empty;
                        keyArguments.Handled = true;
                        break;

                    case Keys.Back:
                        _searchInputControl.Focus();
                        if (_searchInputControl.SelectionStart != 1)
                        {
                            bool cursorAtEnd = _searchInputControl.SelectionStart == _searchInputControl.Text.Length;
                            _searchInputControl.Text = _searchInputControl.Text.Substring(0, _searchInputControl.Text.Length - 1);

                            if (cursorAtEnd)
                            {
                                _searchInputControl.SelectionStart = _searchInputControl.Text.Length;
                            }
                        }
                        keyArguments.Handled = true;
                        break;

                }
            }
        }

        private char GetKeyCharWithProperCasing(bool shiftPressed, char keyCode)
        {
            bool capsLocked = Control.IsKeyLocked(Keys.CapsLock);
            if ((shiftPressed || capsLocked) && shiftPressed != capsLocked)
            {
                return char.ToUpper(keyCode);
            }
            else
            {
                return char.ToLower(keyCode);
            }
        }

        private bool _isEnhancedTextBox = false;
        private TextBoxEx _enhancedTextBox = null;

        public void WireUpAttachedSearchControl(TextBox control, TreeViewEx treeView)
        {
            this._searchInputControl = control;
            this._treeViewControl = treeView;
            this._treeViewControl.TabStop = false;

            AssignSearchControlWndProcHook(treeView);

            _isEnhancedTextBox = (_searchInputControl.GetType() == typeof(TextBoxEx));
            if (_isEnhancedTextBox)
            {
                _enhancedTextBox = _searchInputControl as TextBoxEx;
            }

            this._searchInputControl.KeyDown
                += (sender, args) => HandleSearchInputKeyDown(args);

            this._searchInputControl.KeyUp
                += (sender, args) => HandleSearchInputKeyUp(args);

            this._treeViewControl.KeyDown
                += (sender, args) => HandleTreeViewKeyDown(args);
        }
    }
}
