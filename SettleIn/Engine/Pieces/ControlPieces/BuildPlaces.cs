using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Boards;
using SettleInCommon.Gaming;

namespace SettleIn
{
    /// <summary>
    /// Calculates starting positions for players to put their initial and secondary
    /// placement options. Adds a set of 3d cilinders to the scene, which the user can click.
    /// </summary>
    public class BuildPlaces : ModelVisual3D
    {
        /// <summary>
        /// Returns true if given hex is buildable
        /// </summary>
        /// <param name="h">Hex to check on builability</param>
        /// <returns>true if we can built on the hex, false if we can't</returns>
        private bool CanBuild(Hex h)
        {
            // we cant build on NoneHexes, so omit them
            if (h is NoneHex) return false;

            // we cant build on discoveryhexes, so omit them
            if (h is DiscoveryHex) return false;

            return true;
            /*
            if (h is LandHex)
            {
                // We can built if the hex is visible and we can do initialplacement on it
                return h.Visible && ((LandHex)h).InitialPlacementAllowed;
            }
            else
            {
                // we can built if the hex is visible
                return h.Visible;
            }
             */
        }

        private Point2D AddOffset(Point2D point, EPointPositionOnHex positionOnHex)
        {
            switch (positionOnHex)
            {
                case (EPointPositionOnHex.TopMiddle):
                    return new Point2D(
                        point.X + Hex.HalfWidth,
                        point.Y);
                case (EPointPositionOnHex.RightTop):
                    return new Point2D(
                        point.X + Hex.Width,
                        point.Y + Hex.BottomHeight);
                case (EPointPositionOnHex.RightBottom):
                    return new Point2D(
                        point.X + Hex.Width,
                        point.Y + Hex.PartialHeight);
                case (EPointPositionOnHex.BottomMiddle):
                    return new Point2D(
                        point.X + Hex.HalfWidth,
                        point.Y + Hex.PartialHeight);

            }
            throw new ArgumentException("Enum cannot be " + positionOnHex.ToString());
        }

        public BuildPlaces() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="b"></param>
        public BuildPlaces(BoardVisual b, List<HexPoint> placedItems)
        {
            // This algorithm picks only the uneven rows, and then checks 4 out of total 6 places
            // on the hex: TopMiddle, BottomMiddle, RightTop, RightBottom. 
            // This way we loop through all possible points in the board
            // TODO: add territory initial + secondary building support

            foreach (Hex h in from Hex hex in b.Board.Hexes
                              where
               (hex.Location.H % 2 != 0) &&      //only get hexes on uneven rows
               !(hex is NoneHex) &&     //omit nonehexes, no building on them
               (hex.Location.W != b.Board.Width - 1)    //omit last hex in each row
                              select hex)
            {
                //0 (point at the top of a hex
                if (h.Location.W != 0 &&
                    CanBuild(b.Board.Hexes[h.Location.W - 1, h.Location.H - 1]) &&
                    CanBuild(b.Board.Hexes[h.Location.W, h.Location.H - 1]))
                {
                    Point2D point = AddOffset(EPointPositionOnHex.TopMiddle, b, b.CalculatePosition(h.Location));

                    //when all three sea hexes, there is no possibility to build
                    if (!(b.Board.Hexes[h.Location.W - 1, h.Location.H - 1] is SeaHex &&
                        b.Board.Hexes[h.Location.W, h.Location.H - 1] is SeaHex &&
                        h is SeaHex))
                    {
                        HexPoint hexPoint = CreatePoint(EPointPositionOnHex.TopMiddle, h.Location);

                        if (!HasNearPointOrSelf(placedItems, hexPoint) &&
                                !IsAllSea(hexPoint, b.Board))
                        {
                            this.Children.Add(new BuildPointVisual(point, hexPoint));
                        }
                    }
                }

                // 1 point at the right top
                if (h.Location.W != b.Board.Width - 1 &&
                    CanBuild(b.Board.Hexes[h.Location.W, h.Location.H - 1]) &&
                    CanBuild(b.Board.Hexes[h.Location.W + 1, h.Location.H]))
                {
                    Point2D point = AddOffset(EPointPositionOnHex.RightTop, b, b.CalculatePosition(h.Location));

                    //when all three sea hexes, there is no possibility to build
                    if (!(b.Board.Hexes[h.Location.W, h.Location.H - 1] is SeaHex &&
                        b.Board.Hexes[h.Location.W + 1, h.Location.H] is SeaHex &&
                        h is SeaHex))
                    {
                        HexPoint hexPoint = CreatePoint(EPointPositionOnHex.RightTop, h.Location);

                        if (!HasNearPointOrSelf(placedItems, hexPoint) &&
                                !IsAllSea(hexPoint, b.Board))
                        {
                            this.Children.Add(new BuildPointVisual(point, hexPoint));
                        }
                    }
                }
                if (h.Location.H != b.Board.Height - 1) //skip if hex is on last row
                {
                    // 2 point at the right lower position
                    if (h.Location.W != b.Board.Width - 1 && //skip if last hex in a row (last column)
                    h.Location.H != b.Board.Height - 1 &&
                    CanBuild(b.Board.Hexes[h.Location.W + 1, h.Location.H]) &&
                    CanBuild(b.Board.Hexes[h.Location.W, h.Location.H + 1]))
                    {
                        Point2D point = AddOffset(EPointPositionOnHex.RightBottom, b, b.CalculatePosition(h.Location));

                        //when all three sea hexes, there is no possibility to build
                        if (!(b.Board.Hexes[h.Location.W + 1, h.Location.H] is SeaHex &&
                            b.Board.Hexes[h.Location.W, h.Location.H + 1] is SeaHex &&
                            h is SeaHex))
                        {
                            HexPoint hexPoint = CreatePoint(EPointPositionOnHex.RightBottom, h.Location);

                            if (!HasNearPointOrSelf(placedItems, hexPoint) &&
                                !IsAllSea(hexPoint, b.Board))
                            {
                                this.Children.Add(new BuildPointVisual(point, hexPoint));
                            }
                        }
                    }
                    // 3 point at the bottom
                    if (h.Location.H != b.Board.Height - 1 && //skip if hex is on last row
                    h.Location.W != 0 &&
                    CanBuild(b.Board.Hexes[h.Location.W, h.Location.H + 1]) &&
                    CanBuild(b.Board.Hexes[h.Location.W - 1, h.Location.H + 1]))
                    {
                        Point2D point = AddOffset(EPointPositionOnHex.BottomMiddle, b, b.CalculatePosition(h.Location));

                        //when all three sea hexes, there is no possibility to build
                        if (!(b.Board.Hexes[h.Location.W, h.Location.H + 1] is SeaHex &&
                            b.Board.Hexes[h.Location.W - 1, h.Location.H + 1] is SeaHex &&
                            h is SeaHex))
                        {
                            HexPoint hexPoint = CreatePoint(EPointPositionOnHex.BottomMiddle, h.Location);

                            if (!HasNearPointOrSelf(placedItems, hexPoint) &&
                                !IsAllSea(hexPoint, b.Board))
                            {
                                this.Children.Add(new BuildPointVisual(point, hexPoint));
                            }
                        }
                    }
                }
            }
        }
        private bool IsAllSea(HexPoint point, XmlBoard board)
        {
            return false;
            /* 
            if (board.Hexes[point.Hex1] is SeaHex &&
                board.Hexes[point.Hex2] is SeaHex &&
                board.Hexes[point.Hex3] is SeaHex)
                return true;
            else
                return false;
             */
        }
        private bool HasNearPointOrSelf(List<HexPoint> checkList, HexPoint pointToCheck)
        {
            // Check if the list contains the point
            if (checkList.Contains(pointToCheck)) return true;

            // Check if the list contains any of the neightbours
            List<HexPoint> neighbours = pointToCheck.GetNeighbours();
            if (neighbours.Count != 3) Console.WriteLine("whoa");
            foreach (HexPoint neighbour in neighbours)
                if (checkList.Contains(neighbour)) return true;

            // Nope! the list doesnt contains the point or any of its neighbours
            return false;
        }

        /// <summary>
        /// Constructs a buildpointvisual using a list of hexsides of a player
        /// </summary>
        /// <param name="board"></param>
        /// <param name="places"></param>
        public BuildPlaces(BoardVisual board, GamePlayer player)
        {
            List<HexPoint> possiblePlaces = player.GetTownBuildPlaces(board.Game, board.Board);
            foreach (HexPoint pointToAdd in possiblePlaces)
            {
                Point2D point2 = board.CalculatePosition(pointToAdd);
                BuildPointVisual bpv = new BuildPointVisual(point2, pointToAdd);
                Children.Add(bpv);
            }

        }

        public BuildPlaces(BoardVisual board, List<HexPoint> towns, bool uselessoverloader)
        {
            foreach (HexPoint point in towns)
            {
                Point2D point2d = board.CalculatePosition(point);
                BuildPointVisual bpv = new BuildPointVisual(point2d, point);
                 Children.Add(bpv);
            }
        }

        public BuildPlaces(BoardVisual board, HexSide side)
        {
            foreach (HexPoint p in side.GetNeighbourPoints())
            {
                Children.Add(
                    new BuildPointVisual(board.CalculatePosition(p), p));
            }
            //Children.Add(new BuildPointVisual(board.CalculatePosition(side.GetNeighbourPoints()[0]), side.GetNeighbourPoints()[0]));
            //Children.Add(new BuildPointVisual(board.CalculatePosition(side.GetNeighbourPoints()[1]), side.GetNeighbourPoints()[1]));
        }

        public HexPoint CreatePoint(EPointPositionOnHex position, HexLocation location)
        {
            int w = location.W;
            int h = location.H;
            HexPoint result = new HexPoint();
            switch (position)
            {
                case EPointPositionOnHex.TopMiddle:
                    result.Hex1 = new HexLocation(w - 1, h - 1);
                    result.Hex2 = new HexLocation(w, h - 1);
                    result.Hex3 = new HexLocation(w, h);
                    break;
                case EPointPositionOnHex.RightTop:
                    result.Hex1 = new HexLocation(w, h - 1);
                    result.Hex2 = new HexLocation(w, h);
                    result.Hex3 = new HexLocation(w + 1, h);
                    break;
                case EPointPositionOnHex.RightBottom:
                    result.Hex1 = new HexLocation(w, h);
                    result.Hex2 = new HexLocation(w + 1, h);
                    result.Hex3 = new HexLocation(w, h + 1);
                    break;
                case EPointPositionOnHex.BottomMiddle:
                    result.Hex1 = new HexLocation(w, h);
                    result.Hex2 = new HexLocation(w - 1, h + 1);
                    result.Hex3 = new HexLocation(w, h + 1);
                    break;
            }

            return result;
        }


        public Point2D AddOffset(EPointPositionOnHex position, BoardVisual b, Point2D point)
        {
            Point2D result = new Point2D();
            // Offsets for positioning the visuals correctly
            double offsetx = 0;// (b.Board.Width * Hex.Width) / 2;
            double offsety = 0;// ((b.Board.Height * Hex.PartialHeight) + Hex.BottomHeight) / 2;

            switch (position)
            {
                case EPointPositionOnHex.TopMiddle:
                    result.X = point.X + offsetx + Hex.HalfWidth;
                    result.Y = point.Y + offsety;
                    return result;
                case EPointPositionOnHex.RightTop:
                    result.X = point.X + offsetx + Hex.Width;
                    result.Y = point.Y + offsety + Hex.BottomHeight;
                    return result;
                case EPointPositionOnHex.RightBottom:
                    result.X = point.X + offsetx + Hex.Width;
                    result.Y = point.Y + offsety + Hex.PartialHeight;
                    return result;
                case EPointPositionOnHex.BottomMiddle:
                    result.X = point.X + offsetx + Hex.HalfWidth;
                    result.Y = point.Y + offsety + Hex.Height;
                    return result;
            }
            throw new Exception("Should not reach this");
        }

        public BuildPlaces(List<HexPoint> points, BoardVisual board)
        {
            foreach (HexPoint p in points)
            {
                Children.Add(new BuildPointVisual(board.CalculatePosition(p), p));
            }
        }

        /// <summary>
        /// Constructs a new movdelvisual with appropriate positions marked where
        /// players can build
        /// </summary>
        /// <param name="b"></param>
        public BuildPlaces(BoardVisual b)
        {
            // Pseudo:
            //
            // By only checking the 4 points on the right (top, top right, 
            // lower right, center bottom) points, we can cover the whole
            // board if we just iterate over the hexes in uneven rows
            //
            // 1. Iterate through all hexes in uneven rows
            // 2. Add top center buildpoint, if appropriate
            //  2a. skip the first hex in a row
            // 3. Add top right buildpoint, if appropriate
            //  3a. skip last hex in a row
            // 4. Add lower right buildpoint, if appropriate
            //  4a. skip last hex on the row
            //  4b. Skip if hex is on last row
            // 5. Add center bottom buildpoint, if appropriate
            //  5a. skip last hex on the row
            //  5b. Skip if hex is on last row

            foreach (Hex h in b.Board.Hexes)
            {
                //NoneHexes should not get any build point, so skip them
                //We are only going to iterate over even rows
                if (!(h is NoneHex) && (h.Location.H % 2 != 0))
                {
                    //x offset for positioning the visuals correctly
                    double offsetx = -(b.Board.Width * Hex.Width) / 2;
                    //y offset for positioning the visuals correctly
                    double offsety = -((b.Board.Height * Hex.PartialHeight) + Hex.BottomHeight) / 2;
                    if (h.Location.W != Hex.Width - 1) //omit last hex in row
                    {
                        //0 (point at the top of a hex
                        if (h.Location.W != 0 && // skip if first hex in the row
                            CanBuild(b.Board.Hexes[h.Location.W - 1, h.Location.H - 1]) &&
                            CanBuild(b.Board.Hexes[h.Location.W, h.Location.H - 1]))
                        {
                            //when all three sea hexes, there is no possibility to build
                            if (!(b.Game.Board.Hexes[h.Location.W - 1, h.Location.H - 1] is SeaHex &&
                                b.Game.Board.Hexes[h.Location.W, h.Location.H - 1] is SeaHex &&
                                h is SeaHex))
                            {
                                HexPoint hexPoint = new HexPoint(h.Location, EPointPositionOnHex.TopMiddle);
                                Point2D point = b.CalculatePosition(hexPoint);
                                Children.Add(new BuildPointVisual(point, hexPoint));
                            }
                        }
                        // 1 point at the right top
                        if (h.Location.W != b.Game.Board.Width - 1 && //skip if last hex in a row
                            CanBuild(b.Board.Hexes[h.Location.W, h.Location.H - 1]) &&
                            CanBuild(b.Board.Hexes[h.Location.W + 1, h.Location.H]))
                        {
                            //when all three sea hexes, there is no possibility to build
                            if (!(b.Board.Hexes[h.Location.W, h.Location.H - 1] is SeaHex &&
                                b.Board.Hexes[h.Location.W + 1, h.Location.H] is SeaHex &&
                                h is SeaHex))
                            {
                                HexPoint hexPoint = new HexPoint(h.Location, EPointPositionOnHex.RightTop);
                                Point2D point = b.CalculatePosition(hexPoint);
                                Children.Add(new BuildPointVisual(point, hexPoint));
                            }
                        }
                        if (h.Location.H != Hex.Height - 1) //skip if hex is on last row
                        {
                            // 2 point at the right lower position
                            if (h.Location.W != b.Board.Width - 1 && //skip if last hex in a row (last column)
                                h.Location.H != b.Board.Height - 1 && //skip if hex is on last row
                                CanBuild(b.Board.Hexes[h.Location.W + 1, h.Location.H]) &&
                                CanBuild(b.Board.Hexes[h.Location.W, h.Location.H + 1]))
                            {
                                //when all three sea hexes, there is no possibility to build
                                if (!(b.Board.Hexes[h.Location.W + 1, h.Location.H] is SeaHex &&
                                    b.Board.Hexes[h.Location.W, h.Location.H + 1] is SeaHex &&
                                    h is SeaHex))
                                {
                                    HexPoint hexPoint = new HexPoint(h.Location, EPointPositionOnHex.RightBottom);
                                    Point2D point = b.CalculatePosition(hexPoint);
                                    Children.Add(new BuildPointVisual(point, hexPoint));
                                }
                            }
                            // 3 point at the bottom
                            if (h.Location.H != b.Game.Board.Height - 1 && //skip if hex is on last row
                                h.Location.W != 0 && //skip if first in a row
                                CanBuild(b.Board.Hexes[h.Location.W, h.Location.H + 1]) &&
                                CanBuild(b.Board.Hexes[h.Location.W - 1, h.Location.H + 1]))
                            {
                                //when all three sea hexes, there is no possibility to build
                                if (!(b.Board.Hexes[h.Location.W, h.Location.H + 1] is SeaHex &&
                                    b.Board.Hexes[h.Location.W - 1, h.Location.H + 1] is SeaHex &&
                                    h is SeaHex))
                                {
                                    HexPoint hexPoint = new HexPoint(h.Location, EPointPositionOnHex.BottomMiddle);
                                    Point2D point = b.CalculatePosition(hexPoint);
                                    Children.Add(new BuildPointVisual(point, hexPoint));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
