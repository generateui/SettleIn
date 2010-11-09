using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon
{
    [DataContract]
    public enum EChatItemType
    {
        [EnumMember(Value = "System")]
        System,
        [EnumMember(Value = "Server")]
        Server,
        [EnumMember(Value = "HumanChat")]
        HumanChat,
        [EnumMember(Value = "Lobby")]
        Lobby
    }
}
