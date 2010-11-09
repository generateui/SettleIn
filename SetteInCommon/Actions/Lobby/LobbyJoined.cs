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
    public class LobbyJoinedAction : LobbyAction
    {
        [DataMember]
        public XmlUser NewPlayer { get; set; }
    }
}
