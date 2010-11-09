using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon;

using SettleInCommon.Gaming;
using SettleInCommon.User;
using SettleInCommon.Actions;
using SettleInCommon.Actions.Turn;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Board;

namespace SettleInServer
{
    public class ServerGame
    {
        private List<XmlUser> _Users = new List<XmlUser>();
        private XmlGameSettings _Settings;
        public XmlUser Host { get; set; }
        public int ID { get; set; }
        private XmlGame _Game;

        public XmlGame Game
        {
            get { return _Game; }
            set { _Game = value; }
        }

        public XmlGameSettings Settings
        {
            get { return _Settings; }
        }

        public List<XmlUser> Users
        {
            get
            {
                return _Users;
            }
        }
        public ServerGame(XmlGameSettings settings)
        {
            _Settings = settings;
        }

        public void ExecuteAction(InGameAction action)
        {
            //check if the userid of the action is actually playing
            if ((from u in _Users where u.ID == action.UserID select u).Count() == 0)
            {
                if (action is BuildRoadAction)
                {
                    TryBuildRoad((BuildRoadAction)action);
                }
            }
            else
            // we received an ingame action from a user which is not in this game
            {
                MessageFromServerAction message = new MessageFromServerAction();
                message.Message = String.Format(
                    "Hey! You send an action for game with id: {0}, but you with userid {1} are not in the players list of game with id: {0}",
                    action.GameID, action.UserID);

            }
        }

        private GameAction TryBuildRoad(BuildRoadAction action)
        {
            if (_Game.IsRoadShipPresent(action.Intersection))
            // hey, road or ship found, we can't built
            {
                MessageFromServerAction message = new MessageFromServerAction();
                message.Message = String.Format(
                    "Hey! We cannot build a road at location {0}. There is already a road or ship.",
                    action.GameID, action.UserID);
                return message;
            }
            else
            // yey! no ship or road present, let's build there
            {
                GamePlayer player = _Game.GetPlayer(action.UserID);
                if (player != null)
                {
                    player.BuildRoad(action.Intersection);
                    return action;
                }
                else
                {
                    MessageFromServerAction message = new MessageFromServerAction();
                    message.Message = String.Format(
                        "Hey! We _can_ build a road at location {0}, but userid {1} is not found in game with id {2}",
                       action.Intersection.ToString(), action.UserID, action.GameID);
                    return message;
                }
            }
        }
        public GameAction TryBuildShip(BuildShipAction action)
        {
            if (_Game.IsRoadShipPresent(action.Intersection))
            // hey, road or ship found, we can't built
            {
                MessageFromServerAction message = new MessageFromServerAction();
                message.Message = String.Format(
                    "Hey! We cannot build a ship at location {0}. There is already a road or ship.",
                    action.GameID, action.UserID);
                return message;
            }
            else
            // yey! no ship or road present, let's build there
            {
                GamePlayer player = _Game.GetPlayer(action.UserID);
                if (player != null)
                {
                    player.BuildShip(action.Intersection);
                    return action;
                }
                else
                {
                    MessageFromServerAction message = new MessageFromServerAction();
                    message.Message = String.Format(
                        "Hey! We _can_ build a ship at location {0}, but userid {1} is not found in game with id {2}",
                       action.Intersection.ToString(), action.UserID, action.GameID);
                    return message;
                }
            }
        }
        public GameAction TryBuildTown(PlaceTownAction action)
        {
            if (_Game.IsTownCityPresent(action.Location))
            // town or city found, we cannot build there
            {
                MessageFromServerAction message = new MessageFromServerAction();
                message.Message = String.Format(
                    "Hey! We cannot build a town at location {0}. There is already a town or city.",
                    action.Location);
                return message;
            }
            else
            // the spot is free to build for the player.             
            {
                //check if we have a town or city on the side, and check if we have a road or ship
                // connecting to the location where player wants to put a town
                GamePlayer player = _Game.GetPlayer(action.UserID);
                if (!_Game.HasRoadShipAtPoint(action.Location, player))
                //sweet, we have a road or ship at a side of the location
                // and the spot or its neighbours isnt taken yet
                {
                    player.BuildTown(action.Location);
                    return action;
                }
                else
                {
                    MessageFromServerAction message = new MessageFromServerAction();
                    message.Message = String.Format(
                        "Hey! We cannot build a town at location {0}. You do not have a road or ship at that point.",
                        action.Location);
                    return message;
                }
            }
            throw new Exception("");
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



    }
}
