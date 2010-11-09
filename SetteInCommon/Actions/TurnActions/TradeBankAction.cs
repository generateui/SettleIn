using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    public class TradeBankAction : TurnAction
    {
        [DataMember]
        public ResourceList OfferedCards { get; set; }

        [DataMember]
        public ResourceList WantedCards { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;
            if (OfferedCards == null || WantedCards == null)
            {
                _InvalidMessage = "OfferedCards or WantedCards cannot be null";
                return false;
            }

            
            if (OfferedCards.CountAllExceptDiscovery < 2)
            {
                _InvalidMessage = "We need at least two cards to offer the bank";
                return false;
            }

            // we need at least one card to want from the bank
            if (WantedCards.CountAllExceptDiscovery < 1)
            {
                _InvalidMessage = "We need at least one card to want from the bank";
                return false;
            }

            GamePlayer player = game.GetPlayer(base.Sender);

            // check if the player has the offered cards in hand
            if (!player.Resources.HasAtLeast(OfferedCards)) return false;
            
            int portDivider = 4;
            // check if the offer is valid
            if (OfferedCards.Timber > 0)
            {
                portDivider = 4;
                if (player.Ports.ThreeToOne > 0) portDivider = 3;
                if (player.Ports.Timber > 0) portDivider = 2;
                
                // return invalid state if the trade isnt even
                if (OfferedCards.Timber % portDivider != 0)
                {
                    _InvalidMessage = String.Format("We need {0} timber resources to trade", portDivider);
                    return false;
                }
            }
            if (OfferedCards.Wheat > 0)
            {
                portDivider = 4;
                if (player.Ports.ThreeToOne > 0) portDivider = 3;
                if (player.Ports.Wheat > 0) portDivider = 2;
                if (OfferedCards.Wheat % portDivider != 0)
                {
                    _InvalidMessage = String.Format("We need {0} wheat resources to trade", portDivider);
                    return false;
                }
            }
            if (OfferedCards.Ore > 0)
            {
                portDivider = 4;
                if (player.Ports.ThreeToOne > 0) portDivider = 3;
                if (player.Ports.Ore > 0) portDivider = 2;
                if (OfferedCards.Ore % portDivider != 0)
                {
                    _InvalidMessage = String.Format("We need {0} ore resources to trade", portDivider);
                    return false;
                }
            }
            if (OfferedCards.Clay > 0)
            {
                portDivider = 4;
                if (player.Ports.ThreeToOne > 0) portDivider = 3;
                if (player.Ports.Clay > 0) portDivider = 2;
                if (OfferedCards.Clay % portDivider != 0)
                {
                    _InvalidMessage = String.Format("We need {0} clay resources to trade", portDivider);
                    return false;
                }
            }
            if (OfferedCards.Sheep > 0)
            {
                portDivider = 4;
                if (player.Ports.ThreeToOne > 0) portDivider = 3;
                if (player.Ports.Sheep > 0) portDivider = 2;
                if (OfferedCards.Sheep % portDivider != 0)
                {
                    _InvalidMessage = String.Format("We need {0} sheep resources to trade", portDivider);
                    return false;
                }
            }

            return true;

        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);

            gamePlayer.Resources.SubtractCards(OfferedCards);
            gamePlayer.Resources.AddCards(WantedCards);

            _Message = String.Format("{0} exchanges {1} for {2}.",
                gamePlayer.XmlPlayer.Name, OfferedCards.ToString(), WantedCards.ToString());

            base.PerformTurnAction(xmlGame);
        }

    }
}
