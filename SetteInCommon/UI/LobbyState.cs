using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.User;
using SettleInCommon.Gaming;

namespace SettleInCommon
{
    [DataContract]
    public class XmlLobbyState
    {
        [DataMember]
        public List<XmlGame> Games { get; set; }

        [DataMember]
        public XmlChatLog LobbyChat { get; set; }

        [DataMember]
        public List<XmlUser> Users { get; set; }

        [DataMember]
        public XmlUserDetails ConnectedUser { get; set; }
    }
}
