using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Actions.Lobby
{
    [DataContract]
    public class UserDisconnectedAction : LobbyAction
    {
        [DataMember]
        public int UserID { get; set; }
    }
}
