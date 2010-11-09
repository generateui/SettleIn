using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming
{
    [DataContract]
    public class XmlChatLog
    {
        private List<XmlChatItem> _Chats = new List<XmlChatItem>();

        //Maximum number of history chat messages to send to client when connecting
        private const int _MaxLobbyChatItems = 10;

        [DataMember]
        public List<XmlChatItem> Items
        {
            get
            {
                return _Chats;
            }
            set
            {
                _Chats = value;
            }

        }

        public XmlChatItem this[int i]
        {
            get
            {
                return _Chats[i];
            }
        }

        public void Say(XmlChatItem item)
        {
            _Chats.Add(item);
        }

        public int MessagesCount
        {
            get
            {
                return _Chats.Count;
            }
        }

        /// <summary>
        /// Returns a list of last messageCount messages.
        /// </summary>
        /// <param name="messagesCount">amount of messages</param>
        /// <returns></returns>
        public XmlChatLog CopyMostRecentMessages(int messagesCount)
        {
            XmlChatLog log = new XmlChatLog();
            if (_Chats.Count > 10)
            {
                for (int i = messagesCount - 1;
                    i > messagesCount - _MaxLobbyChatItems;
                    i++)
                {
                    log.Say(_Chats[i]);
                }
            }
            return log;
        }

        public XmlChatLog Copy()
        {
            XmlChatLog result = new XmlChatLog();

            foreach (XmlChatItem item in Items)
                result.Items.Add(item.Copy());

            return result;
        }
    }
}
