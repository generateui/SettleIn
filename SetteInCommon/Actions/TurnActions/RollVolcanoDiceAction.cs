using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Gaming;
using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;

namespace SettleInCommon.Actions.TurnActions
{
    /// <summary>
    /// A player rolled the dice for a volcano
    /// TODO: decide on number assignment strategy for volcano's. Options:
    /// - Use first and second dice for first and second volcano respectively
    /// - Roll one dice and assign number to each volcano
    /// - Roll one dice for each volcano
    /// </summary>
    [DataContract]
    public class RollVolcanoDiceAction : TurnAction
    {
        private List<VolcanoHex> _VolcanosRolled = null;

        [DataMember]
        public int Dice { get; set; }

        [DataMember]
        public List<VolcanoHex> VolcanosRolled
        {
            get { return _VolcanosRolled; }
            set { _VolcanosRolled = value; }
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            //if (_VolcanosRolled == null)
           // {
            //    _InvalidMessage = "No volcanoes to roll for";
            //    return false;
            //}
            //if (RollDice == null)
            //{
            //    _InvalidMessage = "RollDice of type RolDiceAction can't be null";
            //    return false;
            //}
            /*
            if (Dice != 1 &&
                Dice != 2 &&
                Dice != 3 &&
                Dice != 4 &&
                Dice != 5 &&
                Dice != 6)
            {
                _Message = "We use cubes as dice. Number 1-6 please";
                return false;
            }
             */

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            int destroyedCount = 0;
            StringBuilder message = new StringBuilder();
            // TODO: add logic for trade routes, longest route and IslandBonusVPs
            
            List<HexPoint> blownItems = new List<HexPoint>();
            
            var volcanoes = from v in xmlGame.Board.Hexes.OfType<VolcanoHex>()
                            where v.XmlChit.ChitNumber == Chit.GetChitNumber(Dice) 
                            select v;
            
            // Loop through all volcanoes, and create a hexpoint. Then explode the 
            // hexpoint
            foreach (VolcanoHex volcano in volcanoes)
            {
                HexPoint pointToExplode = volcano.Location.GetPosition(Dice);

                foreach (GamePlayer player in xmlGame.Players)
                {
                    // Flag to keep track of exploded towns to do ports administration
                    bool explodedAndLostPosition = false;

                    // blowup city when present at location
                    if (player.Cities.Contains(pointToExplode))
                    {
                        // add it to the list of blown up stuff
                        blownItems.Add(pointToExplode);
                        player.Cities.Remove(pointToExplode);
                        player.StockCities++;
                        destroyedCount++;
                        
                        //only put a town back when we have one in stock
                        if (player.StockTowns > 0)
                        {
                            player.Towns.Add(pointToExplode);
                            player.StockTowns--;
                            message.Append(String.Format("{0} lost a city, and is replaced by a town.", player.XmlPlayer.Name));
                        }
                        else
                        {
                            message.Append(String.Format("{0} lost a city. No towns available in stock, no replacement", player.XmlPlayer.Name));
                            explodedAndLostPosition=true;
                        }
                    }

                    // We do not want to explode a town which got added as city replacement
                    if (player.Towns.Contains(pointToExplode) &&
                        !blownItems.Contains(pointToExplode))
                    {
                        player.Towns.Remove(pointToExplode);
                        player.StockTowns++;
                        destroyedCount++;
                        message.Append(String.Format("{0} lost a town. ", player.XmlPlayer.Name));
                        explodedAndLostPosition=true;
                    }

                    // If a town or (city without replacement town) is removed, check for:
                    // - Ports
                    // - Bonus island VPs
                    // - Trade routes
                    // - Longest route
                    if (explodedAndLostPosition)
                    {
                        // Ports

                        var ports = from SeaHex h in xmlGame.Board.Hexes.OfType<SeaHex>()
                                    where h.XmlPort != null
                                    select h;
                        var port = (from SeaHex p in ports
                                    where
                                     p.XmlPort.SideLocation.HexPoint1.Equals(pointToExplode) ||
                                     p.XmlPort.SideLocation.HexPoint2.Equals(pointToExplode)
                                    select p).SingleOrDefault();

                        // If we find a port, remove it from the list of ports of the player
                        if (port != null)
                        {
                            xmlGame.PlayerOnTurn.Ports.Remove(port.XmlPort.PortType);
                            message.Append(String.Format("{0} also lost his {1} port.", 
                                player.XmlPlayer.Name,
                                port.XmlPort.PortType.ToString()));
                        }


                        // Bonus vps

                        var bonusVpAffectd = player.BonusIslandVPs
                            .Where(hp => hp.Equals(pointToExplode));

                        if (bonusVpAffectd.Count() > 0)
                        {
                            foreach (HexPoint lostBonusVP in bonusVpAffectd)
                            {
                                player.BonusIslandVPs.Remove(lostBonusVP);
                            }
                            message.Append(String.Format("{0} lost {1} bonus victory point(s).", 
                                player.XmlPlayer.Name, bonusVpAffectd.Count()));
                        }

                        // Traderoutes

                        List<TradeRoute> routesToRemove = new List<TradeRoute>();
                        foreach (TradeRoute tradeRoute in player.TradeRoutes)
                        {
                            List<HexPoint> points = new List<HexPoint>();
                            points.Add(tradeRoute[0].HexPoint1);
                            points.Add(tradeRoute[0].HexPoint2);
                            points.Add(tradeRoute[tradeRoute.Length - 1].HexPoint1);
                            points.Add(tradeRoute[tradeRoute.Length - 1].HexPoint2);
                            if (points.Contains(pointToExplode))
                            {
                                routesToRemove.Add(tradeRoute);
                            }
                        }
                        foreach (TradeRoute removeRoute in routesToRemove)
                        {
                            player.TradeRoutes.Remove(removeRoute);
                        }
                        if (routesToRemove.Count > 0)
                        {
                            message.Append(String.Format("{0} lost {1} traderoutes. ", 
                                player.XmlPlayer.Name, routesToRemove.Count));
                        }
                    }
                }
            }

            // When stuff is destroyed, show the destroyed message.Otherwise, notify every player is unscathed
            if (destroyedCount > 0)
            {
                _Message = message.ToString();
            }
            else
            {
                _Message = String.Format("{0} rolled a {1}. Every player escaped volcano(s) unscathed",
                    _GamePlayer.XmlPlayer.Name, Dice);
            }

            base.PerformTurnAction(xmlGame);
        }
        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should roll the dice for volcano eruption", _GamePlayer.XmlPlayer.Name);
            }
        }
    }
}
