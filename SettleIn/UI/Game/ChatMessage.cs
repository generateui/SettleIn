using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.User;

namespace SettleIn.UI.Game
{
    public class ChatMessage
    {
        private XmlUser _User;
        private string _Message = string.Empty;
        private DateTime _DateTime;

        public XmlUser User
        {
            get { return _User; }
            set { _User = value; }
        }

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public DateTime DateTime
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }
    }
}
