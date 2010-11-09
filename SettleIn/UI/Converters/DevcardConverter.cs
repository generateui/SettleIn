using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SettleInCommon.Gaming.DevCards;
using System.Windows.Media;

namespace SettleIn.UI.Converters
{
    public class DevcardConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DevelopmentCard devCard = value as DevelopmentCard;
            if (devCard != null)
            {
                if (devCard is Monopoly) return (ImageSource)Core.Instance.Icons["IconMono48"];
                if (devCard is YearOfPlenty) return (ImageSource)Core.Instance.Icons["IconYop48"];
                if (devCard is VictoryPoint) return (ImageSource)Core.Instance.Icons["IconVp48"];
                if (devCard is RoadBuilding) return (ImageSource)Core.Instance.Icons["IconRb48"];
                if (devCard is Soldier) return (ImageSource)Core.Instance.Icons["IconRobber48"];
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
