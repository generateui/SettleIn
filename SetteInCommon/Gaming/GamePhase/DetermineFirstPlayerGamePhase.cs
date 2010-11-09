using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Board;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.GamePhases
{
    /// <summary>
    /// Each player rolls the dice once. If a highroller emerges, he wins.
    /// If no highroller emerges, all highrollers roll the dice again in a new 
    /// 'phase' ad infitum untill a single highroller emerges.
    /// </summary>
    [DataContract]
    public class DetermineFirstPlayerGamePhase : GamePhase
    {
        protected override void AddActions()
        {
            // an endturnaction is sent by the server
            _AllowedActions.Add(typeof(EndTurnAction));

            // Rolldiceaction is initiated by the player on dice click
            _AllowedActions.Add(typeof(RollDiceAction));

            base.AddActions();
        }
        /// <summary>
        /// Creates gamequeue for each player
        /// </summary>
        /// <param name="inGameAction"></param>
        public override void ProcessAction(InGameAction inGameAction, XmlGame game)
        {
            inGameAction.PerformTurnAction(game);
            
            RollDiceAction rollDice = inGameAction as RollDiceAction;
            if (rollDice != null)
            {
                // Check if a phase has ended. If the queue is empty, every player has rolled the dice.
                if (game.ActionsQueue.Count() == 0)
                {
                    // Make a list of rolls in this round
                    List<RollDiceAction> rolledDices = game.GameLog.GetCurrentRoundRolls(game);

                    // highroll dice number
                    int highRoll = (from rd in rolledDices select rd.Dice).Max();

                    // When starting player is not determined yet, repeat dice roll between winners until 
                    // winner is detemined
                    if (game.GameLog.FirstPlayerIsDetermined(game))
                    {
                        // We have a starting player
                        int winnerID = rolledDices.Where(rd => rd.Dice == highRoll)
                                                                          .First()
                                                                          .Sender;

                        game.ActionsQueue.Enqueue(new StartingPlayerDeterminedAction()
                        {
                            // The starter of the placement/portplacement/turnactionsgamephase
                            PlayerID = winnerID,
                            // winning dice
                            DiceRoll = highRoll,
                            // Server will send this message
                            Sender = 0
                        });
                        return;
                    }
                    else
                    {
                        // Starting player is not determined. Notify players and update Game object
                        game.ActionsQueue.Enqueue(new RolledSameAction()
                        {
                            // Pass on the highest diceroll
                            HighRoll = highRoll,
                            // Server says dice rolled the same
                            Sender = 0
                        });

                        // Enqueue each highroller 
                        foreach (RollDiceAction sameRoll in rolledDices.Where(rd => rd.Dice == highRoll))
                        {
                            game.ActionsQueue.Enqueue(new RollDiceAction() { GamePlayer = sameRoll.GamePlayer });
                        }

                        // First player is on turn
                        game.PlayerOnTurn = game.ActionsQueue.ElementAt(1).GamePlayer;
                        return;
                    }
                }

                // Next player should be the player next on the queue
                game.PlayerOnTurn = game.GetPlayer(game.ActionsQueue
                    .OfType<RollDiceAction>()
                    .First()
                    .Sender);
                
            }
        }

        public override void Start(XmlGame game)
        {
            // expect each player to roll at least once (first phase: everyone rolls once)
            foreach (GamePlayer player in game.Players)
            {
                game.ActionsQueue.Enqueue(new RollDiceAction()
                {
                    GamePlayer = player
                });
            }
        }

        public override Type EndAction()
        {
            return typeof(StartingPlayerDeterminedAction);
        }

        public override GamePhase Next(XmlGame game)
        {
            // Determine if we should skip placing ports
            // randomports are assigned at start using the port lists on each territory.
            // The remaining ports are placed in the placement phase
            List<EPortType> allPorts = new List<EPortType>();
            foreach (Territory t in game.Board.Territories)
            {
                foreach (EPortType p in t.PortList)
                {
                    allPorts.Add(p);
                }
            }
            if (allPorts.Count == 0)
            {
                // We do not have any ports to set, skip to placement phase
                return new PlacementGamePhase();
            }
            else
            {
                // players should place ports
                return new PlacePortGamePhase();
            }
        }
    }
}
