using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Actions.InGame;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.TurnPhases
{
    [DataContract]
    public class TradingTurnPhase : TurnPhase
    {
        // Keep track of the buildphase, so we can switch back
        private BuildTurnPhase _BuildPhase;

        public override bool AllowedAction(InGameAction inGameAction, XmlGame game)
        {
            return base.AllowedAction(inGameAction, game);
        }

        public TradingTurnPhase()
        {
            _BuildPhase = new BuildTurnPhase(this);
        }

        protected override void AddAllowedActions()
        {
            // Player on turn
            _AllowedActions.Add(typeof(TradeOfferAction));
            _AllowedActions.Add(typeof(EndTradeAction));
            _AllowedActions.Add(typeof(TradePlayerAction));

            // Other players (responses)
            _AllowedActions.Add(typeof(AcceptOfferAction));
            _AllowedActions.Add(typeof(CounterTradeOfferAction));
            _AllowedActions.Add(typeof(RejectOfferAction));

            base.AddAllowedActions();
        }

        public override TurnPhase ProcessAction(InGameAction action, XmlGame game)
        {
            // If the action is allowed to be executed in this phase, do it
            if (AllowedAction(action, game))
            {
                action.PerformTurnAction(game);
                return this;
            }
            else
            // If action is not allowed to execute, check if buildphase allows it. If so, move to
            // buildphase
            {
                if (_BuildPhase.AllowedAction(action, game))
                {
                    return _BuildPhase;
                }
                throw new Exception("whoa");
            }
        }

        public override TurnPhase Next()
        {
            return _BuildPhase;
        }

    }
}
