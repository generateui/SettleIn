using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.User;
using SettleInCommon.Gaming;
using SettleInCommon;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions;
using SettleInCommon.Actions.Lobby;
using SettleInCommon.Actions.Result;

namespace SettleInCommon.Actions
{
    [DataContract]
    [KnownType(typeof(MessageFromServerAction))]
    [KnownType(typeof(JoinResult))]
    [KnownType(typeof(InGameAction))]
    [KnownType(typeof(LobbyAction))]
    public class GameAction
    {
        protected string _InvalidMessage = string.Empty;
        protected string _Message = string.Empty;
        protected XmlUser _User;
        private int _Sender = 2;

        [DataMember]
        public int Sender
        {
            get { return _Sender; }
            set { _Sender = value; }
        }

        [DataMember]
        public DateTime DateTime { get; set; }

        public virtual bool IsValid()
        {
            if (Sender == 0)
            {
                _InvalidMessage = "Sender cannot be null";
                return false;
            }
            
            return true;
        }


        public string InvalidMessage
        {
            get
            {
                return _InvalidMessage;
            }
        }

        public virtual string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        public virtual XmlUser User
        {
            get { return _User; }
        }

        public virtual GameAction Copy()
        {
            return (GameAction)this.MemberwiseClone();
        }
    }
}
