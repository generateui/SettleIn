using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.InGame;
using System.Runtime.Serialization;
using SettleInCommon.Actions.TurnActions;

namespace SettleInCommon.Gaming.GamePhases
{
    /// <summary>
    /// Players are happily chitchatting in the lobby
    /// </summary>
    [DataContract]
    public class LobbyGamePhase : GamePhase
    {
        public override GamePhase Next(XmlGame game)
        {
            return new DetermineFirstPlayerGamePhase();
        }

        protected override void AddActions()
        {
            _AllowedActions.Add(typeof(HostStartsGameAction));
            
            base.AddActions();
        }

        public override void ProcessAction(InGameAction inGameAction, XmlGame game)
        {
            /*
            HostStartsGameAction hostStarts = inGameAction as HostStartsGameAction;
            if (hostStarts != null)
            {
                game.ActionsQueue.Enqueue(new DetermineFirstPlayerGamePhase() { Sender = 0 });
            }
             */
            inGameAction.PerformTurnAction(game);
        }
        public override Type EndAction()
        {
            return typeof(StartGameAction);
        }
        public override void Start(XmlGame game)
        {
        }
    }
}
