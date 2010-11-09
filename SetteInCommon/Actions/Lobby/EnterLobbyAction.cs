using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using SettleInCommon.User;

namespace SettleInCommon.Actions.Lobby
{
    [DataContract]
    public class EnterLobbyAction : LobbyAction
    {
        [DataMember]
        public XmlUser UserEnteringLobby { get; set; }
    }
}
