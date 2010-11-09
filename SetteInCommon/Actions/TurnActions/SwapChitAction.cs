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
    /// Player swaps a chit from one territory to another
    /// TODO: This can and should be visualizd using boardvisual propertychanging stuff
    /// </summary>
    [DataContract]
    public class SwapChitAction : TurnAction
    {
        /// <summary>
        /// Old lcoation of a hex with a number to remove
        /// </summary>
        [DataMember]
        HexLocation OldLocation { get; set; }

        /// <summary>
        /// New location of hex to put the number on
        /// </summary>
        [DataMember]
        HexLocation NewLocation { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            // previous action should always be roadbuilding or shipbuilding

            // all fields must have an instance
            if (OldLocation == null || NewLocation == null) // || Number == null)
                return false;

            // we can only swap a chit on two hexes with numbers (resourehexes)
            if (!(game.Board.Hexes[OldLocation] is ResourceHex) ||
                !(game.Board.Hexes[NewLocation] is ResourceHex))
                return false;

            // old chit should be there, and the number should be the same
            if (((ResourceHex)game.Board.Hexes[OldLocation]).XmlChit == null) return false;
            
            // New location should have no chits
            if (((ResourceHex)game.Board.Hexes[OldLocation]).XmlChit != null) return false;

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer player = xmlGame.GetPlayer(Sender);
            Chit chit = ((ResourceHex)xmlGame.Board.Hexes[OldLocation]).XmlChit;

            //remove old chit number
            ((ResourceHex)xmlGame.Board.Hexes[OldLocation]).XmlChit = null;

            //put chitnumber on new location
            ((ResourceHex)xmlGame.Board.Hexes[NewLocation]).XmlChit = chit;

            _Message = String.Format("{0} swapped a number chit from {1} to {2}",
                player.XmlPlayer.Name, OldLocation.ToString(xmlGame.Board),
                NewLocation.ToString(xmlGame.Board));

            base.PerformTurnAction(xmlGame);
        }
        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should swap a chit", _GamePlayer.XmlPlayer.Name);
            }
        }
    }
}
