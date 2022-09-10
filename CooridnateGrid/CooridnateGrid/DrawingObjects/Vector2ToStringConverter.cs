using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CooridnateGrid.DrawingObjects
{
    public class Vector2ToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{((Vector2)value).X} {((Vector2)value).Y}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = ((string)value).Split(' ');
            if(str.Length != 2) return DependencyProperty.UnsetValue;
            float x;
            float y;
            if(!float.TryParse(str[0], out x) || !float.TryParse(str[1],out y))
            {
                return DependencyProperty.UnsetValue;
            }
            return new Vector2(x,y);
        }
    }
}
