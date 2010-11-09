using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
    
namespace SettleInCommon.Board
{
    [DataContract]
    public class Chit
    {
        [DataMember]
        public EChitNumber ChitNumber { get; set; }

        public override string ToString()
        {
            return ChitNumber.ToString();
        }

        public static EChitNumber GetChitNumber(int num)
        {
            switch (num)
            {
                case 2: return EChitNumber.N2;
                case 3: return EChitNumber.N3;
                case 4: return EChitNumber.N4;
                case 5: return EChitNumber.N5;
                case 6: return EChitNumber.N6;
                case 8: return EChitNumber.N8;
                case 9: return EChitNumber.N9;
                case 10: return EChitNumber.N10;
                case 11: return EChitNumber.N11;
                case 12: return EChitNumber.N12;
            }
            return 0;
        }
    }
}
