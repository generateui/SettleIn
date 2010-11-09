using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace SettleIn.UI
{
    public class MaxTradePerTurnConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int max = (int)value;
            switch (max)
            {
                case 1: return (ImageSource)Core.Instance.Icons["IconMax1TradePerTurn48"];
                case 2: return (ImageSource)Core.Instance.Icons["IconMax2TradePerTurn48"];
                case 3: return (ImageSource)Core.Instance.Icons["IconMax3TradePerTurn48"];
                case 4: return (ImageSource)Core.Instance.Icons["IconMax4TradePerTurn48"];
                case 5: return (ImageSource)Core.Instance.Icons["IconMax5TradePerTurn48"];
            }
            return (ImageSource)Core.Instance.Icons["IconMaxUnlimitedTradePerTurn48"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
