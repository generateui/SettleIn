using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using SettleInCommon.Board.Hexes;

namespace SettleInCommon.Board
{
    /// <summary>
    /// Intersection of three hexes
    /// </summary>
    [DataContract]
    public class HexPoint : IEquatable<HexPoint>, IEqualityComparer<HexPoint>
    {
        private HexLocation _Hex1;
        private HexLocation _Hex2;
        private HexLocation _Hex3;

        [DataMember]
        public HexLocation Hex1
        {
            get { return _Hex1; }
            set { _Hex1 = value; }
        }

        [DataMember]
        public HexLocation Hex2
        {
            get { return _Hex2; }
            set { _Hex2 = value; }
        }

        [DataMember]
        public HexLocation Hex3
        {
            get { return _Hex3; }
            set { _Hex3 = value; }
        }

        public EPointPositionOnHex HexPositionOnTopLeftMost
        {
            get
            {
                return Type == EHexPointType.UpperRow1 ?
                    EPointPositionOnHex.BottomMiddle : EPointPositionOnHex.RightBottom;
            }
        }

        public List<HexSide> GetOtherSides(HexSide side)
        {
            List<HexSide> result = new List<HexSide>();

            foreach (HexSide s in GetNeighbourSides)
                if (!side.Equals(s))
                    result.Add(s);

            return result;
        }

        public List<HexPoint> GetOtherNeighbours(HexPoint center, HexPoint ignore)
        {
            List<HexPoint> result = GetNeighbours();
            
            result.Remove(ignore);

            return result;
        }
        public HexLocation TopMost
        {
            get
            {
                List<HexLocation> points = new List<HexLocation>();
                points.Add(Hex3);
                points.Add(Hex2);
                points.Add(Hex1);
                int w = 220;
                int h = 220;
                foreach (HexLocation point in points)
                {
                    if (point.W < w) w = point.W;
                    if (point.H < h) h = point.H;
                }
                List<HexLocation> res = new List<HexLocation>();
                if (Hex1.H == h) res.Add(Hex1);
                if (Hex2.H == h) res.Add(Hex2);
                if (Hex3.H == h) res.Add(Hex3);
                if (res.Count == 1)
                {
                    return res[0];
                }
                else
                {
                    if (res.Count == 2)
                    {
                        HexLocation l = res[0];
                        if (l.W < res[1].W) return l;
                        else return res[1];
                    }
                }
                return null;
            }
        }

        public EHexPointType Type
        {
            get
            {
                List<HexLocation> points = new List<HexLocation>();
                points.Add(Hex3);
                points.Add(Hex2);
                points.Add(Hex1);

                int h = 220;

                foreach (HexLocation point in points)
                    if (point.H < h) h = point.H;

                if ((from p in points
                     where p.H == h
                     select p).Count() == 1)
                    return EHexPointType.UpperRow1;
                else
                    return EHexPointType.UpperRow2;
            }
        }
        public override string ToString()
        {
            return String.Format("hex1: {0}, hex2: {1}, hex3: {2}",
                Hex1.ToString(), Hex2.ToString(), Hex3.ToString());
        }

        public string ToString(XmlBoard board)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(board.Hexes[Hex1].ToString());
            sb.Append(", ");
            sb.Append(board.Hexes[Hex2].ToString());
            sb.Append(", ");
            sb.Append(board.Hexes[Hex3].ToString());

            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            HexPoint point = obj as HexPoint;
            if (point != null)
            {
                return IsEqual(point);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Hex1.GetHashCode() ^ Hex2.GetHashCode() ^ Hex3.GetHashCode();
        }
        public HexPoint(HexLocation hex1, HexLocation hex2, HexLocation hex3)
        {
            _Hex1 = hex1;
            _Hex2 = hex2;
            _Hex3 = hex3;
            if (_Hex1.Equals(_Hex2) ||
                _Hex1.Equals(_Hex3) ||
                _Hex2.Equals(_Hex3))
                throw new Exception("WHOA");

        }

        public HexPoint(HexLocation hex, EPointPositionOnHex relativePosition)
        {
            // we must assume hex comes from a uneven row, and 
            // relative position on the hex is never the two left positions
            if (hex.H % 2 == 0) throw new Exception("WHooa!");
            Hex1 = hex;

            switch (relativePosition)
            {
                case EPointPositionOnHex.TopMiddle:
                    Hex2 = new HexLocation(hex.W - 1, hex.H - 1);
                    Hex3 = new HexLocation(hex.W, hex.H - 1);
                    break;
                case EPointPositionOnHex.RightTop:
                    Hex2 = new HexLocation(hex.W, hex.H - 1);
                    Hex3 = new HexLocation(hex.W + 1, hex.H);
                    break;
                case EPointPositionOnHex.RightBottom:
                    Hex2 = new HexLocation(hex.W + 1, hex.H);
                    Hex3 = new HexLocation(hex.W, hex.H + 1);
                    break;
                case EPointPositionOnHex.BottomMiddle:
                    Hex2 = new HexLocation(hex.W, hex.H + 1);
                    Hex3 = new HexLocation(hex.W - 1, hex.H + 1);
                    break;
                default: throw new Exception("Whoa!");
            }
        }

        public HexPoint() { }

        // create a point out of two neighbouring sides
        public HexPoint(HexSide side1, HexSide side2)
        {
            List<HexLocation> allLocations = new List<HexLocation>();
            allLocations.Add(side1.Hex1);
            allLocations.Add(side1.Hex2);
            allLocations.Add(side2.Hex1);
            allLocations.Add(side2.Hex2);

            HexLocation equalHex = null;
            if (side1.Hex1.Equals(side2.Hex1)) equalHex = side1.Hex1;
            if (side1.Hex1.Equals(side2.Hex2)) equalHex = side1.Hex1;

            allLocations.Remove(equalHex);

            this._Hex1 = allLocations[0];
            this._Hex2 = allLocations[1];
            this._Hex3 = allLocations[2];
        }

        public List<HexPoint> GetNeighbours()
        {
            List<HexPoint> result = new List<HexPoint>();
            HexLocation topmost = this.TopMost;
            if (topmost == null)
            {
            }
            if (topmost.H % 2 == 0)
            {
                //even rows
                if (Type == EHexPointType.UpperRow1)
                {
                    HexPoint p1 = new HexPoint(
                        topmost,
                        new HexLocation(topmost.W - 1, topmost.H),
                        new HexLocation(topmost.W, topmost.H + 1));
                    result.Add(p1);

                    HexPoint p2 = new HexPoint();
                    p2.Hex1 = new HexLocation(topmost.W + 1, topmost.H + 1);
                    p2.Hex2 = new HexLocation(topmost.W, topmost.H + 1);
                    p2.Hex3 = new HexLocation(topmost.W, topmost.H + 2);
                    result.Add(p2);

                    HexPoint p3 = new HexPoint();
                    p3.Hex1 = topmost;
                    p3.Hex2 = new HexLocation(topmost.W + 1, topmost.H + 1);
                    p3.Hex3 = new HexLocation(topmost.W + 1, topmost.H);
                    result.Add(p3);
                }
                else
                {
                    HexPoint p1 = new HexPoint(
                        topmost,
                        new HexLocation(topmost.W + 1, topmost.H),
                        new HexLocation(topmost.W + 1, topmost.H - 1));
                    result.Add(p1);

                    HexPoint p2 = new HexPoint();
                    p2.Hex1 = new HexLocation(topmost.W + 2, topmost.H + 1);
                    p2.Hex2 = new HexLocation(topmost.W + 1, topmost.H + 1);
                    p2.Hex3 = new HexLocation(topmost.W + 1, topmost.H);
                    result.Add(p2);

                    HexPoint p3 = new HexPoint();
                    p3.Hex1 = topmost;
                    p3.Hex2 = new HexLocation(topmost.W + 1, topmost.H + 1);
                    p3.Hex3 = new HexLocation(topmost.W, topmost.H + 1);
                    result.Add(p3);
                }
            }
            else
            {
                //uneven rows
                if (Type == EHexPointType.UpperRow1)
                {
                    HexPoint p1 = new HexPoint();
                    p1.Hex1 = topmost;
                    p1.Hex2 = new HexLocation(topmost.W - 1, topmost.H + 1);
                    p1.Hex3 = new HexLocation(topmost.W - 1, topmost.H);
                    result.Add(p1);

                    HexPoint p2 = new HexPoint();
                    p2.Hex1 = new HexLocation(topmost.W, topmost.H + 1);
                    p2.Hex2 = new HexLocation(topmost.W - 1, topmost.H + 1);
                    p2.Hex3 = new HexLocation(topmost.W, topmost.H + 2);
                    result.Add(p2);

                    HexPoint p3 = new HexPoint();
                    p3.Hex1 = topmost;
                    p3.Hex2 = new HexLocation(topmost.W, topmost.H + 1);
                    p3.Hex3 = new HexLocation(topmost.W + 1, topmost.H);
                    result.Add(p3);
                }
                else
                {
                    // OK
                    HexPoint p1 = new HexPoint();
                    p1.Hex1 = topmost;
                    p1.Hex2 = new HexLocation(topmost.W, topmost.H - 1);
                    p1.Hex3 = new HexLocation(topmost.W + 1, topmost.H);
                    result.Add(p1);

                    HexPoint p2 = new HexPoint();
                    p2.Hex1 = new HexLocation(topmost.W + 1, topmost.H);
                    p2.Hex2 = new HexLocation(topmost.W + 1, topmost.H + 1);
                    p2.Hex3 = new HexLocation(topmost.W, topmost.H + 1);
                    result.Add(p2);

                    HexPoint p3 = new HexPoint();
                    p3.Hex1 = topmost;
                    p3.Hex2 = new HexLocation(topmost.W, topmost.H + 1);
                    p3.Hex3 = new HexLocation(topmost.W - 1, topmost.H + 1);
                    result.Add(p3);
                }
            }
            return result;
        }
        private bool IsEqual(HexPoint point)
        {
            List<HexLocation> points = new List<HexLocation>();
            points.Add(Hex3);
            points.Add(Hex2);
            points.Add(Hex1);

            return (points.Contains(point.Hex1) && points.Contains(point.Hex2) && points.Contains(point.Hex3));
            /*
            if (points.Contains(point.Hex1))
            {
                points.Remove((from p in points where p.Equals(point.Hex1) select p).Single());
                if (points.Contains(point.Hex2))
                {
                    points.Remove((from p in points where p.Equals(point.Hex2) select p).Single());
                    if (points.Contains(point.Hex3))
                    {
                        return true;
                    }
                }
            }
            return false;
             */
        }
        public List<HexSide> GetNeighbourSides
        {
            get
            {

                List<HexSide> result = new List<HexSide>();

                // add all three hex sides around point
                result.Add(new HexSide(Hex1, Hex2));
                result.Add(new HexSide(Hex1, Hex3));
                result.Add(new HexSide(Hex2, Hex3));

                return result;
            }
        }

        public bool HasLocation(HexLocation location)
        {
            return Hex1.Equals(location) ||
                Hex2.Equals(location) ||
                Hex3.Equals(location);
        }

        public bool IsValid()
        {
            if (Hex1.Equals(Hex2) ||
                Hex1.Equals(Hex3) ||
                Hex2.Equals(Hex3))
                return false;

            //if (!Hex1.IsValid() ||
            return true;
        }


        #region IEquatable<XmlHexPoint> Members

        public bool Equals(HexPoint point)
        {
            return IsEqual(point);
        }

        #endregion

        public IEnumerable<HexSide> GetSeaSides(XmlBoard board)
        {
            return from side in GetNeighbourSides
                   where board.Hexes[side.Hex1] is SeaHex ||
                         board.Hexes[side.Hex2] is SeaHex
                   select side;
        }

        #region IEqualityComparer<HexPoint> Members

        public bool Equals(HexPoint x, HexPoint y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(HexPoint obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
