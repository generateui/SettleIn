using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.GamePhases
{
    [DataContract]
    public class PlacementGamePhase : GamePhase
    {
        // Each player should place a town+road
        public override void Start(XmlGame game)
        {
            // Expect each player to place town/road - town/road
            int i = 0;
            bool back = false;

            // A loop going backward. Each index should be hit twice.
            // Example with 4 players: p1 - p2 - p3 - p4 - p4 - p3 - p2 - p1
            while (i > -1)
            {
                // If tournament starting rules are set, second building should be a city
                if (back && game.Settings.TournamentStart)
                {
                    // Tournamanet starting rules, add a city
                    game.ActionsQueue.Enqueue(new BuildCityAction()
                    {
                        GamePlayer = game.Players[i]
                    });
                }
                else
                {
                    // Normal starting rules, add two towns
                    game.ActionsQueue.Enqueue(new BuildTownAction()
                    {
                        GamePlayer = game.Players[i]
                    });
                }

                // This action actually might be a BuildShipAction too.
                // TODO: implement this somewhere
                game.ActionsQueue.Enqueue(new BuildRoadAction()
                {
                    GamePlayer = game.Players[i]
                });

                // if the "back" flag is set, we should decrease the counter
                if (back)
                {
                    i--;
                }
                else
                {
                    i++;
                }
                
                // flip the flag when counter reaches maximum value 
                // (maximum value equals amount of players)
                if (i == game.Players.Count)
                {
                    // next loop is walked with same maximum value
                    i--;

                    // switch flag
                    back = true;
                }
            }

            // When in tournament phase, very player may build a third road
            if (game.Settings.TournamentStart)
            {
                for (int j = 0; j < game.Players.Count; j++)
                {
                    game.ActionsQueue.Enqueue(new BuildRoadAction()
                    {
                        GamePlayer = game.Players[j]
                    });
                }
            }
        }

        protected override void AddActions()
        {
            // Players may build roads/city/town/ship
            // (only in Tournament starting rules)
            _AllowedActions.Add(typeof(BuildCityAction));

            _AllowedActions.Add(typeof(BuildTownAction));
            _AllowedActions.Add(typeof(BuildRoadAction));

            // first and second may be a ship
            _AllowedActions.Add(typeof(BuildShipAction));

            // When building, a hex may be dscovered
            _AllowedActions.Add(typeof(DiscoverHexAction));

            base.AddActions();
        }

        public override void ProcessAction(InGameAction inGameAction, XmlGame game)
        {
            // If we build a town or a city, make sure the next action (buildroad)
            // has OriginatingTownOrCity set. This is ignored for the third road
            // as required by Tournament Starting Rules.
            BuildTownAction buildTown = inGameAction as BuildTownAction;
            if (buildTown != null)
            {
                BuildRoadAction buildRoad3 = game.ActionsQueue.Peek() as BuildRoadAction;
                buildRoad3.OriginatingTownOrCity = buildTown.Location;
            }
            BuildCityAction buildCity = inGameAction as BuildCityAction;
            if (buildCity != null)
            {
                BuildRoadAction buildRoad2 = game.ActionsQueue.Peek() as BuildRoadAction;
                buildRoad2.OriginatingTownOrCity = buildCity.Location;
            }

            inGameAction.PerformTurnAction(game);

            // If the last road or ship has been built, add new gamephase action on the queue
            BuildRoadAction buildRoad = inGameAction as BuildRoadAction;
            BuildShipAction buildShip = inGameAction as BuildShipAction;
            if (buildRoad != null || buildShip != null)
            {
                if (game.ActionsQueue.Count == 0)
                {
                    game.ActionsQueue.Enqueue(new PlacementDoneAction() { Sender = 0 });
                }
                else
                {
                    // Next player is the player of the first action on the queue
                    game.PlayerOnTurn = game.GetPlayer(game.ActionsQueue.Peek().Sender);
                }

            }

        }

        public override GamePhase Next(XmlGame game)
        {
            return new PlayTurnsGamePhase();
        }

        public override Type EndAction()
        {
            return typeof(PlacementDoneAction);
        }

    }
}
