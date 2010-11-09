using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Gaming;

using SettleIn.Engine.Boards;

namespace SettleIn.Engine.Pieces.ControlPieces
{
    public class RoutePlaces : ModelVisual3D
    {
        private List<HexPoint> OpponentsTownsCities(XmlGame game)
        {
            List<HexPoint> result = new List<HexPoint>();

            foreach (GamePlayer player in game.Players)
            {
                if (player.XmlPlayer.Name != game.PlayerOnTurn.XmlPlayer.Name)
                {
                    foreach (HexPoint town in player.Towns) result.Add(town);
                    foreach (HexPoint city in player.Cities) result.Add(city);
                }
            }

            return result;
        }

        private List<HexSide> OpponentsRoadsShips(XmlGame game)
        {
            List<HexSide> result = new List<HexSide>();

            foreach (GamePlayer player in game.Players)
            {
                if (player.XmlPlayer.Name != game.PlayerOnTurn.XmlPlayer.Name)
                {
                    foreach (HexSide road in player.Roads) result.Add(road);
                    foreach (HexSide ship in player.Ships) result.Add(ship);
                }
            }

            return result;
        }

        /// <summary>
        /// Calculate three possible locations for the road to be placed at placement gamephase
        /// </summary>
        /// <param name="game"></param>
        /// <param name="board"></param>
        /// <param name="townOrCity"></param>
        public RoutePlaces(XmlGame game, BoardVisual board, HexPoint townOrCity)
        {
            // Add each side to children
            foreach (HexSide side in townOrCity.GetNeighbourSides)
            {
                Point2D newPoint = board.CalculatePosition(side);
                HexSideVisual newHexSide = new HexSideVisual(newPoint, side);
                Children.Add(newHexSide);
            }
        }

        public RoutePlaces(BoardVisual board, HexSide side)
        {
            foreach (HexSide s in side.GetNeighbours())
            {
                Point2D newPoint = board.CalculatePosition(s);
                HexSideVisual newHexSide = new HexSideVisual(newPoint, s);
                Children.Add(newHexSide);
            }
        }
        public RoutePlaces()
        {
        }

        public RoutePlaces(List<HexSide> list, BoardVisual board)
        {
            foreach (HexSide side in list)
            {
                Children.Add(new HexSideVisual(board.CalculatePosition(side), side));
            }
        }

        /// <summary>
        /// Calculate all possible positions to build a ship or a road
        /// </summary>
        /// <param name="game"></param>
        /// <param name="board"></param>
        /// <param name="ships"></param>
        public RoutePlaces(
            XmlGame game,
            BoardVisual board,
            bool ships)
        {
            if (ships)
            {
                foreach (HexSide side in game.PlayerOnTurn.GetShipBuildPlaces(game, board.Board))
                    Children.Add(new HexSideVisual(board.CalculatePosition(side), side));
            }
            else
            {
                foreach (HexSide side in game.PlayerOnTurn.GetRoadBuildPlaces(game, board.Board))
                    Children.Add(new HexSideVisual(board.CalculatePosition(side), side));
            }
        }

    }
}
