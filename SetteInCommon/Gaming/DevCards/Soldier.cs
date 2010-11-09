using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using SettleInCommon.Board;

namespace SettleInCommon.Gaming.DevCards
{
    public class Soldier : DevelopmentCard
    {
        public override void Execute(XmlGame game, GamePlayer player)
        {
            // Update largest army
            game.CalculateLargestArmy(player);

            _Message = String.Format("{0} played a soldier", player.XmlPlayer.Name);

            base.Execute(game, player);
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;



            return true;
        }
    }
}
