using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.DevCards
{
    public class RoadBuilding : DevelopmentCard
    {
        public override void Execute(XmlGame game, GamePlayer player)
        {
            player.DevRoadShips += 2;
            _Message = String.Format("{0} played a road building card", player.XmlPlayer.Name);

            base.Execute(game, player);
        }
    }
}
