using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using SettleInCommon.Board;

namespace SettleIn.UI
{
    public class TerritoryIDToIconConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Territory t = value as Territory;
            if (t.IsMainland)
                return (ImageSource)Core.Instance.Textures["MainLandTexture"];
            else
                switch (t.ID)
                {
                    case 1: return (ImageSource)Core.Instance.Textures["IslandTexture1"];
                    case 2: return (ImageSource)Core.Instance.Textures["IslandTexture2"];
                    case 3: return (ImageSource)Core.Instance.Textures["IslandTexture3"];
                    case 4: return (ImageSource)Core.Instance.Textures["IslandTexture4"];
                    case 5: return (ImageSource)Core.Instance.Textures["IslandTexture5"];
                    case 6: return (ImageSource)Core.Instance.Textures["IslandTexture6"];
                    case 7: return (ImageSource)Core.Instance.Textures["IslandTexture6"];
                    case 8: return (ImageSource)Core.Instance.Textures["IslandTexture6"];
                    case 9: return (ImageSource)Core.Instance.Textures["IslandTexture6"];
                }
            throw new Exception("Should not reach this code");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
