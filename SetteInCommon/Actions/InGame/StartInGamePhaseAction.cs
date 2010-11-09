using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Actions.InGame;

namespace SettleInCommon.Actions.InGame
{
    /// <summary>
    /// Player starts a turn
    /// </summary>
    [DataContract]
    public class PlacementDoneAction : InGameAction
    {
        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid()) return false;

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            // set current player to winner
            xmlGame.PlayerOnTurn = xmlGame.Players[0];

            _Message = String.Format("{0} should start. Waiting for {0} to roll the dice", GamePlayer.XmlPlayer.Name);

            base.PerformTurnAction(xmlGame);
        }


    }
}
