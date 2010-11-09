using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Gaming.DevCards;

namespace SettleInCommon.Actions.TurnActions
{
    [DataContract]
    public class BuyDevcardAction : TurnAction
    {
        // we need to specify resources since we can pay with discoveries
        [DataMember]
        public ResourceList Resources { get; set; }

        [DataMember]
        public DevelopmentCard DevCard { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            // we need resources
            if (Resources == null) return false;

            // ...and to devcard too.
            if (game.DevCards.Count == 0)
            {
                _InvalidMessage = "Development cards are all gone!";
                return false;
            }

            // we need just three resources
            if (Resources.CountAll != 3) return false;

            // three or more discoveries is OK
            if (Resources.Discoveries > 2) return true;
            
            GamePlayer player = game.GetPlayer(base.Sender);

            if (!player.CanBuildRoad(game, game.Board))
            {
                _InvalidMessage = "Player cannot build the road";
                return false;
            }

            if (!player.Resources.HasAtLeast(Resources))
            {
                _InvalidMessage = "Player does not have given resources";
                return false;
            }

            if (!player.CanPayDevcard)
            {
                _InvalidMessage = "Player is not able to pay for development card";
                return false;
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);
            
            // Perform  resources administration
            gamePlayer.Resources.SubtractCards(Resources);
            xmlGame.Bank.AddCards(Resources);

            // Player should wait a turn before able to play new devcard
            DevCard.IsPlayable = !DevCard.WaitOneTurn;
            DevCard.TurnBought = xmlGame.CurrentTurn;
            
            // Administer devcards 
            gamePlayer.DevCards.Add(DevCard);
            xmlGame.DevCards.Remove(DevCard);

            _Message = String.Format("{0} bought a development card",
                gamePlayer.XmlPlayer.Name);

            base.PerformTurnAction(xmlGame);
        }

        public override void NullifyData()
        {
            DevCard = null;
        }


    }
}
