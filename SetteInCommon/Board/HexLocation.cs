using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using SettleInCommon.Board.Hexes;

namespace SettleInCommon.Board
{
    [DataContract]
    public class HexLocation : IEquatable<HexLocation>
    {
        [DataMember]
        public int W { get; set; }

        [DataMember]
        public int H { get; set; }

        public HexLocation(int w, int h)
        {
            H = h;
            W = w;
        }
        public HexLocation() { }

        public override string ToString()
        {
            return String.Format("w: {0}, h: {1}", W, H);
        }
        public override bool Equals(object obj)
        {
            HexLocation location = obj as HexLocation;
            if (location != null)
            {
                return location.H == H && location.W == W;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return W ^ H;
        }

        #region IEquatable<XmlHexLocation> Members

        public bool Equals(HexLocation other)
        {
            return other.W == W && other.H == H;
        }

        #endregion

        public string ToString(XmlBoard xmlBoard)
        {
            return xmlBoard.Hexes[this].ToString();
        }

        /// <summary>
        /// Returns a list of surrounding hex locations
        /// </summary>
        /// <returns></returns>
        public List<HexLocation> GetNeighbours()
        {
            List<HexLocation> result = new List<HexLocation>();

            // add an offset for uneven rows
            int offset = H % 2 == 0 ? 0 : -1;

            //2 hexes on the same row
            result.Add(new HexLocation(W - 1, H));
            result.Add(new HexLocation(W + 1, H));

            //2 hexes on the row above
            result.Add(new HexLocation(W + 1 + offset, H - 1));
            result.Add(new HexLocation(W + offset, H - 1));

            //2 hexes on the row below
            result.Add(new HexLocation(W + 1 + offset, H + 1));
            result.Add(new HexLocation(W + offset, H + 1));

            return result;
        }

        /// <summary>
        /// Returns a list of hexpoints, index starting with top point, 
        /// going clockwise
        /// </summary>
        /// <returns></returns>
        public List<HexPoint> GetNeighbourPoints()
        {
            List<HexPoint> result = new List<HexPoint>();

            // add an offset for uneven rows
            int offset = H % 2 == 0 ? 0 : -1;

            result.Add(new HexPoint(
                this, 
                new HexLocation(W + offset,     H - 1),
                new HexLocation(W + offset + 1, H - 1)));

            result.Add(new HexPoint(
                this, 
                new HexLocation(W + offset + 1, H - 1),
                new HexLocation(W + 1, H)));

            result.Add(new HexPoint(
                this, 
                new HexLocation(W + 1, H),
                new HexLocation(W + offset + 1, H + 1)));

            result.Add(new HexPoint(
                this,
                new HexLocation(W + offset + 1, H + 1),
                new HexLocation(W + offset,     H + 1)));

            result.Add(new HexPoint(
                this,
                new HexLocation(W + offset, H + 1),
                new HexLocation(W - 1, H)));

            result.Add(new HexPoint(
                this,
                new HexLocation(W - 1, H),
                new HexLocation(W + offset, H - 1)));

            return result;

        }

        /// <summary>
        /// Returns the position of the hexpoint on the hexlocation
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public HexPoint GetPosition(ERotationPosition position)
        {
            List<HexPoint> neighbours = GetNeighbourPoints();
            switch (position)
            {
                case ERotationPosition.Deg0: return neighbours[0];
                case ERotationPosition.Deg60: return neighbours[1];
                case ERotationPosition.Deg120: return neighbours[2];
                case ERotationPosition.Deg180: return neighbours[3];
                case ERotationPosition.Deg240: return neighbours[4];
                case ERotationPosition.Deg300: return neighbours[5];
            }

            return null;
        }

        /// <summary>
        /// Returns two locations
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public HexSide GetSideLocation(ERotationPosition position)
        {
            List<HexPoint> neighbours = GetNeighbourPoints();
            switch (position)
            {
                case ERotationPosition.Deg0: return new HexSide(neighbours[3], neighbours[4]);
                case ERotationPosition.Deg60: return new HexSide(neighbours[2], neighbours[3]);
                case ERotationPosition.Deg120: return new HexSide(neighbours[1], neighbours[2]);
                case ERotationPosition.Deg180: return new HexSide(neighbours[0], neighbours[1]);
                case ERotationPosition.Deg240: return new HexSide(neighbours[5], neighbours[0]);
                case ERotationPosition.Deg300: return new HexSide(neighbours[4], neighbours[5]);
            }

            return null;
        }

        /// <summary>
        /// Returns hexpoint based on a dice roll
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public HexPoint GetPosition(int i)
        {
            switch (i)
            {
                case 1: return GetPosition(ERotationPosition.Deg0);
                case 2: return GetPosition(ERotationPosition.Deg60);
                case 3: return GetPosition(ERotationPosition.Deg120);
                case 4: return GetPosition(ERotationPosition.Deg180);
                case 5: return GetPosition(ERotationPosition.Deg240);
                case 6: return GetPosition(ERotationPosition.Deg300);
            }
            return null;
        }

        public List<HexSide> GetPoints()
        {
            List<HexSide> result = new List<HexSide>();

            foreach (HexLocation neighbour in GetNeighbours())
                result.Add(new HexSide(neighbour, this));
            
            return result;
        }
    }
}
