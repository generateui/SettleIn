using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.User;

namespace SettleInCommon
{
    [DataContract]
    public class XmlComment
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public XmlUser Author { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public Int64 CommentID { get; set; }
    }
}
