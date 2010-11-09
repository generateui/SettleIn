using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    [DataContract]
    public enum EResource : int
    {
        [EnumMember(Value = "Wheat")]
        Wheat,
        [EnumMember(Value = "Clay")]
        Clay,
        [EnumMember(Value = "Ore")]
        Ore,
        [EnumMember(Value = "Timber")]
        Timber,
        [EnumMember(Value = "Sheep")]
        Sheep,
        [EnumMember(Value = "Discovery")]
        Discovery,
        [EnumMember(Value = "Gold")]
        Gold,
        [EnumMember(Value = "Volcano")]
        Volcano
    }
}
