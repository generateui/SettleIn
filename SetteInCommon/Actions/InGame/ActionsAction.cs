using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;
using SettleInCommon.Actions.TurnActions;

namespace  SettleInCommon.Actions.InGame
{
    /// <summary>
    /// Represents a list of game commands to be executed consecutively
    /// </summary>
    [DataContract]
    [KnownType(typeof(AcceptOfferAction))]
    [KnownType(typeof(CounterTradeOfferAction))]
    [KnownType(typeof(GameChatAction))]
    [KnownType(typeof(LooseCardsAction))]
    [KnownType(typeof(PickGoldAction))]
    [KnownType(typeof(PlayerReconnectedAction))]
    [KnownType(typeof(PlayerLeftAction))]
    [KnownType(typeof(PlayerLostConnection))]
    [KnownType(typeof(SpectatorLeftAction))]
    [KnownType(typeof(SpectatorLostConnection))]
    [KnownType(typeof(SpectatorJoinedAction))]
    [KnownType(typeof(TurnAction))]
    [KnownType(typeof(BuildRoadAction))]
    [KnownType(typeof(BuildShipAction))]
    [KnownType(typeof(BuyDevcardAction))]
    [KnownType(typeof(ClaimVictoryAction))]
    [KnownType(typeof(EndTurnAction))]
    [KnownType(typeof(MoveShipAction))]
    [KnownType(typeof(TradeOfferAction))]
    [KnownType(typeof(BuildCityAction))]
    [KnownType(typeof(PlaceRobberPirateAction))]
    [KnownType(typeof(BuildTownAction))]
    [KnownType(typeof(PlacePortAction))]
    [KnownType(typeof(PlayDevcardAction))]
    [KnownType(typeof(RobPlayerAction))]
    [KnownType(typeof(RollDiceAction))]
    [KnownType(typeof(RollVolcanoDiceAction))]
    [KnownType(typeof(SwapChitAction))]
    [KnownType(typeof(TradeBankAction))]
    [KnownType(typeof(TradePlayerAction))]
    public class InGameActionsAction : InGameAction
    {
        [DataMember]
        List<InGameAction> Actions { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid()) return false;

            if (Actions == null)
            {
                _InvalidMessage = "Actions cannot be null";
                return false;
            }

            // we can't contain ourself for now, not needed yet
            if (Actions.OfType<InGameActionsAction>().Count() > 0)
            {
                _InvalidMessage = "We cannot contain another list ourselves";
                return false;
            }

            // all containing turns must be valid also
            XmlGame gameCopy = game.Copy();
            foreach (InGameAction action in Actions)
            {
                if (!action.IsValid(game))
                {
                    _InvalidMessage = String.Format("A game in the list of games failed to validate:/r/n{0}", 
                        action.InvalidMessage);
                    return false;
                }
                // perform the action so the state of the game gets updated for 
                // the next action
                action.PerformTurnAction(gameCopy);
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            foreach (InGameAction action in Actions)
            {
                action.PerformTurnAction(xmlGame);
            }
        } 
    }
}
