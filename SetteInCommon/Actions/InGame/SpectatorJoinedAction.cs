using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.User;

namespace SettleInCommon.Actions.InGame
{
    [DataContract]
    public class SpectatorJoinedAction : InGameAction
    {
        [DataMember]
        public XmlUser Spectator { get; set; }
    }
}
