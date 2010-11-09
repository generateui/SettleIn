using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;
using SettleInCommon.Board;

namespace SettleInCommon.Actions.InGame
{
    [DataContract]
    public class LooseCardsAction : InGameAction
    {
        [DataMember]
        public ResourceList ResourcesLost { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid()) return false;

            if (ResourcesLost == null)
            {
                _InvalidMessage = "ResourcesLost object cannot be null";
                return false;
            }

            if (ResourcesLost.CountAllExceptDiscovery == 0)
            {
                _InvalidMessage = "We cannot loose 0 cards";
                return false;
            }


            GamePlayer player = game.GetPlayer(base.Sender);
            int numResources = player.Resources.CountAllExceptDiscovery;

            // we should have more cards in hand than the maximum allowed
            if (numResources <= game.Settings.MaximumCardsInHandWhenSeven)
            {
                _InvalidMessage = "You should have more than the maximum allowed cards";
                return false;
            }

            // Player should have the resources he is trying to get rid off
            if (!player.Resources.HasAtLeast(ResourcesLost))
            {
                _InvalidMessage = String.Format("{0} does not have the specified resources ({1}) to loose",
                    player.XmlPlayer.Name, ResourcesLost.ToString());
                return false;
            }

            // player must ditch half of his cards, rounded down
            int half = (numResources % 2 == 0 ? 
                numResources / 2 : (numResources - 1) / 2);

            if (ResourcesLost.CountAllExceptDiscovery != half)
            {
                _InvalidMessage = String.Format("You should get rid of {0} cards, not {1} cards",
                    half, ResourcesLost.CountAllExceptDiscovery);
                return false;
            }

            return true;
        }
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);

            gamePlayer.Resources.SubtractCards(ResourcesLost);
            xmlGame.Bank.AddCards(ResourcesLost);

            _Message = String.Format("{0} lost {1}",
                gamePlayer.XmlPlayer.Name, ResourcesLost.ToString());
                
        }
    }
}
