using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SettleInCommon.Gaming.DevCards;
using System.Windows.Media;
using SettleInCommon.Board;

namespace SettleIn.UI.Converters
{
    public class PortConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            EPortType res = (EPortType)value;
            switch (res)
            {
                case EPortType.Timber: return (ImageSource)Core.Instance.Icons["IconTimberPort"];
                case EPortType.Wheat: return (ImageSource)Core.Instance.Icons["IconWheatPort"];
                case EPortType.Ore: return (ImageSource)Core.Instance.Icons["IconOrePort"];
                case EPortType.Clay: return (ImageSource)Core.Instance.Icons["IconClayPort"];
                case EPortType.Sheep: return (ImageSource)Core.Instance.Icons["IconSheepPort"];
                case EPortType.ThreeToOne: return (ImageSource)Core.Instance.Icons["Icon31Port"];
                case EPortType.None: throw new Exception();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
