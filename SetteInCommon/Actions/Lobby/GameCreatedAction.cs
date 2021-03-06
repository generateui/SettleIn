﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.Lobby
{
    [DataContract]
    public class GameCreatedAction : LobbyAction
    {
        [DataMember]
        public XmlGameSettings Game { get; set; }

        [DataMember]
        public int ID { get; set; }
    }
}
