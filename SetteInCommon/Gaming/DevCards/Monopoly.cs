using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using SettleInCommon.Board;
using SettleInCommon.User;

namespace SettleInCommon.Gaming.DevCards
{
    [DataContract]
    public class Monopoly : DevelopmentCard
    {
        [DataMember]
        public EResource Resource { get; set; }

        public override void Execute(XmlGame game, GamePlayer player)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append(String.Format("{0} stole ", player.XmlPlayer.Name));

            foreach (GamePlayer gamePlayer in game.Players)
            {
                //steal only form opponents
                if (!gamePlayer.XmlPlayer.Equals(player.XmlPlayer.ID))
                {
                    // and only if he has the type of resource
                    if (gamePlayer.Resources[Resource, true] > 0)
                    {
                        // add resources by the development card owner
                        player.Resources[Resource, true] += gamePlayer.Resources[Resource, true];

                        msg.Append(String.Format("{0} {1} from {2}, ",
                            gamePlayer.Resources[Resource, true], Resource.ToString(), gamePlayer.XmlPlayer.Name));

                        // remove resources at victims
                        gamePlayer.Resources[Resource, true] = 0;
                    }
                }
            }

            // remove the trailing ","
            _Message = msg.ToString().Substring(0, msg.ToString().Length - 2);

            base.Execute(game, player);
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            if (Resource == EResource.Volcano)
            {
                _InvalidMessage = "Cannot monopoly on volcano resource";
                return false;
            } 
            if (Resource == EResource.Gold)
            {
                _InvalidMessage = "Cannot monopoly gold resource";
                return false;
            }
            if (Resource == EResource.Discovery)
            {
                _InvalidMessage = "Cannot monopoly discovery resource";
            }

            return true;
        }
    }
}
