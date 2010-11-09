using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using SettleInCommon.Gaming;
using SettleInCommon.Actions.InGame;

namespace SettleInCommon.Actions.TurnActions
{
    public class TradePlayerAction : TurnAction
    {
        /// <summary>
        /// A TradeOfferAction may originate from a TradeOfferAction started by the player on turn,
        /// or by a player performing a counteroffer.
        /// </summary>
        [DataMember]
        public TradeOfferAction Trade { get; set; }

        [DataMember]
        public CounterTradeOfferAction Counter { get; set; }
        
        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            if (Trade == null)
            {
                _InvalidMessage = "The trade object cannot be null";
                return false;
            }

            if (Trade.Sender == 0)
            {
                _InvalidMessage = "Player to trade with cannot be the server (ID=0)";
                return false;
            }

            if (!Trade.IsValid())
            {
                _InvalidMessage = Trade.InvalidMessage;
                return false;
            }
            // TODO: check if trade is valid

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);
            // TODO: set trading player
            //GamePlayer tradePlayer = xmlGame.GetPlayer(AcceptedPlayerID);

            _Message = String.Format("{0} traded {1} for {2} with {3}",
                gamePlayer.XmlPlayer.Name, Trade.OfferedCards.ToString(),
                Trade.WantedCards.ToString(),"");// tradePlayer.XmlPlayer.Name);

            base.PerformTurnAction(xmlGame);
        }
    }
}
