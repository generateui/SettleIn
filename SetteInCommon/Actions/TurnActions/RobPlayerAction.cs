using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Gaming;

namespace SettleInCommon.Actions.TurnActions
{
    /// <summary>
    /// Player on turn robs victim from a handcard. 
    /// Played after RolldiceAction or PlaceRobberPirateAction ( originating PlayDevcardAction(soldier))
    /// </summary>
    [DataContract]
    public class RobPlayerAction : TurnAction
    {
        // when PlayerID is null, the player didnt rob anyone. How Refreshing!
        [DataMember]
        public int? PlayerID { get; set; }

        [DataMember]
        public EResource StolenResource { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) 
                return false;

            GamePlayer robbedPlayer = null;

            if (PlayerID != null)
            {
                robbedPlayer = game.GetPlayer((int)PlayerID);
            }

            if (PlayerID == 0)
            {
                _InvalidMessage = "We can't rob the server!";
                return false;
            }

            // .
            if (robbedPlayer == null &&
                PlayerID != null)
            {
                _InvalidMessage = "Either we should rob no one, or we should rob someone";
                return false;
            }
            // TODO: check if the robbed player has a town or city on one 
            // of the 6 points

            // 
            if (robbedPlayer != null)
            {
                _InvalidMessage = "The player should have something to be robbed";
                if (robbedPlayer.Resources.CountAllExceptDiscovery == 0) return false;
            }

            return true;
        }
        public override void PerformTurnAction(XmlGame xmlGame)
        {
            if (PlayerID == null)
            {
                _Message = String.Format("{0} stole nothing! How refreshing", xmlGame.GetPlayer(Sender).XmlPlayer.Name);
            }
            else
            {
                // Rob the player
                GamePlayer robbedPlayer = xmlGame.GetPlayer((int)PlayerID);
                robbedPlayer.Resources.RemoveResource(StolenResource);

                // Give card to robbing player
                GamePlayer player = xmlGame.GetPlayer(Sender);
                player.Resources.AddResource(StolenResource);

                _Message = String.Format("{0} stole 1 {1} from {2}",
                    player.XmlPlayer.Name, StolenResource.ToString(), robbedPlayer.XmlPlayer.Name);
            }

            base.PerformTurnAction(xmlGame);
        }
        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should steal a card", _GamePlayer.XmlPlayer.Name);
            }
        }
    }
}
