using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettleInCommon.Extensions
{
    public static class ExtensionMethods
    {
        public static decimal PercentageOf(this double number, int percent)
        {
            return (decimal)(number * percent / 100);
        }

        public static decimal PercentageOf(this double number, float percent)
        {
            return (decimal)(number * percent / 100);
        }

        public static decimal PercentageOf(this double number, double percent)
        {
            return (decimal)(number * percent / 100);
        }

        public static decimal PercentageOf(this double number, long percent)
        {
            return (decimal)(number * percent / 100);
        }

        public static decimal PercentOf(this double position, int total)
        {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }

        public static decimal PercentOf(this double position, float total)
        {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }

        public static decimal PercentOf(this double position, double total)
        {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }

        public static decimal PercentOf(this double position, long total)
        {
            decimal result = 0;
            if (position > 0 && total > 0)
                result = (decimal)((decimal)position / (decimal)total * 100);
            return result;
        }
    }
}
