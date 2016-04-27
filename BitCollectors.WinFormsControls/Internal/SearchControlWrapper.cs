using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitCollectors.WinFormsControls.Internal
{
    internal class SearchControlWrapper
    {
        private TextBoxEx _searchInputControl;
        private TreeViewEx _treeViewControl;

        private void AssignSearchControlWndProcHook(TreeViewEx treeView)
        {
            //var action = new Action(() =>
            //{
            //    var searchHook = new SearchControlNativeWindowHooks { TreeViewControl = treeView };
            //    searchHook.AssignHandle(_searchInputControl.Handle);
            //});

            //_searchInputControl.HandleCreated += (sender, args) => action();

            //if (_searchInputControl.IsHandleCreated)
            //{
            //    action();
            //}
        }

        private void HandleSearchInputKeyDown(KeyEventArgs keyArguments)
        {
            switch (keyArguments.KeyCode)
            {
                case Keys.Down:
                    keyArguments.Handled = true;
                    keyArguments.SuppressKeyPress = true;
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
                        var textBox = _searchInputControl as TextBoxEx;
                        if (textBox != null && textBox.EscapeKeyClearsInput)
                        {
                            textBox.Text = string.Empty;
                            keyArguments.Handled = true;
                        }
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

                    case Keys.Up:
                        if (_treeViewControl.SelectedNode?.Level == 0 && _treeViewControl.SelectedNode?.Index == 0)
                        {
                            _enhancedTextBox.Focus();
                            _enhancedTextBox.SelectAll();
                            keyArguments.Handled = true;
                        }
                        break;
                }
            }
        }

        private char GetKeyCharWithProperCasing(bool shiftPressed, char keyCode)
        {
            var capsLocked = Control.IsKeyLocked(Keys.CapsLock);
            return (shiftPressed || capsLocked) && shiftPressed != capsLocked
                ? char.ToUpper(keyCode)
                : char.ToLower(keyCode);
        }

        private bool _isEnhancedTextBox = false;
        private TextBoxEx _enhancedTextBox = null;

        public void WireUpAttachedSearchControl(TextBoxEx control, TreeViewEx treeView)
        {
            this._searchInputControl = control;
            this._treeViewControl = treeView;
            this._treeViewControl.TabStop = false;

            AssignSearchControlWndProcHook(treeView);

            _enhancedTextBox = _searchInputControl as TextBoxEx;
            _isEnhancedTextBox = (_enhancedTextBox != null);

            this._searchInputControl.TextChanged
                += (sender, args) => HandleSearchInputTextChanged(args);

            this._searchInputControl.KeyDown
                += (sender, args) => HandleSearchInputKeyDown(args);

            this._searchInputControl.KeyUp
                += (sender, args) => HandleSearchInputKeyUp(args);

            this._treeViewControl.KeyDown
                += (sender, args) => HandleTreeViewKeyDown(args);

            this._treeViewControl.KeyUp
                += (sender, args) => HandleTreeViewKeyUp(args);
        }

        private void HandleSearchInputTextChanged(EventArgs args)
        {
            _treeViewControl?.Filter(_searchInputControl.Text);
        }

        private void HandleTreeViewKeyUp(KeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keys.Escape:
                    _treeViewControl.Filter(_searchInputControl.Text);
                    break;
            }
        }
    }
}
