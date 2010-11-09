using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using SettleInCommon.Board;

namespace SettleInCommon.Gaming.DevCards
{
    public class YearOfPlenty : DevelopmentCard
    {
        [DataMember]
        public ResourceList Resources { get; set; }

        public override void Execute(XmlGame game, GamePlayer player)
        {
            _Message = String.Format("{0} gained {1} by playing a Year of Plenty card",
                player.XmlPlayer.Name, Resources.ToString());
            
            // give player the resources
            player.Resources.AddCards(Resources);

            base.Execute(game, player);
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            return true;
        }
    }
}
