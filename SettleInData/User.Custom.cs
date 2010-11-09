using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.User;

namespace SettleInData
{
    public partial class User
    {
        public XmlUser BuildXmlUser()
        {
            XmlUser xmlUser = new XmlUser(_Name);
            
            //xmlUser.GamesPlayed = this.
            xmlUser.ID = _ID;
            //xmlUser.Rating = (int)_Rating;
            //xmlUser.GamesWon = u

            return xmlUser;
        }
    }
}
