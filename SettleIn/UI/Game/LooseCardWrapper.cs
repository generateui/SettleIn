using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.Actions.InGame;

namespace SettleIn.UI.Game
{
    public class LooseCardWrapper
    {
        private ResourceList _ResourcesLost;
        private ResourceList _PlayerResources;
        private int _ResourcesToLoose;
        private GamePlayer _Player;
        private LooseCardsAction _Loosecards;

        public LooseCardsAction Loosecards
        {
            get { return _Loosecards; }
            set { _Loosecards = value; }
        }

        public ResourceList ResourcesLost
        {
            get { return _ResourcesLost; }
            set { _ResourcesLost = value; }
        }

        public ResourceList PlayerResources
        {
            get { return _PlayerResources; }
            set { _PlayerResources = value; }
        }

        public int ResourcesToLoose
        {
            get { return _ResourcesToLoose; }
            set { _ResourcesToLoose = value; }
        }

        public GamePlayer Player
        {
            get { return _Player; }
            set { _Player = value; }
        }

    }
}
