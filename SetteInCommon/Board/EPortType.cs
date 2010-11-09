using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    /// <summary>
    /// Type of the port for the seafarers.
    /// </summary>
    [DataContract]
    public enum EPortType
    {
        [EnumMember(Value = "Clay")]
        Clay,
        [EnumMember(Value = "Wheat")]
        Wheat,
        [EnumMember(Value = "Timber")]
        Timber,
        [EnumMember(Value = "Ore")]
        Ore,
        [EnumMember(Value = "Sheep")]
        Sheep,
        [EnumMember(Value = "ThreeToOne")]
        ThreeToOne,
        [EnumMember(Value = "Random")]
        Random,
        [EnumMember(Value = "None")]
        None
    };
}
