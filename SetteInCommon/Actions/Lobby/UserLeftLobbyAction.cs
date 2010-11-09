using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Actions.Lobby
{
    /// <summary>
    /// Occurs when a connected player in lobby, while _not_ playing a game
    /// leaves
    /// </summary>
    [DataContract]
    public class UserLeftLobbyAction : LobbyAction
    {
       
    }
}
