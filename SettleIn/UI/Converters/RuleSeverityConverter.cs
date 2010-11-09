using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace SettleIn.UI
{
    public class RuleSeverityConverter :IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            RuleSeverity sev = (RuleSeverity) value;
            switch (sev)
            {
                case RuleSeverity.Error: return (ImageSource) Core.Instance.Icons["IconError16"];
                case RuleSeverity.Warning: return (ImageSource) Core.Instance.Icons["IconWarning16"];
                case RuleSeverity.Information: return (ImageSource) Core.Instance.Icons["IconInformation16"];
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
