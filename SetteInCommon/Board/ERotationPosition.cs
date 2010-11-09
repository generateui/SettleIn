using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    [DataContract]
    public enum ERotationPosition : int
    {
        [EnumMember(Value = "60")]
        Deg60 = 60,
        [EnumMember(Value = "120")]
        Deg120 = 120,
        [EnumMember(Value = "180")]
        Deg180 = 180,
        [EnumMember(Value = "240")]
        Deg240 = 240,
        [EnumMember(Value = "300")]
        Deg300 = 300,
        [EnumMember(Value = "0")]
        Deg0 = 0,
    }
}
