using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    [DataContract]
    public enum ESideDirection
    {
        //           
        //   slopedown  ^  slopeup
        //             /  \  
        //     updown |    | updown
        //     updown |    | updown
        //             \  /   
        //   slopedown   +  slopeup  
        //          

        // |
        [EnumMember(Value = "UpDown")]
        UpDown,
        // \
        [EnumMember(Value = "SlopeUp")]
        SlopeUp,
        // /
        [EnumMember(Value = "SlopeDown")]
        SlopeDown
    }
}
