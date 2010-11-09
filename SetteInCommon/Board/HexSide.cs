using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    /// <summary>
    /// Immutable duo of HexPoint locations
    /// </summary>
    /// TODO: Make truly immutable
    /// TODO: Refactor neighbour methods into cached getter properties
    [DataContract]
    public class HexSide : IEquatable<HexSide>
    {
        private HexPoint _HexPoint1;
        private HexPoint _HexPoint2;

        [DataMember]
        public HexLocation Hex1 { get; set; }

        [DataMember]
        public HexLocation Hex2 { get; set; }


        public HexPoint HexPoint1
        {
            get 
            {
                if (_HexPoint1 == null)
                    CalculatePoints();
                return _HexPoint1; 
            }
        }

        private void CalculatePoints()
        {
            HexLocation loc1 = null;
            HexLocation loc2 = null;
            HexLocation lefttop = HighestOrLeftestHex;
            int offset = lefttop.H % 2 == 0 ? 1 : 0;
            switch (Direction)
            {
                case ESideDirection.UpDown:
                    loc1 = new HexLocation(offset + lefttop.W, lefttop.H - 1);
                    loc2 = new HexLocation(offset + lefttop.W, lefttop.H + 1);
                    break;
                case ESideDirection.SlopeDown:
                    loc1 = new HexLocation(offset + lefttop.W, lefttop.H + 1);
                    loc2 = new HexLocation(lefttop.W - 1, lefttop.H);
                    break;
                case ESideDirection.SlopeUp:
                    loc1 = new HexLocation(lefttop.W + 1, lefttop.H);
                    loc2 = new HexLocation(lefttop.W -1 + offset, lefttop.H + 1);
                    break;
            }
            _HexPoint1 = new HexPoint(Hex1, Hex2, loc1);
            _HexPoint2 = new HexPoint(Hex1, Hex2, loc2);
        }

        public HexPoint HexPoint2
        {
            get 
            {
                if (_HexPoint2 == null)
                    CalculatePoints();
                return _HexPoint2;
            }
        }

        /// <summary>
        /// Returns the uppermost hex, or when both HexLocations consist of two HexLocations on the same
        /// row, the leftmost HexLocation
        /// </summary>
        public HexLocation HighestOrLeftestHex
        {
            get
            {
                if (Hex1.H == Hex2.H)
                    //both on same row, return leftest
                    return Hex1.W < Hex2.W ? Hex1 : Hex2;
                else
                    //different rows, return highest
                    return Hex1.H > Hex2.H ? Hex2 : Hex1;
            }
        }

        public ESideDirection Direction
        {
            get
            {
                // both hexes are on the same row, so the side is updown
                if (Hex1.H == Hex2.H) return ESideDirection.UpDown;

                if (HighestOrLeftestHex.H % 2 == 0)
                //even rows
                {
                    if (Hex1.W == Hex2.W)
                        return ESideDirection.SlopeDown;
                    else
                        return ESideDirection.SlopeUp;
                }
                else
                //uneven rows
                {
                    if (Hex1.W == Hex2.W)
                        return ESideDirection.SlopeUp;
                    else
                        return ESideDirection.SlopeDown;
                }
            }
        }

        /// <summary>
        /// Returns a list of all neighbour sides this side has
        /// </summary>
        /// <returns></returns>
        public List<HexSide> GetNeighbours()
        {

            List<HexSide> result = new List<HexSide>();
            HexLocation highestHex = HighestOrLeftestHex;
            int rowOffset = 0;

            switch (Direction)
            {
                case ESideDirection.SlopeDown:
                    // on even rows, add 1 to hexes not on same row as highestHex
                    if (highestHex.H % 2 == 0)
                        rowOffset = 1;

                    result.Add(new HexSide(
                        new HexLocation(highestHex.W - 1, highestHex.H), 
                        highestHex));

                    result.Add(new HexSide(
                        new HexLocation(highestHex.W - 1, highestHex.H),
                        new HexLocation(highestHex.W - 1 + rowOffset, highestHex.H + 1)));

                    result.Add(new HexSide(new HexLocation(
                        highestHex.W + rowOffset, highestHex.H + 1), highestHex));

                    result.Add(new HexSide(
                        new HexLocation(highestHex.W - 1 + rowOffset, highestHex.H + 1),
                        new HexLocation(highestHex.W + rowOffset, highestHex.H + 1)));
                    break;
                case ESideDirection.SlopeUp:
                    // on uneven rows, remove 1 to hexes not on same row as highestHex
                    if (highestHex.H % 2 != 0)
                        rowOffset = -1;

                    result.Add(new HexSide(
                        highestHex,
                        new HexLocation(highestHex.W + 1, highestHex.H)));

                    result.Add(new HexSide(
                        new HexLocation(highestHex.W + 1, highestHex.H),
                        new HexLocation(highestHex.W + 1 + rowOffset, highestHex.H + 1)));

                    result.Add(new HexSide(
                        highestHex,
                        new HexLocation(highestHex.W + rowOffset, highestHex.H + 1)));

                    result.Add(new HexSide(
                        new HexLocation(highestHex.W + rowOffset, highestHex.H + 1),
                        new HexLocation(highestHex.W + 1 + rowOffset, highestHex.H + 1)));
                    break;
                case ESideDirection.UpDown:
                    // highesthex is in this case the leftest hex
                    // on uneven rows, remove 1 to hexes not on same row as highestHex
                    if (highestHex.H % 2 == 0)
                    {
                        rowOffset = 1;
                    }
                    else
                    {
                        rowOffset = 0;
                    }

                    result.Add(new HexSide(
                        highestHex,
                        new HexLocation(highestHex.W + rowOffset, highestHex.H - 1)));

                    result.Add(new HexSide(
                        new HexLocation(highestHex.W + rowOffset, highestHex.H - 1),
                        new HexLocation(highestHex.W + 1, highestHex.H)));

                    result.Add(new HexSide(
                        new HexLocation(highestHex.W, highestHex.H),
                        new HexLocation(highestHex.W + rowOffset, highestHex.H + 1)));

                    result.Add(new HexSide(
                        new HexLocation(highestHex.W + 1, highestHex.H),
                        new HexLocation(highestHex.W + rowOffset, highestHex.H + 1)));
                    break;
            }

            return result;
        }

        /// <summary>
        /// Built constructor
        /// </summary>
        /// <param name="hex1"></param>
        /// <param name="hex2"></param>
        public HexSide(HexLocation hex1, HexLocation hex2)
        {
            Hex1 = hex1;
            Hex2 = hex2;
            CalculatePoints();
        }
        /// <summary>
        /// Construct a hexSide from two HexPoints
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        public HexSide(HexPoint point1, HexPoint point2)
        {
            _HexPoint1 = point1;
            _HexPoint2 = point2;
            if (point1.Equals(point2)) { }
            CalculateHexes();
        }

        /// <summary>
        /// Calculates the HexLocations based on two points
        /// </summary>
        private void CalculateHexes()
        {
            List<HexLocation> locations = new List<HexLocation>();
            locations.Add(_HexPoint1.Hex1);
            locations.Add(_HexPoint1.Hex2);
            locations.Add(_HexPoint1.Hex3);
            locations.Add(_HexPoint2.Hex1);
            locations.Add(_HexPoint2.Hex2);
            locations.Add(_HexPoint2.Hex3);

            var x = from l in locations
                    group l by l into lunique
                    where lunique.Count() == 2
                    select lunique.Key;

            Hex1 = x.First();
            Hex2 = x.Last();
        }

        /// <summary>
        /// Returns true if the side (consisting of two locations) uses given location
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public bool HasLocation(HexLocation check)
        {
            return Hex1.Equals(check) || Hex2.Equals(check);
        }

        /// <summary>
        /// Of the two hexpoints a sid has, returns the other point given compared to parameter point
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        public HexPoint GetOtherPoint(HexPoint first)
        {
            if (first.Equals(HexPoint1)) 
                return HexPoint2;
            else
                return HexPoint1;
        }

        /// <summary>
        /// Gets the two points at each end of the side
        /// </summary>
        /// <returns></returns>
        public List<HexPoint> GetNeighbourPoints()
        {
            List<HexPoint> result = new List<HexPoint>();

            HexLocation top = HighestOrLeftestHex;
            
            // TODO: headache code
            // either visualize using pics or rewrite
            switch (Direction)
            {
                case ESideDirection.UpDown:
                    int offset = HighestOrLeftestHex.H % 2 == 0 ? 1 : 0;
                    result.Add(new HexPoint(Hex1, Hex2,
                        new HexLocation(top.W + offset, top.H - 1)));
                    result.Add(new HexPoint(Hex1, Hex2,
                        new HexLocation(top.W + offset, top.H + 1)));
                    break;
                case ESideDirection.SlopeDown:
                    int offset2 = HighestOrLeftestHex.H % 2 == 0 ? 1 : 0;
                    result.Add(new HexPoint(Hex1, Hex2,
                        new HexLocation(top.W - 1, top.H)));
                    // generates bad hex
                    result.Add(new HexPoint(Hex1, Hex2,
                        new HexLocation(top.W + offset2, top.H + 1)));
                        break;
                case ESideDirection.SlopeUp:
                    int offset3 = HighestOrLeftestHex.H % 2 == 0 ? 0 : 1;
                    result.Add(new HexPoint(Hex1, Hex2,
                        new HexLocation(top.W - offset3, top.H + 1)));
                    result.Add(new HexPoint(Hex1, Hex2,
                        new HexLocation(top.W + 1, top.H)));
                    break;
            }
            return result;
        }

        private bool IsEqual(HexSide other)
        {
            return (Hex1.Equals(other.Hex1) && Hex2.Equals(other.Hex2)) ||
                   (Hex1.Equals(other.Hex2) && Hex2.Equals(other.Hex1));
        }
        public override bool Equals(object obj)
        {
            HexSide other = obj as HexSide;
            if (other != null)
                return IsEqual(other);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Hex1.GetHashCode() ^ Hex2.GetHashCode();
        }

        #region IEquatable<XmlHexSide> Members

        public bool Equals(HexSide other)
        {
            bool eq = IsEqual(other);
            return eq;
        }

        #endregion

        public HexPoint OtherPoint(HexPoint seaPoint)
        {
            if (HexPoint1.Equals(seaPoint))
                return HexPoint2;
            else
                return HexPoint1;
        }
    }
}
