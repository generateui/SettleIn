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
    public class BuildRoadAction : TurnAction
    {
        /// <summary>
        /// When in placement phase, town/city placed before
        /// Can be removed thus replaced by checking previous action of current player
        /// </summary>
        [DataMember]
        public HexPoint OriginatingTownOrCity { get; set; }

        [DataMember]
        public HexSide Intersection { get; set; }

        [DataMember]
        public bool FromDevcard { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;
            if (Intersection == null)
            {
                _InvalidMessage = "Intersection cannot be null";
                return false;
            }

            // the spot must be free still
            if (game.AllRoadsShips().Contains(Intersection))
            {
                _InvalidMessage = "Already built on the given location";
                return false;
            }

            GamePlayer player = game.GetPlayer(base.Sender);

            if (game.Phase.GetType() == typeof(PlacementGamePhase))
            {
                if (!player.CanBuildRoad(game, game.Board))
                {
                    _InvalidMessage = "Player cannot build the road";
                    return false;
                }

                /*
                if (!player.CanPayRoad)
                {
                    _Message = "Player cannot pay for the road";
                    return false;
                }
                 */
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame game)
        {
            GamePlayer pplayer = game.GetPlayer(Sender);
            bool usedRoadbuildingToken = false;

            if (game.Board.Hexes.OfType<DiscoveryHex>().Count() > 0)
            {

            }

            //when in InGame phase, player should pay for road somehow
            if (game.Phase.GetType() == typeof(PlayTurnsGamePhase))
            {
                // When played a roadbuilding card, first use up roadbuilding tokens
                if (pplayer.DevRoadShips > 0)
                {
                    //the player has played a road building card this turn
                    pplayer.DevRoadShips--;
                }
                else
                {
                    pplayer.Resources.SubtractCards(ResourceList.Road);
                    game.Bank.AddCards(ResourceList.Road);
                }
                
            }

            pplayer.StockRoads--;
            pplayer.Roads.Add(Intersection);

            if (game.Phase.GetType() == typeof(PlayTurnsGamePhase))
            {
                // Check if the LR should be updated
                game.CalculateLongestRoad(pplayer);
            }

            _Message = String.Format("{0} built a road" ,game.GetPlayer(Sender).XmlPlayer.Name);

            if (usedRoadbuildingToken)
                _Message += ", using his roadbuilding development card.";

            base.PerformTurnAction(game);
        }

        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should build a road", _GamePlayer.XmlPlayer.Name);
            }
        }
    }
}
