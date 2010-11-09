using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.InGame
{
    /// <summary>
    /// Represents a counter trade offer from a player
    /// </summary>
    [DataContract]
    public class CounterTradeOfferAction : TradeOfferAction
    {
        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid()) return false;

            if (game.GameLog.Count == 0)
            {
                _InvalidMessage = "We should have more then zero gameactions";
                return false;
            }

            //Make sure the last action by the current player is a TradeOfferAction.
            // Therefore, the last TurnAction should be a TradeOfferAction
            if (!(game.GameLog.OfType<TurnAction>()
                .Last<TurnAction>() is TradeOfferAction))
            {
                _InvalidMessage = "Last played action should be a tradeoffer action";
                return false;
            }

            return true;
        }

    }
}
