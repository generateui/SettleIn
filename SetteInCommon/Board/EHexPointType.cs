using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    [DataContract]
    public enum EHexPointType
    {
        // the point has 2 hexes on the highest row (1 on lowest)
        [EnumMember(Value="UpperRow2")]
        UpperRow2,
        // the point has 1 hex on the highest row (2 on lowest)
        [EnumMember(Value = "UpperRow1")]
        UpperRow1
    }
}
