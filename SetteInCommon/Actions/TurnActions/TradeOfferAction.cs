using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.Actions.InGame;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    [KnownType(typeof(CounterTradeOfferAction))]
    public class TradeOfferAction : TurnAction
    {
        [DataMember]
        public ResourceList OfferedCards { get; set; }

        [DataMember]
        public ResourceList WantedCards { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            if (OfferedCards == null ||
                WantedCards == null)
            {
                _InvalidMessage = "Offered- or WantedCards can't be null";
                return false;
            }

            if (OfferedCards.CountAllExceptDiscovery < 1 ||
                WantedCards.CountAllExceptDiscovery < 1)
            {
                _InvalidMessage = "We must have something to offer and want";
                return false;
            }

            if (OfferedCards.Discoveries > 0 ||
                WantedCards.Discoveries > 0) 
            {
                _InvalidMessage = "We cant trade discoveries";
                return false;
            }
                 
            GamePlayer player = game.GetPlayer(Sender);
            
            if (!player.Resources.HasAtLeast(OfferedCards)) 
            {
                _InvalidMessage="player should have the resources in hand";
                return false;
            }

            if (game.GameLog.OfType<TradeOfferAction>()
                    .Where(to => to.TurnID == game.CurrentTurn)
                    .Count() >= game.Settings.MaximumTradesPerTurn)
            {
                _InvalidMessage = String.Format("Player has already offered {0} trades", 
                    game.Settings.MaximumTradesPerTurn); 
            }

            // we can't think of more exceptions here
            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);

            _Message = String.Format("{0} offered to trade {1} for {2}",
                gamePlayer.XmlPlayer.Name, OfferedCards.ToString(),
                WantedCards.ToString());

            base.PerformTurnAction(xmlGame);
        }
        
    }
}
