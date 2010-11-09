using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;
using SettleInCommon;
using SettleInCommon.User;
using SettleInCommon.Board;

namespace SettleInCommon.Actions.InGame
{
    [DataContract]
    public class PickGoldAction : InGameAction
    {
        private ResourceList _Resources = new ResourceList();
        
        [DataMember]
        public ResourceList Resources 
        {
            get { return _Resources; }
            set { _Resources = value; }
        }

        [DataMember]
        public int Amount { get; set; }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid()) return false;

            if (Resources == null || Amount == 0)
            {
                _InvalidMessage = "Resources or amount cannot be null or 0";
                return false;
            }

            //Bank should have the picked resources
            if (!game.Bank.HasAtLeast(Resources))
            {
                _InvalidMessage = String.Format("Bank does not have {0}",
                    Resources);
                return false;
            }

            if (Amount != Resources.CountAllExceptDiscovery)
            {
                _InvalidMessage = "Amount of gold should be equal to the amount of picked resources";
                return false;
            }

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);

            gamePlayer.Resources.AddCards(Resources);
            xmlGame.Bank.SubtractCards(Resources);

            _Message = String.Format("{0} gained {1} from his {2} gold resources",
                gamePlayer.XmlPlayer.Name, Resources.ToString(), Amount);

            base.PerformTurnAction(xmlGame);
        }

    }
}
