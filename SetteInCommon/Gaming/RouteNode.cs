using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Board;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming
{
    [DataContract]
    public class RouteNode : HexSide
    {
        private int _PlayerID = 0;
        private bool _IsRoad = true;

        [DataMember]
        public bool IsShip
        {
            get { return !_IsRoad; }
            set { _IsRoad= !value; }
        }

        [DataMember]
        public int PlayerID
        {
            get { return _PlayerID; }
            set { _PlayerID = value; }
        }

        [DataMember]
        public bool IsRoad
        {
            get { return _IsRoad; }
            set { _IsRoad = value; }
        }
        public RouteNode(HexLocation loc1, HexLocation loc2)
            : base(loc1, loc2)
        {
        }

        public RouteNode(HexSide side)
            : base(side.Hex1, side.Hex2)
        {
 
        }
        public RouteNode(HexPoint point1, HexPoint point2)
            : base(point1, point2)
        {
        }

        public override int GetHashCode()
        {
            return Hex1.W ^ Hex1.H ^ Hex2.W ^ Hex2.H;
        }

    }
}
