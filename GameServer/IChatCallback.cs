using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;

using SettleInCommon.Actions;
using SettleInCommon.Gaming;
using SettleInCommon.User;

namespace GameServer
{
    /// <summary>
    /// Receive : receive a globally broadcasted action performed by the sender
    /// </summary>
    public interface IChatCallback
    {
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.LobbyAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.NewGameChangedAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.JoinGameAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.UserDisconnectedAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.UserLeftLobbyAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.LobbyJoinedAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.GameCreatedAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.EnterLobbyAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.GameJoinedAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.LobbyChatAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.Lobby.TryCreateGameAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.InGameAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.GameChatAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.LooseCardsAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.PlayerLeftAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.PlayerLostConnection))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.SpectatorLeftAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.PlayerReconnectedAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.SpectatorJoinedAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.PickGoldAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.AcceptOfferAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.TurnAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.TradeOfferAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.InGame.CounterTradeOfferAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.TradePlayerAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.PlaceRobberPirateAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.BuildTownAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.PlayDevcardAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.RobPlayerAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.BuildRoadAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.BuildShipAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.BuyDevcardAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.ClaimVictoryAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.EndTurnAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.MoveShipAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.BuildCityAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.RollVolcanoDiceAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.RollDiceAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.SwapChitAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.TurnActions.TradeBankAction))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SettleInCommon.Actions.MessageFromServerAction))]
        [OperationContractAttribute(IsOneWay = true)]
        void Receive(GameAction action);
    }
}
