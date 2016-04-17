using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace BitCollectors.WinFormsControls.Common
{
    [Editor(typeof(BorderUiTypeEditor), typeof(UITypeEditor))]
    public class Border
    {
        public Color Color
        {
            get;
            set;
        }

        public int Size
        {
            get;
            set;
        }

        public bool Equals(Border other)
        {
            return other != null && (Color.Equals(other.Color) && Size == other.Size);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Color.GetHashCode() * 397) ^ Size;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Border && Equals((Border)obj);
        }

        public override string ToString()
        {
            return $"{this.Size}px, {this.Color.ToString()}";
        }
    }
}
