using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CooridnateGrid.ConverterClasses
{
    public class Vector3ToStringConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{((Vector3)value).X} | {((Vector3)value).Y} | {((Vector3)value).Z}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = ((string)value).Split(new char[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            float x = 0;
            float y = 0;
            float z = 0;
            if (str.Length > 0)
                float.TryParse(str[0], out x);
            if (str.Length > 1)
                float.TryParse(str[1], out y);
            if (str.Length > 2)
                float.TryParse(str[2], out z);
            return new Vector3(x, y, z);

        }
    }
}
