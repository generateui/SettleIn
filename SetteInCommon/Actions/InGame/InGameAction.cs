using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;
using SettleInCommon;

using SettleInCommon.Actions.TurnActions;

namespace SettleInCommon.Actions.InGame
{
    [DataContract]
    [KnownType(typeof(AcceptOfferAction))]
    [KnownType(typeof(CounterTradeOfferAction))]
    [KnownType(typeof(GameChatAction))]
    [KnownType(typeof(LooseCardsAction))]
    [KnownType(typeof(PickGoldAction))]
    [KnownType(typeof(PlayerReconnectedAction))]
    [KnownType(typeof(PlayerLeftAction))]
    [KnownType(typeof(PlayerLostConnection))]
    [KnownType(typeof(SpectatorLeftAction))]
    [KnownType(typeof(SpectatorLostConnection))]
    [KnownType(typeof(SpectatorJoinedAction))]
    [KnownType(typeof(TurnAction))]
    public abstract class InGameAction : GameAction
    {
        protected GamePlayer _GamePlayer;
        /// <summary>
        /// Server assigned GameID
        /// </summary>
        [DataMember]
        public int GameID { get; set; }

        public virtual string ToDoMessage
        {
            get { return "This command has no TODO message"; }
        }

        public virtual GamePlayer GamePlayer
        {
            get 
            {
                if (Sender == 0 && _GamePlayer == null)
                {
                    _GamePlayer = new GamePlayer()
                    {
                        XmlPlayer = new User.XmlUser() 
                        {
                            Name = "Server",
                            ID = 0 
                        }
                    };
                }
                return _GamePlayer; 
            }
            set
            {
                _GamePlayer = value;
                Sender = _GamePlayer.XmlPlayer.ID;
            }
        }
        
        public virtual TurnAction NextAction(XmlGame game)
        {
            return null;
        }

        /// <summary>
        /// Index number of the action in the game.
        /// 
        /// Valuable for i.e. checking gamestate is still correct.
        /// </summary>
        [DataMember]
        public int Index { get; set; }

        public virtual void PerformTurnAction(XmlGame xmlGame)
        {
            xmlGame.GameLog.Add(this);
        }

        public virtual bool IsValid(XmlGame game)
        {
            if (!base.IsValid()) return false;

            if (Sender == 0)
            {
                _InvalidMessage = "Server (id=0) can't play";
                return false;
            }

            return true;
        }
    }
}
