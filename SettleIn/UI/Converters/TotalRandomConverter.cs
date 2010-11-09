using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace SettleIn.UI
{
    public class TotalRandomConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
            /*
            if (b.XmlBoard.Hexes.RandomHexCount == b.XmlBoard.RandomHexes.CountAll)
                return (ImageSource)Core.Instance.Textures["IconYes"];
            else
                return (ImageSource)Core.Instance.Textures["IconNo"];
             */
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
