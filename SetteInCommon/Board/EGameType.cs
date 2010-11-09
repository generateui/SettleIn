using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    [DataContract]
    public enum EGameType
    {
        [EnumMember(Value = "Standard")]
        Standard,
        [EnumMember(Value = "SeaFarers")]
        SeaFarers,
        [EnumMember(Value = "CitiesKnights")]
        CitiesKnights
    }
}
