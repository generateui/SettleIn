using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Actions.InGame;
using System.Runtime.Serialization;
using SettleInCommon.Board.Hexes;

namespace SettleInCommon.Gaming.TurnPhases
{
    [DataContract]
    public class RollDiceTurnPhase : TurnPhase
    {
        /// <summary>
        /// Player currently on turn may roll the dice
        /// </summary>
        protected override void AddAllowedActions()
        {
            // When a 7 is rolled, player should move robber/pirate and optionally rob a player
            _AllowedActions.Add(typeof(RobPlayerAction));
            _AllowedActions.Add(typeof(PlaceRobberPirateAction));
            _AllowedActions.Add(typeof(LooseCardsAction));
            _AllowedActions.Add(typeof(PickGoldAction));

            // Rolling a volcano dice is allowed
            _AllowedActions.Add(typeof(RollVolcanoDiceAction));
            // Subsequently, trade routes and longest routes may change when a town/city explodes
            _AllowedActions.Add(typeof(TradeRoutesChangedAction));
            _AllowedActions.Add(typeof(LongestRouteAchievedAction));

            _AllowedActions.Add(typeof(RollDiceAction));

            base.AddAllowedActions();
        }

        public override TurnPhase Next()
        {
            return new TradingTurnPhase();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public override TurnPhase ProcessAction(InGameAction action, XmlGame game)
        {
            if (AllowedAction(action, game))
            {
                RollDiceAction rollDice = action as RollDiceAction;
                if (rollDice != null)
                {
                    rollDice.PerformTurnAction(game);

                    // When a volcano is rolled, expect player to roll dice for volcano
                    /*
                    if (rollDice.HexesAffected.OfType<VolcanoHex>().Count() > 0)
                    {
                        game.ActionsQueue.Enqueue(new RollVolcanoDiceAction()
                        {
                            GamePlayer = rollDice.GamePlayer,
                            VolcanosRolled = rollDice.HexesAffected.OfType<VolcanoHex>().ToList<VolcanoHex>()
                        });

                        // We expect another action, return current phase
                        return this;
                    }
                     */

                    // When a 7 is rolled, enqueue every player required to loose cards to do so
                    if (rollDice.Dice == 7)
                    {
                        // Add each player required to loose cards to the queue
                        foreach (int i in rollDice.LooserPlayers)
                        {
                            game.ActionsQueue.Enqueue(new LooseCardsAction()
                            {
                                GamePlayer = game.GetPlayer(i)
                            });
                        }

                        // Expect player to place robber/pirate and rob a player
                        game.ActionsQueue.Enqueue(new PlaceRobberPirateAction() { GamePlayer = action.GamePlayer });
                        game.ActionsQueue.Enqueue(new RobPlayerAction() { GamePlayer = action.GamePlayer });

                        // We have actions to be done, we should stay in this phase
                        return this;
                    }

                    // Any other number has been rolled. 
                    // Proceed to trading phase
                    if (game.ActionsQueue.Count == 0)
                    {
                        return new TradingTurnPhase();
                    }
                    else
                    {
                        return this;
                    }
                }

                RobPlayerAction robPlayer = action as RobPlayerAction;
                if (robPlayer != null)
                {
                    robPlayer.PerformTurnAction(game);

                    // When finished robbing, advance phase to trading
                    return this;
                }
                // perform the action
                action.PerformTurnAction(game);
                EndTurnAction endTurn = action as EndTurnAction;
                if (endTurn != null)
                {
                    return new BuildTurnPhase(null);
                }

                // Return current state
                return this;
            }
            else
            // Action is not allowed in rollDice phase. Check if it is allowed in subsequent phases, 
            // then return that phase
            {
                TradingTurnPhase trading = new TradingTurnPhase();
                BuildTurnPhase building = new BuildTurnPhase(trading);
                if (trading.AllowedAction(action, game))
                {
                    return trading;
                }
                if (building.AllowedAction(action, game))
                {
                    return building;
                }
                return null;
            }
        }
    }
}
