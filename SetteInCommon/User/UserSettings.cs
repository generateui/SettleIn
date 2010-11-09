using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SettleInCommon.User
{
    [DataContract]
    public class UserSettings
    {
        [DataMember]
        public XmlColor FirstColor { get; set; }
        
        [DataMember]
        public XmlColor SecondColor { get; set; }
    }
}
