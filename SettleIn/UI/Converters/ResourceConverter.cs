using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SettleInCommon.Board;
using System.Windows.Media;

namespace SettleIn.UI.Converters
{
    public class ResourceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            EResource resource = (EResource)value;
            switch (resource)
            {
                case EResource.Clay: return (ImageSource)Core.Instance.Icons["IconClayCard"];
                case EResource.Timber: return (ImageSource)Core.Instance.Icons["IconTimberCard"];
                case EResource.Wheat: return (ImageSource)Core.Instance.Icons["IconWheatCard"];
                case EResource.Ore: return (ImageSource)Core.Instance.Icons["IconOreCard"];
                case EResource.Sheep: return (ImageSource)Core.Instance.Icons["IconSheepCard"];
                case EResource.Discovery: return (ImageSource)Core.Instance.Icons["IconDiscoveryCard"];
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
