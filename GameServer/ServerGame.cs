using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon;
using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Actions;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Board;
using SettleInCommon.Gaming.DevCards;
using SettleInCommon.Actions.Lobby;

namespace GameServer
{
    public class ServerGame : ICloneable
    {
        // locking object
        private object _ThreadSync = new object();

        private List<XmlUser> _Users = new List<XmlUser>();
        private List<XmlUser> _Spectators = new List<XmlUser>();
        private XmlGame _Game;
        private XmlGameSettings _Settings;
        private XmlUser _Host;
        private int _ID;

        public List<XmlUser> Spectators
        {
            get { return _Spectators; }
            set { _Spectators = value; }
        }

        public List<XmlUser> Users
        {
            get { return _Users; }
            set { _Users = value; }
        }

        public XmlUser Host
        {
            get { return _Host; }
            set { _Host = value; }
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public XmlGame Game
        {
            get { return _Game; }
            set { _Game = value; }
        }

        public ServerGame() { }

        public ServerGame(XmlGameSettings settings)
        {
            _Game = new XmlGame() { Settings = settings };
        }

        public GameAction ExecuteAction(InGameAction action)
        {
            if (action.IsValid(_Game))
            {
                //Execute the game action if the action is valid
                action.PerformTurnAction(_Game);

                // Send action back to sender for confirmation
                
                //Send action to all other players and spectators

                //Add action to the gamelog
                _Game.GameLog.Add(action);
                return action;
            }
            else
            // we received an illegal ingameaction.
            // This should only happen when a user deliberately tampers with data
            {
                MessageFromServerAction message = new MessageFromServerAction();
                message.Message = String.Format(
                    "The game action does not seem to be valid. The reason: action object says \"{0}\"",
                    action.InvalidMessage);
                return message;
            }
        }

        public void UserDisconnected(XmlUser user)
        {
        }

        public void UserLeft(XmlUser user)
        {
        }

        public void SpectatorJoined(XmlUser user)
        {

        }

        #region ICloneable Members

        public object Clone()
        {
            return new ServerGame()
            {
                Game = _Game.Copy(),
                ID = _ID,
                Users = new List<XmlUser>(_Users)
            };
        }

        #endregion
    }
}
