using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SettleInCommon.User
{
    [DataContract]
    public class XmlUserCredentials
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Password { get; set; }

        public XmlUserCredentials(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }

        public XmlUserCredentials() { }
    }
}
