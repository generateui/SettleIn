using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.User;
using SettleInCommon.Actions;
using SettleInCommon.Gaming;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Actions
{
    /// <summary>
    /// This class is used when carrying out any of the 4 chat callback actions
    /// such as Receive, ReceiveWhisper, UserEnter, UserLeave <see cref="IChatCallback">
    /// IChatCallback</see> for more details
    /// </summary>
    [DataContract]
    public class GameEventArgs : EventArgs
    {
        [DataMember]
        public int Person { get; set; }

        [DataMember]
        public GameAction Action { get; set; }
    }
}
