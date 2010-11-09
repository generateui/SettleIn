using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.User;

namespace SettleInCommon.Actions.Lobby
{
    [DataContract]
    [KnownType(typeof(GameCreatedAction))]
    [KnownType(typeof(EnterLobbyAction))]
    [KnownType(typeof(JoinGameAction))]
    [KnownType(typeof(GameJoinedAction))]
    [KnownType(typeof(LobbyChatAction))]
    [KnownType(typeof(TryCreateGameAction))]
    [KnownType(typeof(LobbyJoinedAction))]
    [KnownType(typeof(UserDisconnectedAction))]
    [KnownType(typeof(NewGameChangedAction))]
    [KnownType(typeof(UserLeftLobbyAction))]
    public abstract class LobbyAction : GameAction
    {
    }
}
