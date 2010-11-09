using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.User;

namespace SettleInCommon.Gaming
{
    [DataContract]
    public class XmlPlayedGame
    {
        [DataMember]
        public XmlUser[] Players;

        [DataMember]
        public XmlBoard Board;

        [DataMember]
        public string Name;

        [DataMember]
        public XmlGameResult[] Result { get; set; }

        [DataMember]
        public int ID {get; set;}
    }
}
