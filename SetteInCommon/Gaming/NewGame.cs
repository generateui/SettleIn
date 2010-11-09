using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SettleInCommon.Gaming
{
    [DataContract]
    public class NewGame
    {
        [DataMember]
        public XmlGameSettings Settings { get; set; }

        [DataMember]
        public int UserID { get; set; }
    }
}
