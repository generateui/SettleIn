using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Gaming.GamePhases;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.TurnPhases
{
    [DataContract]
    public class BuildTurnPhase : TurnPhase
    {
        private TradingTurnPhase _TradingPhase;

        public TradingTurnPhase TradingPhase
        {
            get { return _TradingPhase; }
            set { _TradingPhase = value; }
        }

        public BuildTurnPhase(TradingTurnPhase tradingPhase)
            : base()
        {
            _TradingPhase = tradingPhase;
        }

        protected override void AddAllowedActions()
        {
            _AllowedActions.Add(typeof(ClaimVictoryAction));

            // Player should b able to build stuff
            _AllowedActions.Add(typeof(BuildCityAction));
            _AllowedActions.Add(typeof(BuildTownAction));
            _AllowedActions.Add(typeof(BuildShipAction));
            _AllowedActions.Add(typeof(BuildRoadAction));

            // Moving ships can only be done in building phase
            _AllowedActions.Add(typeof(MoveShipAction));

            // Playing devcards can be done in buildphase as well as BeforeDiceRoll (soldier+victorypoint)
            _AllowedActions.Add(typeof(BuyDevcardAction));
            _AllowedActions.Add(typeof(PlayDevcardAction));
            _AllowedActions.Add(typeof(PlaceRobberPirateAction));
            _AllowedActions.Add(typeof(RobPlayerAction));

            // Ending a turn is only allowed in buildphase
            _AllowedActions.Add(typeof(EndTurnAction));

            // Changes to road/army can only happen in build phase
            _AllowedActions.Add(typeof(LargestArmyAchievedAction));
            _AllowedActions.Add(typeof(LongestRouteAchievedAction));
            _AllowedActions.Add(typeof(TradeRoutesChangedAction));
            
            // Swapping chits is only allowed in buildphase
            _AllowedActions.Add(typeof(SwapChitAction));

            // Discovering hexes may happen in placement gamephase as well as in build turnphase
            _AllowedActions.Add(typeof(DiscoverHexAction));

            // Trading with players is done in TradePhase, bank trades are allowed in buildphase.
            _AllowedActions.Add(typeof(TradeBankAction));
            
            base.AddAllowedActions();
        }

        public override bool AllowedAction(InGameAction inGameAction, XmlGame game)
        {
            if (!base.AllowedAction(inGameAction, game))
                return false;

            // Check if we are allowed to trade
            TradeOfferAction tradeOffer = inGameAction as TradeOfferAction;
            if (tradeOffer != null && !game.Settings.TradingAfterBuilding)
                return false;

            // Default on returning true
            return true;
        }

        public override TurnPhase ProcessAction(InGameAction action, XmlGame game)
        {
            if (AllowedAction(action, game))
            {
                action.PerformTurnAction(game);

                EndTurnAction endTurn = action as EndTurnAction;
                if (endTurn != null)
                {
                    return new BeforeDiceRollTurnPhase();
                }
                
                return this;
            }
            else
            {
                // Check if we are allowed to trade
                TradeOfferAction tradeOffer = action as TradeOfferAction;
                if (tradeOffer != null)
                {
                    // Only when game setting allows it
                    if (game.Settings.TradingAfterBuilding)
                    {
                        return _TradingPhase;
                    }
                    return null;
                }
                return null;
            }


        }

        public override TurnPhase Next()
        {
            return new BeforeDiceRollTurnPhase();
        }
    }
}
