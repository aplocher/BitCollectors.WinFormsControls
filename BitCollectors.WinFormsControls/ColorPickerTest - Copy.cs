using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BitCollectors.WinFormsControls
{
    [DefaultProperty("Color"), DefaultEvent("ColorChanged")]
    public class ColorPicker : Control
    {
        private const string DefaultColorName = "Black";

        #region Fields
        private CheckBox _internalCheckbox;
        private bool _textDisplayed = true;
        private readonly EditorService _editorService;
        #endregion

        public ColorPicker(Color c)
        {
            CheckBox = new CheckBox();
            CheckBox.Appearance = Appearance.Button;
            CheckBox.Dock = DockStyle.Fill;
            CheckBox.TextAlign = ContentAlignment.MiddleCenter;
            SetColor(c);
            Controls.Add(CheckBox);
            _editorService = new EditorService(this);
        }

        public ColorPicker()
            : this(Color.FromName(DefaultColorName))
        {
        }

        #region Properties

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Description("The currently selected color."), Category("Appearance"),
         DefaultValue(typeof(Color), DefaultColorName)]
        public Color Color
        {
            get { return CheckBox.BackColor; }
            set
            {
                SetColor(value);
                if (ColorChanged != null)
                {
                    ColorChanged(this, EventArgs.Empty);
                }
            }
        }

        // No need to display ForeColor and BackColor and Text in the property browser:
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Description("True meanse the control displays the currently selected color's name, False otherwise."),
         Category("Appearance"), DefaultValue(true)]
        public bool TextDisplayed
        {
            get { return _textDisplayed; }
            set
            {
                _textDisplayed = value;
                SetColor(Color);
            }
        }

        private CheckBox CheckBox
        {
            get { return _internalCheckbox; }
            set
            {
                if (_internalCheckbox != null)
                    _internalCheckbox.CheckStateChanged -= OnCheckStateChanged;

                _internalCheckbox = value;

                if (_internalCheckbox != null)
                    _internalCheckbox.CheckStateChanged += OnCheckStateChanged;
            }
        }

        #endregion

        public event EventHandler ColorChanged;

        private void CloseDropDown()
        {
            _editorService.CloseDropDown();
        }

        // Primitive color inversion.
        private Color GetInvertedColor(Color c)
        {
            if ((Convert.ToInt32(c.R) + Convert.ToInt32(c.G) + Convert.ToInt32(c.B)) > ((255 * 3) / 2))
            {
                return Color.Black;
            }
            return Color.White;
        }

        // If the associated CheckBox is checked, the drop-down UI is displayed.
        // Otherwise it is closed.
        private void OnCheckStateChanged(object sender, EventArgs e)
        {
            if (CheckBox.CheckState == CheckState.Checked)
            {
                ShowDropDown();
            }
            else
            {
                CloseDropDown();
            }
        }

        private void SetColor(Color c)
        {
            CheckBox.BackColor = c;
            CheckBox.ForeColor = GetInvertedColor(c);
            CheckBox.Text = _textDisplayed ? c.Name : string.Empty;
        }

        private void ShowDropDown()
        {
            try
            {
                // This is the Color type editor - it displays the drop-down UI calling
                // our IWindowsFormsEditorService implementation.
                var Editor = new ColorEditor();
                // Display the UI.
                var C = Color;
                var NewValue = Editor.EditValue(_editorService, C);
                // If the user didn't cancel the selection, remember the new color.
                if (((NewValue != null)) && (!_editorService.Canceled))
                {
                    Color = (Color)NewValue;
                }
                // Finally, "pop-up" the associated CheckBox.
                CheckBox.CheckState = CheckState.Unchecked;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        // This is a simple Form descendant that hosts the drop-down control provided
        // by the ColorEditor class (in the call to DropDownControl).
        private class DropDownForm : Form
        {
            #region Fields

            // did the user cancel the color selection?
            // was the form closed by calling the CloseDropDown method?
            private bool _CloseDropDownCalled;

            #endregion

            public DropDownForm()
            {
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                KeyPreview = true;
                // to catch the ESC key
                StartPosition = FormStartPosition.Manual;
                // The ColorUI control is hosted by a Panel, which provides the simple border frame
                // not available for Forms.
                var P = new Panel();
                P.BorderStyle = BorderStyle.FixedSingle;
                P.Dock = DockStyle.Fill;
                Controls.Add(P);
            }

            #region Properties

            public bool Canceled { get; private set; }

            #endregion

            #region Overrides

            protected override void OnDeactivate(EventArgs e)
            {
                // We set the Owner to Nothing BEFORE calling the base class.
                // If we wouldn't do it, the Picker form would lose focus after 
                // the dropdown is closed.
                Owner = null;
                base.OnDeactivate(e);
                // If the form was closed by any other means as the CloseDropDown call, it is because
                // the user clicked outside the form, or pressed the ESC key.
                if (!_CloseDropDownCalled)
                {
                    Canceled = true;
                }
                Hide();
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                base.OnKeyDown(e);
                if ((e.Modifiers == 0) && (e.KeyCode == Keys.Escape))
                {
                    Hide();
                }
            }

            #endregion

            public void CloseDropDown()
            {
                _CloseDropDownCalled = true;
                Hide();
            }

            public void SetControl(Control ctl)
            {
                ((Panel)Controls[0]).Controls.Add(ctl);
            }
        }

        // This class actually hosts the ColorEditor.ColorUI by implementing the 
        // IWindowsFormsEditorService.
        private class EditorService : IWindowsFormsEditorService, IServiceProvider
        {
            #region Fields

            // Reference to the drop down, which hosts the ColorUI control.
            private DropDownForm _DropDownHolder;
            // The associated color picker control.
            private readonly ColorPicker _Picker;

            #endregion

            // Cached _DropDownHolder.Canceled flag in order to allow it to be inspected
            // by the owning ColorPicker control.

            public EditorService(ColorPicker owner)
            {
                _Picker = owner;
            }

            #region Properties

            public bool Canceled { get; private set; }

            #endregion

            #region

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IWindowsFormsEditorService))
                {
                    return this;
                }
                return null;
            }

            public void CloseDropDown()
            {
                if ((_DropDownHolder != null))
                {
                    _DropDownHolder.CloseDropDown();
                }
            }

            public void DropDownControl(Control control)
            {
                Canceled = false;
                // Initialize the hosting form for the control.
                _DropDownHolder = new DropDownForm();
                _DropDownHolder.Bounds = control.Bounds;
                _DropDownHolder.SetControl(control);
                // Lookup a parent form for the Picker control and make the dropdown form to be owned
                // by it. This prevents to show the dropdown form icon when the user presses the At+Tab system 
                // key while the dropdown form is displayed.
                var PickerForm = GetParentForm(_Picker);
                if (((PickerForm != null)) && (PickerForm is Form))
                {
                    _DropDownHolder.Owner = (Form)PickerForm;
                }
                // Ensure the whole drop-down UI is displayed on the screen and show it.
                PositionDropDownHolder();
                _DropDownHolder.Show();
                // ShowDialog would disable clicking outside the dropdown area!
                // Wait for the user to select a new color (in which case the ColorUI calls our CloseDropDown
                // method) or cancel the selection (no CloseDropDown called, the Cancel flag is set to True).
                DoModalLoop();
                // Remember the cancel flag and get rid of the drop down form.
                Canceled = _DropDownHolder.Canceled;
                _DropDownHolder.Dispose();
                _DropDownHolder = null;
            }

            public DialogResult ShowDialog(Form dialog)
            {
                throw new NotSupportedException();
            }

            #endregion

            private void DoModalLoop()
            {
                Debug.Assert((_DropDownHolder != null));
                while (_DropDownHolder.Visible)
                {
                    Application.DoEvents();
                    // The sollowing is the undocumented User32 call. For more information
                    // see the accompanying article at http://www.vbinfozine.com/a_colorpicker.shtml
                    MsgWaitForMultipleObjects(1, IntPtr.Zero, 1, 5, 255);
                }
            }

            private Control GetParentForm(Control ctl)
            {
                do
                {
                    if (ctl.Parent == null)
                    {
                        return ctl;
                    }
                    ctl = ctl.Parent;
                } while (true);
            }

            [DllImport("User32", SetLastError = true)]
            private static extern Int32 MsgWaitForMultipleObjects(Int32 nCount, IntPtr pHandles, Int16 bWaitAll,
                Int32 dwMilliseconds, Int32 dwWakeMask);

            private void PositionDropDownHolder()
            {
                var loc = _Picker.Parent.PointToScreen(_Picker.Location);
                var screenRect = Screen.PrimaryScreen.WorkingArea;

                if (loc.X < screenRect.X)
                {
                    loc.X = screenRect.X;
                }
                else if ((loc.X + _DropDownHolder.Width) > screenRect.Right)
                {
                    loc.X = screenRect.Right - _DropDownHolder.Width;
                }
                // Do the same for the Y coordinate.
                if ((loc.Y + _Picker.Height + _DropDownHolder.Height) > screenRect.Bottom)
                {
                    loc.Offset(0, -_DropDownHolder.Height);
                    // dropdown will be above the picker control
                }
                else
                {
                    loc.Offset(0, _Picker.Height);
                    // dropdown will be below the picker
                }
                _DropDownHolder.Location = loc;
            }
        }
    }
}