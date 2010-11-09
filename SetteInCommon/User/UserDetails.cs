using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;
using SettleInCommon.User;

namespace SettleInCommon
{
    [DataContract]
    public class XmlUserDetails
    {
        [DataMember]
        public int WonLadderGames { get; set; }

        [DataMember]
        public int PlayedLadderGames { get; set; }

        [DataMember]
        public Guid FavoriteMapID { get; set; }

        [DataMember]
        public int Rating { get; set; }

        [DataMember]
        public int TotalQuits { get; set; }

        [DataMember]
        public int TotalDrops { get; set; }

        [DataMember]
        public XmlUser[] Buddies;

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public TimeSpan AverageTurnTime { get; set; }

        public XmlUserDetails Copy()
        {
            XmlUserDetails result = new XmlUserDetails();

            result.AverageTurnTime = AverageTurnTime;
            result.FavoriteMapID = FavoriteMapID;
            result.ID = ID;
            result.PlayedLadderGames = PlayedLadderGames;
            result.Rating = Rating;
            result.TotalDrops = TotalDrops;
            result.TotalQuits = TotalQuits;
            result.WonLadderGames = WonLadderGames;

            return result;

        }
    }
}
