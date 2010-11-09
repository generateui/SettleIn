using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.Lobby
{
    [DataContract]
    public class TryCreateGameAction : LobbyAction
    {
        [DataMember]
        public XmlGameSettings GameSettings { get; set; }
    }
}
