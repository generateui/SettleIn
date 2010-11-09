using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Gaming;
using System.Runtime.Serialization;

namespace SettleInCommon.Actions.InGame
{
    /// <summary>
    /// Occurs when at start, two or more players rolled equal to highest dice
    /// </summary>
    [DataContract]
    public class RolledSameAction : InGameAction
    {
        [DataMember]
        public List<int> RolledSamePlayers { get; set; }

        [DataMember]
        public int HighRoll { get; set; }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            RolledSamePlayers = xmlGame.RolledSame(xmlGame.GameLog.GetCurrentRoundRolls(xmlGame), HighRoll);
            int i = 0;
            StringBuilder sb = new StringBuilder();

            // Create message
            foreach (int playedID in RolledSamePlayers)
            {
                i++;
                sb.Append(xmlGame.GetPlayer(playedID).XmlPlayer.Name);
                string divider = i == RolledSamePlayers.Count ? " and " : ", ";
                sb.Append(divider);
            }

            _Message = String.Format("{0} rolled a {1}, so they have to roll again",
                sb.ToString(), HighRoll.ToString());
        }

    }
}
