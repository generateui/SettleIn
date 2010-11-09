using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Actions;
using SettleInCommon;
using SettleInCommon.User;

namespace SettleInCommon.Actions.Result
{
    [DataContract]
    public class JoinResult
    {
        [DataMember]
        public XmlUser User { get; set; }

        [DataMember]
        public MessageFromServerAction FailMessage { get; set; }

        [DataMember]
        public XmlLobbyState LobbyState { get; set; }
    }
}
