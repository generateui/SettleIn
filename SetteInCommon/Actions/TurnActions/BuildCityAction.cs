using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Gaming.GamePhases;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    public class BuildCityAction : TurnAction
    {
        [DataMember]
        public HexPoint Location { get; set; }

        public bool IsSecond(XmlGame game)
        {
            if (!game.Settings.TournamentStart)
                return false;
            else
            {
                if (game.GameLog.OfType<BuildCityAction>().Count() < game.Players.Count)
                    return true;
                else
                    return false;
            }
        }

        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should build a city", _GamePlayer.XmlPlayer.Name);
            }
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            // we need at least an instance of the new place
            if (Location == null)
            {
                _InvalidMessage = "Location cant be null";
                return false;
            }

            // player should have a ship or road at some neighbour
            GamePlayer player = game.GetPlayer(base.Sender);

            //foreach (HexSide neighbour in Location.GetNeighbourSides)
            //    if (player.Ships.Contains(neighbour))
            //    {
            //        _InvalidMessage = "No neighbouring ship or road found";
            //        return false;
            //    }

            if (!(player.Towns.Contains(Location)))
            {
                _InvalidMessage = "No town found to replace with a city";
                return false;
            }
            if (game.Phase.GetType() == typeof(PlayTurnsGamePhase))
            {
                if (!player.CanBuildCity())
                {
                    _InvalidMessage = "Player cannot build the city";
                    return false;
                }

                if (!player.CanPayCity)
                {
                    _Message = "Player cannot pay for the city";
                    return false;
                }
            }

            // couldnt find a neighbour, assume invalid state
            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);
            if (xmlGame.Phase.GetType() == typeof(PlayTurnsGamePhase))
            {
                gamePlayer.Towns.Remove(Location);
                gamePlayer.Cities.Add(Location);
                gamePlayer.Resources.SubtractCards(ResourceList.City);
                xmlGame.Bank.AddCards(ResourceList.City);
                gamePlayer.StockCities--;
                gamePlayer.StockTowns++;
            }
            if (xmlGame.Phase.GetType() == typeof(PlacementGamePhase))
            {
                gamePlayer.Cities.Add(Location);
                gamePlayer.StockCities--;

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

            _Message = String.Format("{0} build a city at {1}",
                gamePlayer.XmlPlayer.Name, Location.ToString(xmlGame.Board));

            base.PerformTurnAction(xmlGame);
        }
    }
}
