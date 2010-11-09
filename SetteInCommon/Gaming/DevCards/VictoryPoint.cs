using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Gaming.DevCards
{
    public class VictoryPoint : DevelopmentCard
    {
        public override void Execute(XmlGame game, GamePlayer player)
        {
            _Message = String.Format("{0} played a Victory Point card"
                , player.XmlPlayer.ID);
            
            base.Execute(game, player);
        }


        public override bool WaitOneTurn
        {
            get
            {
                return false;
            }
        }
    }
}
