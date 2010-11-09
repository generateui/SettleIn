using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using SettleInCommon.Board;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Gaming;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Gaming.GamePhases;

namespace SettleInCommon.Actions.TurnActions
{
    // TODO: add bank check for resources
    [DataContract]
    public class RollDiceAction : TurnAction
    {
        private Dictionary<GamePlayer, ResourceList> _playersResources =
            new Dictionary<GamePlayer, ResourceList>();

        private List<ResourceHex> _HexesAffected = new List<ResourceHex>();
        private List<int> _LooserPlayers = new List<int>();

        [DataMember]
        public List<int> LooserPlayers
        {
            get { return _LooserPlayers; }
            set { _LooserPlayers = value; }
        }

        public Dictionary<GamePlayer, ResourceList> PlayersResources
        {
            get { return _playersResources; }
        }


        [DataMember]
        public int Dice1 { get; set; }

        [DataMember]
        public int Dice2 { get; set; }

        public List<ResourceHex> HexesAffected
        {
            get { return _HexesAffected; }
        }

        public override string Message
        {
            get
            {
                return _Message;
            }
        }

        public int Dice
        {
            get { return Dice1 + Dice2; }
        }

        public override bool IsValid(XmlGame game)
        {
            if (!base.IsValid(game)) return false;

            return true;
        }

        public override void PerformTurnAction(XmlGame xmlGame)
        {
            _playersResources = new Dictionary<Gaming.GamePlayer, ResourceList>();
            GamePlayer gamePlayer = xmlGame.GetPlayer(Sender);

            if (xmlGame.Phase is PlayTurnsGamePhase)
            {
                if (Dice != 7)
                {
                    // gather all resource hexes without the robber
                    IEnumerable<ResourceHex> rolledHexes =
                        from h in xmlGame.Board.Hexes.OfType<ResourceHex>()
                        where h.XmlChit != null &&
                            // we need a chit with the same number as rolled dice
                        h.XmlChit.ChitNumber == Chit.GetChitNumber(Dice)
                        select h;

                    bool volcanoRolled = false;

                    // Iterate over all hexes with resources
                    foreach (ResourceHex hex in rolledHexes)
                    {
                        if (!volcanoRolled && hex is VolcanoHex)
                        {
                            volcanoRolled = true;
                        }
                        // For normal resources, the location of the robber is omitted.
                        if (!hex.Location.Equals(xmlGame.Robber))
                        {
                            foreach (GamePlayer player in xmlGame.Players)
                            {
                                // make a list of cities
                                IEnumerable<HexPoint> citiesOnHex =
                                    from city in player.Cities
                                    where city.HasLocation(hex.Location)
                                    select city;

                                IEnumerable<HexPoint> townsOnHex =
                                    from town in player.Towns
                                    where town.HasLocation(hex.Location)
                                    select town;

                                ResourceList gainedResources = new ResourceList();
                                gainedResources.AddResources(
                                    hex.Resource, (citiesOnHex.Count() * 2) + townsOnHex.Count());

                                //update gamestate
                                player.Resources.AddCards(gainedResources);
                                xmlGame.Bank.SubtractCards(gainedResources);
                                if (!_playersResources.Keys.Contains(player))
                                {
                                    //add new entry when no resources registered yet
                                    _playersResources.Add(player, gainedResources);
                                }
                                else
                                {
                                    // we already gained resources, add resources from current hex 
                                    _playersResources[player].AddCards(gainedResources);
                                }
                            } // Robber
                        } // Foreach hex

                        _HexesAffected.Add(hex);
                    }

                    // Remove gold from resourcesGained, add PickGoldAction for each player with gold
                    foreach (KeyValuePair<GamePlayer, ResourceList> kvp in _playersResources)
                    {
                        if (kvp.Value.Gold > 0)
                        {
                            xmlGame.ActionsQueue.Enqueue(new PickGoldAction()
                            {
                                GamePlayer = kvp.Key,
                                Amount = kvp.Value.Gold
                            });
                        }
                        kvp.Key.Resources.Gold = 0;
                    }

                    // If there is a volcano producing stuff, expect player to roll for the volcano number
                    if (volcanoRolled)
                    {
                        xmlGame.ActionsQueue.Enqueue(new RollVolcanoDiceAction()
                        {
                            GamePlayer = _GamePlayer
                        });
                    }

                    _Message = String.Format("{0} rolled {1} ({2} + {3})",
                        gamePlayer.XmlPlayer.Name, Dice, Dice1, Dice2);
                }
                else
                {
                    // Rolled a 7, create list of players to loose cards
                    string playerList = string.Empty;

                    foreach (GamePlayer p in xmlGame.Players)
                    {
                        if (p.Resources.Count > xmlGame.Settings.MaximumCardsInHandWhenSeven)
                        {
                            _LooserPlayers.Add(p.XmlPlayer.ID);
                            // Add comma and playername to message
                            playerList += _LooserPlayers.Count > 0 ?
                                ", " + p.XmlPlayer.Name : p.XmlPlayer.Name;
                        }
                    }

                    _Message = String.Format("{0} rolled a 7.", playerList);

                    if (_LooserPlayers.Count > 0)
                    {
                        _Message = String.Format("{0}{1} loose half of their resources", _Message, playerList);
                    }
                }
            }
            
            _User = gamePlayer.XmlPlayer;
            base.PerformTurnAction(xmlGame);
        }
        public override string ToDoMessage
        {
            get
            {
                return String.Format("{0} should roll the dice", _GamePlayer.XmlPlayer.Name);
            }
        }
    }
}
