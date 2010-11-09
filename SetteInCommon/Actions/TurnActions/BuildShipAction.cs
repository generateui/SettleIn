using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Gaming.GamePhases;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    public class BuildShipAction : TurnAction
    {
        [DataMember]
        public HexSide Intersection { get; set; }

        [DataMember]
        public bool FromDevcard { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            if (Intersection == null)
            {
                _InvalidMessage = "Intersection can't be null";
                return false;
            }
            if (game.GetPlayer(Sender).Ships.Contains(Intersection))
            {
                _InvalidMessage = "location is already taken";
                return false;
            }

            GamePlayer player = game.GetPlayer(base.Sender);

            if (game.Phase.GetType() != typeof(PlacementGamePhase))
            {
                if (!player.CanBuildShip(game, game.Board))
                {
                    _InvalidMessage = "Player cannot build the ship";
                    return false;
                }

                if (!player.CanPayShip)
                {
                    _Message = "Player cannot pay for the ship";
                    return false;
                }
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            bool usingRoadbuildingToken = false;
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);

            // in Ingame phase, player should pay for ship somehow
            if (xmlGame.Phase.GetType() == typeof(PlayTurnsGamePhase))
            {
                // Default on using a RoadBuilding development card token
                if (gamePlayer.DevRoadShips > 0)
                {
                    gamePlayer.DevRoadShips--;
                }
                else
                {
                    gamePlayer.Resources.SubtractCards(ResourceList.Ship);
                    xmlGame.Bank.AddCards(ResourceList.Ship);
                }

                // Check if the LR should be updated
                xmlGame.CalculateLongestRoad(gamePlayer);
            }
            else
            {
                // when in placement phase, ship is free
            }

            gamePlayer.StockShips--;
            gamePlayer.Ships.Add(Intersection);

            _Message = String.Format("{0} built a ship", 
                xmlGame.GetPlayer(Sender).XmlPlayer.Name);

            if (usingRoadbuildingToken)
                _Message += ", using his Roadbuilding development card";

            base.PerformTurnAction(xmlGame);
        }

        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should build a ship", _GamePlayer.XmlPlayer.Name);
            }
        }


    }
}
