using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    [DataContract]
    public enum EChitNumber
    {
        [EnumMember(Value = "2")]
        N2 = 2,
        [EnumMember(Value = "3")]
        N3 = 3,
        [EnumMember(Value = "4")]
        N4 = 4,
        [EnumMember(Value = "5")]
        N5 = 5,
        [EnumMember(Value = "6")]
        N6 = 6,
        [EnumMember(Value = "8")]
        N8 = 8,
        [EnumMember(Value = "9")]
        N9 = 9,
        [EnumMember(Value = "10")]
        N10 = 10,
        [EnumMember(Value = "11")]
        N11 = 11,
        [EnumMember(Value = "12")]
        N12 = 12,
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Random")]
        Random
    }
}
