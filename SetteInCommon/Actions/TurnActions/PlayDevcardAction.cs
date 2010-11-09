using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming.DevCards;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Board;

namespace SettleInCommon.Actions.TurnActions
{
    [KnownType(typeof(Soldier))]
    [KnownType(typeof(VictoryPoint))]
    [KnownType(typeof(RoadBuilding))]
    [KnownType(typeof(Monopoly))]
    [KnownType(typeof(YearOfPlenty))]
    [KnownType(typeof(UnknownDevelopmentCard))]
    [DataContract]
    public class PlayDevcardAction : TurnAction
    {
        [DataMember]
        public DevelopmentCard DevCard { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            if (DevCard == null)
            {
                _InvalidMessage = "Devcard cannot be null";
                return false;
            }

            GamePlayer player = game.GetPlayer(Sender);
            if (!DevCard.IsPlayable)
            {
                _InvalidMessage = "You already played a non-victorypoint devcard this turn, or your devcards are bought this turn";
                return false;
            }

            if (!DevCard.IsPlayable)
            {
                _InvalidMessage = "Development card is not playable. Wait one turn";
            }

            if (!DevCard.IsValid(game))
            {
                _InvalidMessage = "Development card is not valid";
                return false;
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);
            if (DevCard is Soldier)
            {
                xmlGame.ActionsQueue.Enqueue(new PlaceRobberPirateAction()
                {
                    GamePlayer = gamePlayer
                });
            }

            DevelopmentCard devCardToRemove=null;

            //remove devcard from players hand
            foreach (DevelopmentCard devcard in gamePlayer.DevCards)
            {
                Type t = this.DevCard.GetType();
                if (devcard.ID == DevCard.ID)
                {
                    devCardToRemove = devcard;
                    break;
                }
            }

            gamePlayer.PlayedDevcards.Add(devCardToRemove);
            gamePlayer.DevCards.Remove(devCardToRemove);

            // Execute devcard
            devCardToRemove.Execute(xmlGame, gamePlayer);

            // Mark all other cards needing to wait one turn as unplayable, if we play a non-unique dvcard
            if (devCardToRemove.WaitOneTurn)
            {
                foreach (DevelopmentCard dc in gamePlayer.DevCards)
                    dc.IsPlayable = !dc.WaitOneTurn;
            }

            _Message = DevCard.Message;

            base.PerformTurnAction(xmlGame);
        }
    }
}
