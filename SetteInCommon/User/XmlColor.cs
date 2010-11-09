using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SettleInCommon.User
{
    [DataContract]
    public class XmlColor
    {
        [DataMember]
        public byte R { get; set; }

        [DataMember]
        public byte G { get; set; }
        
        [DataMember]
        public byte B { get; set; }

        public XmlColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
