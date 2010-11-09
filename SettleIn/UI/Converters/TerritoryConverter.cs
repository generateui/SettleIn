using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

using SettleInCommon.Board;

namespace SettleIn.UI
{
    public class TerritoryConverter:IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return (ImageSource)Core.Instance.Icons["IconMainland48"];
            else
                return (ImageSource)Core.Instance.Icons["IconIsland48"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
