using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Gaming.GamePhases;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    public class BuildTownAction : TurnAction  
    {
        [DataMember]
        public HexPoint Location { get; set; }

        public bool IsSecond(XmlGame game)
        {
            if (game.Settings.TournamentStart)
                return false;
            else
            {
                if (game.GameLog.OfType<BuildTownAction>().Count() < (game.Players.Count * 2))
                    return true;
                else
                    return false;
            }
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;
            // we need at least an instance of the new place
            if (Location == null)
            {
                _InvalidMessage = "Location cannot be null";
                return false;
            }

            // 
            if (game.AllTownsCities().Contains(Location))
            {
                _InvalidMessage = "The spot and its neighbours is already used by anyone";
                return false;
            }

            // player should have a ship or road at some neighbour
            GamePlayer player = game.GetPlayer(base.Sender);
            if (game.Phase.GetType() == typeof(PlayTurnsGamePhase))
            {
                List<HexSide> roadsShips = player.GetRoadsShips();
                List<HexSide> neighbours = Location.GetNeighbourSides;

                if (!roadsShips.Contains(neighbours[0]) &&
                    !roadsShips.Contains(neighbours[1]) &&
                    !roadsShips.Contains(neighbours[2]))
                {
                    _InvalidMessage = "No ship or road found at neighbouring  location";
                    return false;
                }

                if (!player.CanPayTown)
                {
                    _Message = "Player cannot pay for the town";
                    return false;
                }

                if (!player.CanBuildTown(game, game.Board))
                {
                    _InvalidMessage = "Player cannot build the town";
                    return false;
                }
            }

            // check if location is suitable (hexpoint neighbours can't be 
            // already built on)

            // check if location is a valid one to built on 
            // (contains at least a landhex)


            // couldnt find a neighbour, assume invalid state
            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);

            // update town management
            gamePlayer.Towns.Add(Location);
            gamePlayer.StockTowns--;

            if (xmlGame.Phase.GetType() == typeof(PlayTurnsGamePhase))
            {
                // remove players' resources
                gamePlayer.Resources.SubtractCards(ResourceList.Town);

                // put resources back to bank
                xmlGame.Bank.AddCards(ResourceList.Town);
                
                // Check if the LR should be updated
                xmlGame.CalculateLongestRoad(gamePlayer);
            }
            if (xmlGame.Phase.GetType() == typeof(PlacementGamePhase) &&
                gamePlayer.GetTownsCities().Count ==2)
            {
                //player gets resources in neighbouring hexes
                List<HexLocation> hexes = new List<HexLocation>();
                hexes.Add(Location.Hex1);
                hexes.Add(Location.Hex2);
                hexes.Add(Location.Hex3);

                foreach (HexLocation loc in hexes)
                {
                    Hex hex = xmlGame.Board.Hexes[loc];
                    if (hex is ResourceHex)
                    {
                        gamePlayer.Resources.Add(((ResourceHex)hex).Resource);
                    }
                }
            }

            // Check if the town is built on a port, if so, add port 
            // to list of ports of the player

            // get the port
            var ports = from SeaHex h in xmlGame.Board.Hexes.OfType<SeaHex>()
                        where h.XmlPort != null
                        select h;
            var port = (from SeaHex p in ports
                       where
                        p.XmlPort.SideLocation.HexPoint1.Equals(Location) ||
                         p.XmlPort.SideLocation.HexPoint2.Equals(Location)
                        select p).SingleOrDefault();

            if (port != null)
            {
                xmlGame.PlayerOnTurn.Ports.Add(port.XmlPort.PortType);
            }

            _Message = String.Format("{0} build a town.", 
                gamePlayer.XmlPlayer.Name);
            _User = gamePlayer.XmlPlayer;
            _GamePlayer = gamePlayer;

            base.PerformTurnAction(xmlGame);
        }
        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should build a town", _GamePlayer.XmlPlayer.Name);
            }
        }
    }
}
