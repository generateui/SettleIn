using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using SettleIn.UI.Game;
using System.Windows.Controls;
using System.Windows.Media;

namespace SettleIn.UI.Converters
{
    public class OfferStatusConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OfferStatus status = (OfferStatus)value;
            switch (status)
            {
                case OfferStatus.Accepted:
                    return (ImageSource)Core.Instance.Icons["IconAcceptOffer48"];
                case OfferStatus.CounterOffer:
                    return (ImageSource)Core.Instance.Icons["IconCounterOffer48"];
                case OfferStatus.Declined:
                    return (ImageSource)Core.Instance.Icons["IconRejectOffer48"];
                case OfferStatus.NoAnswer:
                    return (ImageSource)Core.Instance.Icons["IconWaitAnswer48"];
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
