using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.Board.Hexes;

namespace SettleInCommon.Actions.TurnActions
{
    /// <summary>
    /// Used when the board has ports which should be placed onto the board
    /// before the game starts
    /// </summary>
    [DataContract]
    public class PlacePortAction : TurnAction
    {
        [DataMember]
        public int TerritoryID { get; set; }

        [DataMember]
        public HexLocation Location { get; set; }

        [DataMember]
        public Port Port { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            if (Port == null || Location == null)
            {
                _InvalidMessage = "Location or Port cannot be null";
                return false;
            }

            // Check if the location is a seahex
            if (!(game.Board.Hexes[Location] is SeaHex))
            {
                _InvalidMessage = "Location is not a SeaHex. Ports can only be placed on Seahexes";
                return false;
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            ((SeaHex)xmlGame.Board.Hexes[Location]).XmlPort = Port;

            base.PerformTurnAction(xmlGame);
        }
        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should place a port", _GamePlayer.XmlPlayer.Name);
            }
        }
    }
}
