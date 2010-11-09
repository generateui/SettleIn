using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.User;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    public class MoveShipAction : TurnAction
    {
        [DataMember]
        public HexSide OldLocation { get; set; }

        [DataMember]
        public HexSide NewLocation { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            GamePlayer player = game.GetPlayer(base.Sender);

            if (OldLocation == null || NewLocation == null)
            {
                _Message = "OldLocation or NewLocation is null";
                return false;
            }

            if (!player.Ships.Contains(OldLocation))
            {
                _Message = String.Format("Cannot find a ship matching the OldLocation given at player {0}", player.XmlPlayer.Name);
                return false;
            }

            // we cannot move the ship to a spot an opponent owns
            if (game.AllRoadsShips().Contains(NewLocation)) 
            {
                _Message = String.Format("Some opponent already has a ship at location {0}", NewLocation.ToString());
                return false;
            }

           /*
            // new location either should have a town or a city
            if (townsCities.Contains(NewLocation.Hex1) ||
                townsCities.Contains(NewLocation.Hex2))
                return true;
            */

            //another ship is also OK as suitable location
            foreach (HexSide neighbour in NewLocation.GetNeighbours())
                if (player.Ships.Contains(neighbour)) return true;

            return false;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);

            gamePlayer.Ships.Remove(OldLocation);
            gamePlayer.Ships.Add(NewLocation);

            _Message = String.Format("{0} moved a ship from {1} to {2}",
                gamePlayer.XmlPlayer.Name, OldLocation.ToString(), NewLocation.ToString());

            base.PerformTurnAction(xmlGame);
        }

    }
}
