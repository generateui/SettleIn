using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Gaming.TurnPhases;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.GamePhases
{
    /// <summary>
    /// Represnt gamephase in where every player takes turns untill a player wins the game
    /// RollDiceAction denotes RollDiceTurnPhase
    /// TradeOffer denotes TradingTurnPhase
    /// Any build action denotes BuildTurnPhase
    /// EndTurn denotes BeforeDiceRollPhase
    /// </summary>
    [DataContract]
    public class PlayTurnsGamePhase : GamePhase
    {
        /// <summary>
        /// Phase on the turn player is in
        /// </summary>
        private TurnPhase _TurnPhase = new BeforeDiceRollTurnPhase();

        public TurnPhase TurnPhase
        {
            get { return _TurnPhase; }
        }

        public override bool IsAllowed(InGameAction inGameAction, XmlGame game)
        {
            return _TurnPhase.AllowedAction(inGameAction, game);
        }

        public override void ProcessAction(InGameAction inGameAction, XmlGame game)
        {
            ClaimVictoryAction claimVictory = inGameAction as ClaimVictoryAction;
            if (claimVictory != null)
            {
                _TurnPhase.ProcessAction(inGameAction, game);
                return;
            }

            PlacementDoneAction placementDone = inGameAction as PlacementDoneAction;
            if (placementDone != null)
            {
                _TurnPhase.ProcessAction(inGameAction, game);
                return;
            }
            EndTurnAction endTurn = inGameAction as EndTurnAction;
            if (endTurn != null)
            {
                _TurnPhase = new BeforeDiceRollTurnPhase();
                endTurn.PerformTurnAction(game);
                return;
            }
            // Process the actual in the current turnphase
            TurnPhase next = _TurnPhase.ProcessAction(inGameAction, game);

            // If return phase does not match current phase, we have to switch phases and process the action
            // again
            if (_TurnPhase != next)
            {
                _TurnPhase = next; 
                _TurnPhase = _TurnPhase.ProcessAction(inGameAction, game);
            }

            // When the incoming action is not valid, check if it's valid in the next phase.
            // If so, switch phases
            if (!_TurnPhase.AllowedAction(inGameAction, game))
            {
                if (_TurnPhase.Next().AllowedAction(inGameAction, game))
                {
                    _TurnPhase = _TurnPhase.Next();
                }
            }
        }
        public override Type EndAction()
        {
            return typeof(ClaimVictoryAction);
        }
        public override GamePhase Next(XmlGame game)
        {
            return new EndedGamePhase();
        }
        public override void Start(XmlGame game)
        {
            game.ActionsQueue.Enqueue(new PlacementDoneAction() 
            { 
                Sender = 0 
            });
        }
    }
}
