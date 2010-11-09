using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.InGame
{
    /// <summary>
    /// Occurs when a starting player has been determined using dices
    /// </summary>
    [DataContract]
    public class StartingPlayerDeterminedAction : InGameAction
    {
        [DataMember]
        public int PlayerID { get; set; }

        [DataMember]
        public int DiceRoll { get; set; }

        /// Reorder players in consecutive order. First player becomes index 0, 
        /// players after that follow up by index number
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            // Check if we need to reorder the list 
            if (xmlGame.Players[0] != xmlGame.GetPlayer(PlayerID))
            {
                // Make a new list
                List<GamePlayer> newList = new List<GamePlayer>();

                // Get index of winner
                int winnerIndex = xmlGame.Players.IndexOf(xmlGame.GetPlayer(PlayerID));

                for (int i = 0; i < xmlGame.Players.Count; i++)
                {
                    // Create index for the consecutive player
                    int followup = i + winnerIndex;

                    // compute remainder in case we overflow the index of the players list
                    followup %= xmlGame.Players.Count;

                    newList.Add(xmlGame.Players[followup]);
                }

                // Set the new players list reflecting the new order
                xmlGame.Players = newList;
            }
            
            GamePlayer player = xmlGame.GetPlayer(PlayerID);

            // First player will start the game
            xmlGame.PlayerOnTurn = xmlGame.Players[0];

            _Message = String.Format("{0} rolled {1}, so he is highroller",
                player.XmlPlayer.Name, DiceRoll);

            base.PerformTurnAction(xmlGame);
        }
    }
}
