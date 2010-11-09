using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.TurnPhases
{
    /// <summary>
    /// Gamephase before player rolls the dice
    /// </summary>
    [DataContract]
    public class BeforeDiceRollTurnPhase : TurnPhase
    {
        /// <summary>
        /// Before rolling dice, players may:
        /// - roll the dice (transition to RollDicePhase)
        /// - play a soldier (and subsequntly place the robber and robbing a player)
        /// - play a victorypoint development card
        /// - claim victory 
        /// </summary>
        protected override void AddAllowedActions()
        {
            _AllowedActions.Add(typeof(RollDiceTurnPhase));
            _AllowedActions.Add(typeof(PlayDevcardAction));
            _AllowedActions.Add(typeof(PlaceRobberPirateAction));
            _AllowedActions.Add(typeof(RobPlayerAction));

            // Victory can be claimed bfore dicerolling. Usefull for several reasons:
            // - VictoryPoint development cards may be played during BeforeDiceRoll
            // - Longest route may be transferred to player by volcano/LR blokage in another players' turn
            // - LA may be claimed due to soldier play
            _AllowedActions.Add(typeof(ClaimVictoryAction));

            base.AddAllowedActions();
        }

        public override bool AllowedAction(InGameAction inGameAction, XmlGame game)
        {
            return base.AllowedAction(inGameAction, game);
        }

        public override TurnPhase ProcessAction(InGameAction action, XmlGame game)
        {
            RollDiceAction rollDice = action as RollDiceAction;
            if (rollDice != null)
            {
                return new RollDiceTurnPhase();
            }
            if (AllowedAction(action, game))
            {

                action.PerformTurnAction(game);
                return this;
            }
            else
            {
                return null;
            }
        }

        public override TurnPhase Next()
        {
            return new RollDiceTurnPhase();
        }
    }
}
