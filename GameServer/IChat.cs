using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;

using SettleInCommon;
using SettleInCommon.Actions;
using SettleInCommon.Actions.Result;
using SettleInCommon.Gaming;
using SettleInCommon.User;

namespace GameServer
{
    /// <summary>
    /// This interface provides 4 methods that may be used in order to clients
    /// to carry out specific actions in the chat room. This interface
    /// expects the clients that implement this interface to be able also support
    /// a callback of type <see cref="IChatCallback">IChatCallback</see>
    /// 
    /// There are methods for
    /// 
    /// Say : send a globally broadcasted message
    /// Whisper : send a personal message
    /// Join : join the chat room
    /// Leave : leave the chat room
    /// </summary>
    [ServiceContract(
        SessionMode = SessionMode.Required, 
        CallbackContract=typeof(IChatCallback))]
    public interface IChat
    {
        [OperationContract(
            IsOneWay = false, 
            IsInitiating = true, 
            IsTerminating = false, 
            AsyncPattern = false)]
        JoinResult Join(XmlUserCredentials credentials);

        [OperationContract(
            IsOneWay = true,
            IsInitiating = false)] 
        void Leave();

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
        [OperationContract(
            IsOneWay = true, 
            IsInitiating = false)] 
        void Say(GameAction action);
    }
}
