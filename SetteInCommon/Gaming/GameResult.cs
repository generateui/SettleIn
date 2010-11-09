using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

using SettleInCommon.User;

namespace SettleInCommon.Gaming
{
    [DataContract]
    public class XmlGameResult
    {
        [DataMember]
        public XmlUser Player { get; set; }
        
        [DataMember]
        public int OldRating { get; set; }
        
        [DataMember]
        public int GamePoints { get; set; }
        
        [DataMember]
        public int LadderPointsFromGame { get; set; }

        [DataMember]
        public bool Winner { get; set; }

        [DataMember]
        public Guid BoardID { get; set; }
    }
}
