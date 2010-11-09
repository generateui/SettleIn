using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Actions.TurnActions;

namespace SettleInCommon.Gaming
{
    [DataContract]
    public class Turn
    {
        [DataMember]
        public List<TurnAction> TurnActions { get; set; }
    }
}
