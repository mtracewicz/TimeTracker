using System;
using System.Windows.Media;

namespace GUI
{
    class Utils
    {
        public static Brush GetBrushFromHex(String hexColor)
        {
            return (Brush)new BrushConverter().ConvertFromString(hexColor);
        }

    }
}
