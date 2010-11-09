using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SettleInCommon.User
{
    [DataContract]
    public class XmlUser : IEquatable<int>, IEquatable<XmlUser>, IEqualityComparer<XmlUser>
    {
        public XmlUser(string name)
        {
            this.Name = name;
        }
        public XmlUser() { }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int GamesWon { get; set; }

        [DataMember]
        public int GamesPlayed { get; set; }

        [DataMember]
        public int Rating { get; set; }

        public override bool Equals(object obj)
        {
            XmlUser user = obj as XmlUser;
            if (user != null)
            {
                return user.ID == this.ID;
            }
            return false;
        }

        [DataMember]
        public XmlUserDetails Details { get; set; }

        public XmlUser Copy()
        {
            XmlUser result = new XmlUser();

            if (Details!=null) result.Details = Details.Copy();
            result.GamesPlayed = GamesPlayed;
            result.GamesWon = GamesWon;
            result.ID = ID;
            result.Name = Name;
            result.Rating = Rating;

            return result;

        }

        public override int GetHashCode()
        {
            return ID;
        }

        #region IEquatable<int> Members

        public bool Equals(int other)
        {
            return ID == other;
        }

        #endregion

        #region IEquatable<XmlUser> Members

        public bool Equals(XmlUser other)
        {
            return other.ID == ID;
        }

        #endregion

        #region IEqualityComparer<XmlUser> Members

        public bool Equals(XmlUser x, XmlUser y)
        {
            return y.ID == x.ID;
        }

        public int GetHashCode(XmlUser obj)
        {
            return ID;
        }

        #endregion
    }
}
