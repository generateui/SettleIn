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
    /// Each player places a port, starting by the player determined in the DetermineFirstPlayer phase
    /// A port may belong to a territory, thus placement of a port to a certain territory must be enforced
    /// </summary>
    [DataContract]
    public class PlacePortGamePhase : GamePhase
    {
        public override void ProcessAction(InGameAction inGameAction, XmlGame game)
        {
            inGameAction.PerformTurnAction(game);

            if (game.ActionsQueue.Count == 0)
            {
                // Notify we want to start the placement phase
                game.ActionsQueue.Enqueue(new PortsPlacedAction() { Sender = 0 });
            }
            else
            {
                // Move to the next player
                game.PlayerOnTurn = game.NextPlayer;
            }
        }

        protected override void AddActions()
        {
            _AllowedActions.Add(typeof(PlacePortAction));

            // Call base class method to add base class allowed actions
            base.AddActions();
        }
        public override GamePhase Next(XmlGame game)
        {
            return new PlacementGamePhase();
        }

        public override bool IsAllowed(InGameAction inGameAction, XmlGame game)
        {
            if (!base.IsAllowed(inGameAction, game)) return false;

            // When a placeport action is preformed, the action should be originating
            // from the expected player on the actionsqueue
            PlacePortAction placePort = inGameAction as PlacePortAction;
            if (placePort != null)
            {
                if (placePort.Sender != game.ActionsQueue.Peek().Sender)
                {
                    return false;
                }
            }

            return true;
        }

        public override Type EndAction()
        {
            return typeof(PortsPlacedAction);
        }

        /// <summary>
        /// Winner of the diceroll contest starts placing a port
        /// </summary>
        /// <param name="game"></param>
        /// <returns>First player to place a port</returns>
        public override void Start(XmlGame game)
        {
            int portCount = 0;

            foreach (Territory t in game.Board.Territories)
            {
                foreach (EPortType port in t.PortList)
                {
                    game.ActionsQueue.Enqueue(new PlacePortAction()
                    {
                        // Placing ports goes chronologically starting with the winner.
                        // The first player always has the advantage:
                        // - For example with 5 ports and 4 players, first player may place twice
                        //   while the rest only once.
                        // - First player may plac first, conveniently placing port alongside
                        // - Since port stack is open, first player placing last port is 100% certain
                        //   known port
                        GamePlayer = game.Players[portCount % game.Players.Count],
                        // pass territoryID such that player knows to expect possible port locations
                        TerritoryID = t.ID
                    });

                    portCount++;
                }
            }
        }
    }
}
