using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace SettleIn.UI
{
    public class RandomTopMessageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BoardVisual b = value as BoardVisual;
            return null;
            throw new NotImplementedException();
            //return b.RandomHexes.CountAll + " random hexes on the board. Choose an equal amount of hexes and chits. These hexes and chits are to be randomly assigned when the board is loaded at the start of the game.";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
