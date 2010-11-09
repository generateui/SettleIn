using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SettleInCommon.Gaming
{
    /// <summary>
    /// whenboth players provide equal amount of ships, and the route is equal seen
    /// from each others own views (endtowncity is opponent starttowncity & vice versa)
    /// the TradeRoute with the lowset TurnID wins
    /// </summary>
    public class TradeRoute : Route
    {
        private int _TurnID;

        public int TurnID
        {
            get { return _TurnID; }
            set { _TurnID = value; }
        }
        /// <summary>
        /// Determines the winner of the TradeRoute
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public GamePlayer Winner(XmlGame game, GamePlayer player)
        {
            int playerCount = this.Where(rn => rn.PlayerID == player.XmlPlayer.ID).Count();
            var opponentIDs = this.Where(rn => rn.PlayerID !=player.XmlPlayer.ID);
            int opponentID=0;
            if (opponentIDs.Count() >0)
                opponentID=opponentIDs.First().PlayerID;
            if (opponentID != 0)
            {
                int ships = GetShipCount(player.XmlPlayer.ID);
                int opponentShips=GetShipCount(opponentID);

                if (ships > opponentShips)
                    return player;

                if (ships < opponentShips)
                    return game.GetPlayer(opponentID);
                else
                // we have equal amount of ships, decide on earliest route
                {
                    return null;
                    //TODO: lookup the trade route
                }
            }
            else
            // no opponents, return current player
            {
                return player;
            }

        }
        public int GetShipCount(int playerID)
        {
            return this.Where(rn => rn.PlayerID == playerID && rn.IsShip).Count();
        }

        public bool EqualsOpposite(TradeRoute second)
        {
            if (this.First().Equals(second.Last()) &&
                this.Last().Equals(second.First()) &&
                Count == second.Count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Equality is achieved when 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            TradeRoute second = obj as TradeRoute;
            if (this.First().Equals(second.First()) &&
                this.Last().Equals(second.Last()) &&
                Count == second.Count)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.First().GetHashCode() ^ Count ^ this.Last().GetHashCode();
        }

    }
}
