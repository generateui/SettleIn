using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    [DataContract]
    public enum EPointPositionOnHex
    {
        //          topmiddle,
        //              ^
        //    LeftTop  /  \  RightTop
        //            |    |
        //            |    |
        // LeftBottom  \  /  RightBottom
        //               +    
        //         BottomMiddle
        [EnumMember(Value = "TopMiddle")]
        TopMiddle,
        [EnumMember(Value = "RightTop")]
        RightTop,
        [EnumMember(Value = "RightBottom")]
        RightBottom,
        [EnumMember(Value = "BottomMiddle")]
        BottomMiddle,
        [EnumMember(Value = "LeftBottom")]
        LeftBottom,
        [EnumMember(Value = "LeftTop")]
        LeftTop
    };
}
