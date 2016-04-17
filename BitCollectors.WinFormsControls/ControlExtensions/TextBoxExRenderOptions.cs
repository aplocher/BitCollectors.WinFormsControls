using BitCollectors.WinFormsControls.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BitCollectors.WinFormsControls.ControlExtensions
{
    [Serializable]
    [TypeConverter(typeof(TextBoxLayoutExtrasTypeConverter))]
    public class TextBoxLayoutExtras : INotifyPropertyChanged
    {
        private Border _border;
        private Border _mouseOverBorder;
        private Border _focusBorder;
        //[TypeConverter(typeof(ExpandableObjectConverter))]public static DependencyProperty Choice4Property = DependencyProperty.Register("Choice4", typeof(Choice), typeof(MyControl));

        [Category("Border")]
        [DefaultValue(typeof(Border), null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
        [ReadOnly(true)]
        public Border Border
        {
            get { return _border; }
            set
            {
                if (_border != value)
                {
                    _border = value;
                    OnPropertyChanged();
                }
            }
        }

        [Category("Border")]
        [DefaultValue(typeof(Border), null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
        public Border MouseOverBorder
        {
            get { return _mouseOverBorder; }
            set
            {
                if (_mouseOverBorder != value)
                {
                    _mouseOverBorder = value;
                    OnPropertyChanged();
                }
            }
        }

        [Category("Border")]
        [DefaultValue(typeof(Border), null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
        public Border FocusBorder
        {
            get { return _focusBorder; }
            set
            {
                if (_focusBorder != value)
                {
                    _focusBorder = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //public delegate void PropertyChangedEventHandler(string propertyname);

    public class TextBoxLayoutExtrasTypeConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context) { return true; }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(TextBoxLayoutExtras));
        }
    }
}
