using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace SettleIn.UI.Converters
{
    public class OpacityColoringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color c = ColorFromString((string)value);
            return new SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color ColorFromString(string val)
        {
            val = val.Replace("#", "");

            byte a = 64;
            byte pos = 0;
            if (val.Length == 8)
            {
                a = System.Convert.ToByte(val.Substring(pos, 2), 16);
                pos = 2;
            }

            byte r = System.Convert.ToByte(val.Substring(pos, 2), 16);
            pos += 2;

            byte g = System.Convert.ToByte(val.Substring(pos, 2), 16);
            pos += 2;

            byte b = System.Convert.ToByte(val.Substring(pos, 2), 16);

            return Color.FromArgb(a, r, g, b);

        }

        #endregion
    }
}
