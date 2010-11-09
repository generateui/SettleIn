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
    [DataContract]
    public class PlaceRobberPirateAction : TurnAction
    {
        [DataMember]
        public HexLocation Location { get; set; }

        [DataMember]
        public bool IsPirate { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            // we need a good location
            if (Location == null)
            {
                _InvalidMessage = "Location can't be null";
                return false;
            }

            // Make sure the player does not put robber or pirate on the edge of the map,
            // which is forbidden
            // TODO: remove assumption edge of map equals boundaries of array
            // possible solutions:
            // -on map serialization determine an edgemap, include edgemap property
            //
            if (Location.H == 0 ||
                Location.W == 0 ||
                Location.H >= game.Board.Height ||
                Location.W >= game.Board.Width)
            {
                _InvalidMessage = "putting robber on the edge is not allowed";
                return false;
            }

            // TODO: check if a previous action included rolling a 7,
            // or playing a soldier development card

            // Player may not leave the robber on the same location
            if (game.Robber.Equals(Location))
            {
                _InvalidMessage = "putting robber back onto same location is not allowed";
                return false;
            }

            // valid hexes include:
            // -seahexes for pirate
            // -ordinary resource hexes
            // -volcano's
            // -jungle's
            // 
            // invalid places include:
            // -Randomhex
            // -Nonehex
            // -DiscoveryHex
            // -

            Hex hex = game.Board.Hexes[Location];

            if (hex is NoneHex ||
                hex is DiscoveryHex ||
                hex is RandomHex)
            {
                _InvalidMessage = "Can't place robber or pirate on a " + hex.GetType().Name;
                return false;
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            if (xmlGame.Board.Hexes[Location] is SeaHex)
            {
                xmlGame.Pirate = Location;
                IsPirate = true;
            }
            else
            {
                xmlGame.Robber = Location;
                IsPirate = false;
            }

            _Message = String.Format("{0} put the robber on the {1}",
                xmlGame.GetPlayer(Sender).XmlPlayer.Name, Location.ToString(xmlGame.Board));

            base.PerformTurnAction(xmlGame);
        }

        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should place the robber or the pirate", _GamePlayer.XmlPlayer.Name);
            }
        }

    }
}
