using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using SettleInCommon;

using SettleInCommon.User;

namespace SettleInCommon.Gaming
{
    [DataContract]
    public class XmlChatItem
    {
        [DataMember]
        public XmlUser User { get; set; }
        
        [DataMember]
        public DateTime DateTime { get; set; }
        
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public EChatItemType Type { get; set; }

        public XmlChatItem() { }

        public XmlChatItem Copy()
        {
            XmlChatItem result = new XmlChatItem();
            
            result.DateTime = DateTime;
            result.Message = Message;
            result.Type = Type;
            result.User = User;
            
            return result;
        }
    }
}
