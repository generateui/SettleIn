using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SettleIn.UI.Elements.Game;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon.Gaming.DevCards;
using SettleInCommon.User;
using SettleInCommon.Actions.Lobby;
using SettleIn.UI.Game;
using SettleInCommon.Actions;
using System.ComponentModel;
using System.Timers;
using System.Windows.Threading;
using SettleIn.Engine.ViewPort;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Gaming.GamePhases;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for GameMain2.xaml
    /// </summary>
    public partial class GameMain2 : UserControl
    {
        private InGameAction _CurrentAction;
        private InGameAction _QueuedAction;

        /// <summary>
        /// List of errors receivd from the server
        /// </summary>
        private ObservableCollection<GameAction> _Errors = new ObservableCollection<GameAction>();

        public delegate void ExecuteGameActionHandler(InGameAction action);
        public delegate void DoneHereHandler(bool success);

        private XmlGame Game
        {
            get { return gameBoardViewPort.CurrentGame; }
        }

        private GamePlayer Player
        {
            get { return gameBoardViewPort.CurrentGame.PlayingPlayer; }
        }

        public GameMain2()
        {
            InitializeComponent();

            gameBoardViewPort.ActionCompleted += new GameViewPort3D.ActionCompletedHandler(gameBoardViewPort_ActionCompleted);

            this.Loaded += new RoutedEventHandler(GameMain2_Loaded);
            uiBuildPalette.ExecuteGameAction += new ExecuteGameActionHandler(uiBuildPallette_ExecuteGameAction);
            uiBuildPalette.DoneHere += new DoneHereHandler(uiBuildPalette_DoneHere);
            uiBankTrade.DoneHere += new DoneHereHandler(uiBankTrade_DoneHere);
            spTrade.ExecuteGameAction += new ExecuteGameActionHandler(uiTradeOffer_ExecuteAction);
            spTrade.DoneHere += new DoneHereHandler(spTrade_DoneHere);
            uiResourcesGained.DoneHere += new DoneHereHandler(uiResourcesGained_DoneHere);
            uiStealCard.DoneHere += new DoneHereHandler(uiStealCard_DoneHere);
            uiLooseCards.DoneHere += new DoneHereHandler(uiLooseCards_DoneHere);
            uiPickGoldAll.DoneHere += new DoneHereHandler(uiPickGoldAll_DoneHere);

            // Server events
            Server.Instance.InGameActionReceived += new Server.InGameActionReceivedHandler(Server_InGameActionReceived);
            Server.Instance.TurnActionReceived += new Server.TurnActionReceivedHandler(Server_TurnActionReceived);
            Server.Instance.GameActionReceived += new Server.GameActionReceivedHandler(Instance_GameActionReceived);
            
            uiNewHotSeat.NewGameClicked += new NewGame.NewGameClickedHandler(uiNewHotSeat_NewGameClicked);
        }

        void uiBuildPalette_DoneHere(bool success)
        {
            // Volcano OK button hit
            uiBuildPalette.UnSetVolcanoDice();
        }

        void uiPickGoldAll_DoneHere(bool success)   
        {
            // Get rid of the pick gold window
            RemoveOverlay(uiPickGoldAll);

            // Send the actions to the server
            foreach (PickGoldAction pickGold in uiPickGoldAll.PickedGold
                .Where(pga => pga.Amount != 0))
            {
                pickGold.GamePlayer = Game.GetPlayer(pickGold.Sender);
                Server.Instance.Say(pickGold);
            }
        }

        void uiLooseCards_DoneHere(bool success)
        {
            // Set UI state
            RemoveOverlay(uiLooseCards);

            // Send every loosecards action to the server
            foreach (LooseCardWrapper lostCards in uiLooseCards.LostCards)
            {
                if (lostCards.ResourcesLost != null)
                {
                    // Since the control binds to the resources, we need to add them here
                    LooseCardsAction loosIt = new LooseCardsAction()
                    {
                        ResourcesLost = lostCards.ResourcesLost
                    };
                    Server.Instance.Say(loosIt);
                }
            }
        }

        /// <summary>
        /// A player has clicked "Steal card" button on the uiStealCard
        /// </summary>
        /// <param name="success"></param>
        void uiStealCard_DoneHere(bool success)
        {
            int? victimID = null;
            if (success)
            {
                victimID = uiStealCard.Victim.XmlPlayer.ID;
            }

            RemoveOverlay(uiStealCard);
            RobPlayerAction robPlayer = new RobPlayerAction()
            {
                GamePlayer = Game.PlayingPlayer,
                PlayerID = victimID
            };
            Server.Instance.Say(robPlayer);
        }

        void uiResourcesGained_DoneHere(bool success)
        {
            uiBuildPalette.ShowActions();
            RemoveOverlay(uiResourcesGained);
            gameBoardViewPort.SetNeutral();

            if (Game.ActionsQueue.Count > 0)
            {
                // Picking gold occurs _before_ possible explosion. 
                if (Game.ActionsQueue.Peek() is PickGoldAction)
                {
                    uiPickGoldAll.SetData(Game);
                    ShowOverlay(uiPickGoldAll);
                }
            }
        }

        void Instance_GameActionReceived(GameAction action)
        {
            MessageFromServerAction serverMessage = action as MessageFromServerAction;
            if (serverMessage != null)
            {
                _Errors.Add(serverMessage);
                uiChatLogTabControl.SelectedIndex = 2;
            }
        }

        void uiNewHotSeat_NewGameClicked()
        {
            XmlBoard board = Core.Instance.Boards.ToList().Find
                (b => b.ID.Equals(uiNewHotSeat.Settings.Map));
            if (board == null)
            {
                MessageBox.Show("Whoops! Board not found in list of boards. /r/n Might be an out of date board definition (unsupported old version if file format).",
                    "Board missing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                HostStartsGameAction newHotSeat = new HostStartsGameAction()
                {
                    Sender = Core.Instance.CurrentPlayer.ID,
                    DateTime = DateTime.Now,
                    Settings = uiNewHotSeat.Settings
                };
                Server.Instance.Say(newHotSeat);
                //remove settings UI window
                RemoveOverlay(uiNewHotSeat);
            }
        }

        void SetPlayerUI(bool canPlay)
        {
            if (canPlay)
            {
                cvBuildOverlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                cvBuildOverlay.Visibility = Visibility.Visible;
            }
        }

        void Server_TurnActionReceived(TurnAction turnAction)
        {
            DequeueIfExpected(turnAction);

            // Execute the action
            Game.Phase.ProcessAction(turnAction, Game);

            PlayTurnsGamePhase playTurns = Game.Phase as PlayTurnsGamePhase;
            if (playTurns != null)
            {
                uiBuildPalette.SetStatus(playTurns.TurnPhase);
            }

            EndTurnAction endTurn = turnAction as EndTurnAction;
            if (endTurn != null)
            {
                //TODO: add check for hotseat server
                gameBoardViewPort.CurrentGame.PlayingPlayer = 
                    gameBoardViewPort.CurrentGame.PlayerOnTurn;
                uiBuildPalette.ShowActions();
            }

            TradeBankAction tradeBank = turnAction as TradeBankAction;
            if (tradeBank != null)
            {
                uiBuildPalette.ShowActions();
            }

            RollDiceAction diceRolled = turnAction as RollDiceAction;
            if (diceRolled != null)
            {
                if (Game.Phase.GetType() == typeof(PlayTurnsGamePhase))
                {
                    ShowResourcesFromDiceRoll(diceRolled);
                    uiBuildPalette.SetStatus(String.Format("Waiting for {0} to finish turn actions", 
                        Game.GetPlayer(turnAction.Sender).XmlPlayer.Name));
                }
                if (Game.ActionsQueue.Count > 0)
                {
                    InGameAction expectedAction = Game.ActionsQueue.Peek();

                    // Check if players need to loose cards.
                    if (expectedAction is LooseCardsAction)
                    {
                        uiLooseCards.SetActions(Game.ActionsQueue.OfType<LooseCardsAction>());
                        ShowOverlay(uiLooseCards);
                    }



                }
            }

            RollVolcanoDiceAction rolledVolcano = turnAction as RollVolcanoDiceAction;
            if (rolledVolcano != null)
            {
                uiBuildPalette.ShowVolcanoDiceResult(rolledVolcano);
                // Show OK button
                // Remove volcano dice after OK button is clicked
                // Show fancy UI 
            }

            // Check if a volcano dice needs to be rolled
            if (Game.ActionsQueue.Count > 0)
            {
                if (Game.ActionsQueue.Peek() is RollVolcanoDiceAction)
                {
                    uiBuildPalette.SetVolcanoDice();
                    return;
                }
            }

            SetNextActionAsExpected();
        }

        private void DequeueIfExpected(InGameAction inGameAction)
        {
            if (Game.ActionsQueue.Count > 0)
            {
                // Grab the expected action
                InGameAction expected = Game.ActionsQueue.Peek();

                // Sender & Type needs to correspond to match ecxpectation
                if (expected.Sender == inGameAction.Sender &&
                    expected.GetType() == inGameAction.GetType())
                {
                    // we encountered the expected first action on the queue
                    Game.ActionsQueue.Dequeue();
                }
            }
        }

        void CurrentGame_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PlayerOnTurn")
            {
                uiPlayerHand.ItemsSource = gameBoardViewPort.CurrentGame.PlayerOnTurn.Resources;
                uiBankTrade.DataContext = gameBoardViewPort.CurrentGame.PlayingPlayer;
            }
        }

        void Server_InGameActionReceived(InGameAction inGameAction)
        {
            // If incoming action is the expected action, remove it from the queue
            DequeueIfExpected(inGameAction);

            StartGameAction startGame = inGameAction as StartGameAction;
            if (startGame != null)
            {
                gameBoardViewPort.CurrentGame = startGame.Game;
                Game.PropertyChanged += new PropertyChangedEventHandler(CurrentGame_PropertyChanged);
                SetData();
                uiBuildPalette.ShowActions();
            }

            // TODO: remove, add data hook
            GameChatAction chat = inGameAction as GameChatAction;
            if (chat != null)
            {
                lbxChatBox.Items.Add(new ChatMessage()
                {
                    DateTime = chat.DateTime,
                    Message = chat.ChatMessage,
                    User = new XmlUser()
                    {
                        ID = chat.Sender,
                        Name = "Hmx"
                    }
                });
            }

            Game.Phase.ProcessAction(inGameAction, gameBoardViewPort.CurrentGame);

            if (inGameAction.GetType() == Game.Phase.EndAction())
            {
                Game.Phase = Game.Phase.Next(Game);
                Game.Phase.Start(Game);

                inGameAction.GamePlayer = Game.PlayerOnTurn;
                uiBuildPalette.SetStatus(Game.Phase);
            }

            if (Game.ActionsQueue.Count > 0)
            {
                if (Game.ActionsQueue.Peek() is RollVolcanoDiceAction)
                {
                    uiBuildPalette.SetVolcanoDice();
                    return;
                }
            }

            SetNextActionAsExpected();
        }

        private void SetNextActionAsExpected()
        {
            if (Game.ActionsQueue.Count > 0)
            {
                // Grab next expected action from queue
                InGameAction nextExpected = Game.ActionsQueue.Peek();

                // Set UI user message
                uiBuildPalette.SetStatus(nextExpected.ToDoMessage);

                // Update viewport behaviour
                gameBoardViewPort.BeginAction(nextExpected);
            }
        }

        private void ShowResourcesFromDiceRoll(RollDiceAction diceRolled)
        {
            SetOverlay(uiResourcesGained, false);
            gameBoardViewPort.BeginAction(diceRolled);
            uiResourcesGained.DataContext = diceRolled;
            uiBuildPalette.dices.dice1.Number = diceRolled.Dice1;
            uiBuildPalette.dices.dice2.Number = diceRolled.Dice2;
            uiBuildPalette.SetStatus(diceRolled.Message);
        }

        void spTrade_DoneHere(bool success)
        {
            if (success)
            {
            }
            else
            {
                RemoveOverlay(spTrade);
            }
        }

        /// <summary>
        /// Player done tradig event handler for the banktrade window
        /// </summary>
        /// <param name="success"></param>
        void uiBankTrade_DoneHere(bool success)
        {
            if (success)
            {
                TradeBankAction tradeBank = new TradeBankAction()
                {
                    OfferedCards = uiBankTrade.OfferedCards,
                    WantedCards = uiBankTrade.WantedCards,
                    GamePlayer = Player
                };
                Server.Instance.Say(tradeBank);
                if (_QueuedAction != null)
                {
                    BuyDevcardAction buyDev = _QueuedAction as BuyDevcardAction;
                    if (buyDev!=null)
                    {
                        buyDev.Resources = Game.PlayingPlayer.GetDevcardResources();
                    }
                    uiBuildPalette.ShowActions();
                    ExecuteAction(_QueuedAction);
                    _QueuedAction = null;
                }
            }
            RemoveOverlay(uiBankTrade);
        }

        void gameBoardViewPort_ActionCompleted(InGameAction action)
        {
            Server.Instance.Say(action);

            // If player was placing robber or pirate, make sure he robs someone afterwars
            PlaceRobberPirateAction placeRobber = action as PlaceRobberPirateAction;
            if (placeRobber != null)
            {
                List<GamePlayer> players = new List<GamePlayer>();
                List<HexPoint> points = placeRobber.Location.GetNeighbourPoints();
                foreach (HexPoint p in points)
                {
                    foreach (GamePlayer player in Game.Players)
                    {
                        if (player.GetTownsCities().Contains(p) &&
                            player.XmlPlayer.ID != Game.PlayingPlayer.XmlPlayer.ID &&
                            !players.Contains(player))
                            players.Add(player);
                    }
                }
                uiStealCard.SetUI(players);
                ShowOverlay(uiStealCard);
            }

            uiBuildPalette.ShowActions();
        }

        void uiTradeOffer_ExecuteAction(InGameAction action)
        {
        }

        void uiBuildPallette_ExecuteGameAction(InGameAction action)
        {
            action.GamePlayer = gameBoardViewPort.CurrentGame.PlayerOnTurn;
            if (action is TradeBankAction)
            {
                uiBankTrade.UpdateUI(null, Game.Bank, Player.Resources.Copy(), Player.Ports, BankTrade.ETradeType.None);
                ShowOverlay(uiBankTrade);
                _CurrentAction = action;
                return;
            }
            if (action is TradeOfferAction)
            {
                spTrade.TradeOffer = (TradeOfferAction)action;
                ShowOverlay(spTrade);
                return;
            }
            if (action is EndTurnAction)
            {
            }
            if (action is BuildTownAction)
            {
                // popup autotrade window if we need any amount of gold
                int neededGold = Player.TownNeededGold();
                if (neededGold > 0)
                {
                    ResourceList temp = gameBoardViewPort.CurrentGame.PlayingPlayer.Resources.Copy();
                    temp.SubtractCards(ResourceList.Town);

                    ResourceList wanted = ResourceList.Town;
                    wanted.Wheat -= Player.Resources.Wheat;
                    wanted.Sheep -= Player.Resources.Sheep;
                    wanted.Timber -= Player.Resources.Timber;
                    wanted.Clay -= Player.Resources.Clay;
                    uiBankTrade.UpdateUI(wanted, Game.Bank, temp, Player.Ports, BankTrade.ETradeType.Town);
                    ShowOverlay(uiBankTrade);
                    _QueuedAction = action;
                    uiBuildPalette.HideActions();
                }
            }
            if (action is BuildCityAction)
            {
                // popup autotrade window if we need any amount of gold
                int neededGold = Player.CityNeededGold();
                if (neededGold > 0)
                {
                    ResourceList temp = Player.Resources.Copy();
                    temp.SubtractCards(ResourceList.City);
                    int possibleGold = Player.GoldCount(temp);

                    ResourceList wanted = ResourceList.City;
                    wanted.Wheat -= Player.Resources.Wheat;
                    wanted.Ore -= Player.Resources.Ore;
                    uiBankTrade.UpdateUI(wanted, Game.Bank, temp, Player.Ports, BankTrade.ETradeType.City);
                    ShowOverlay(uiBankTrade);
                    _QueuedAction = action;
                    uiBuildPalette.HideActions();
                }
            }
            if (action is BuildRoadAction)
            {
                // popup autotrade window if we need any amount of gold
                int neededGold = Player.RoadNeededGold();
                if (neededGold > 0)
                {
                    ResourceList temp = Player.Resources.Copy();
                    temp.SubtractCards(ResourceList.Road);
                    int possibleGold = Player.GoldCount(temp);

                    ResourceList wanted = ResourceList.Road;
                    wanted.SubtractCards(Player.Resources);
                    //wanted.Clay -= Player.Resources.Clay;
                    //wanted.Timber -= Player.Resources.Timber;
                    uiBankTrade.UpdateUI(wanted, Game.Bank, temp, Player.Ports, BankTrade.ETradeType.Road);
                    ShowOverlay(uiBankTrade);
                    _QueuedAction = action;
                    uiBuildPalette.HideActions();
                }
            }
            if (action is BuildShipAction)
            {
                // popup autotrade window if we need any amount of gold
                int neededGold = Player.ShipNeededGold();
                if (neededGold > 0)
                {
                    ResourceList temp = Player.Resources.Copy();
                    temp.SubtractCards(ResourceList.Ship);
                    int possibleGold = Player.GoldCount(temp);

                    ResourceList wanted = ResourceList.Ship;
                    wanted.Timber -= Player.Resources.Timber;
                    wanted.Sheep -= Player.Resources.Sheep;
                    uiBankTrade.UpdateUI(wanted, Game.Bank, temp, Player.Ports, BankTrade.ETradeType.Ship);
                    ShowOverlay(uiBankTrade);
                    _QueuedAction = action;
                }
            }
            BuyDevcardAction buyDev = action as BuyDevcardAction;
            if (buyDev != null)
            {
                // popup autotrade window if we need any amount of gold
                if (buyDev.Resources.Count != 3)
                {
                    ResourceList temp = Player.Resources.Copy();
                    temp.SubtractCards(ResourceList.Devcard);

                    int possibleGold = Player.GoldCount(temp);

                    ResourceList wanted = new ResourceList();
                    //TODO: find solution for following problem:
                    // We cannot specify a list of wnated cards since discoveries acts as a joker
                    // For instance, when having 1 wheat, 1 discovery, the wanted list
                    // can consist of either 1 ore or 1 sheep
                    // Possible solution: 
                    // add jungle to wanted resources
                    // and OK on possible substitute
                    if (Player.Resources.Discoveries == 0)
                    {
                        wanted = ResourceList.Devcard;
                        wanted.Wheat -= Player.Resources.Wheat;
                        wanted.Ore -= Player.Resources.Ore;
                        wanted.Sheep -= Player.Resources.Sheep;
                    }
                    else
                    {
                        // add solution here
                    }
                    uiBankTrade.Action = buyDev;
                    uiBankTrade.UpdateUI(wanted, Game.Bank, temp, Player.Ports, BankTrade.ETradeType.Devcard);
                    ShowOverlay(uiBankTrade);
                    _QueuedAction = action;
                    return;
                }
                else
                {
                    // otherwise default on buying card instantly
                    buyDev.Resources = Player.GetDevcardResources();
                }
            }
            PlayDevcardAction playDev = action as PlayDevcardAction;
            if (playDev != null)
            {
                //Server.Instance.Say(playDev);
                if (playDev.DevCard is Soldier)
                {
                    ExecuteAction(playDev);
                    ExecuteAction(new PlaceRobberPirateAction() { GamePlayer = action.GamePlayer });
                    return;
                }
            }
            ClaimVictoryAction claimVictory = action as ClaimVictoryAction;
            if (claimVictory != null)
            {
            }
            RollDiceAction rollDice = action  as RollDiceAction;
            if (rollDice !=null)
            {
                action.GamePlayer = Game.PlayingPlayer;
                Server.Instance.Say(action);
                return;
            }

            ExecuteAction(action);
        }

        private void ExecuteAction(InGameAction action)
        {
            if (gameBoardViewPort.BeginAction(action))
            {
                uiBuildPalette.HideActions();
            }
            // if there is no associated behaviour, execute action immediately
            else
            {
                action.GamePlayer = Game.PlayingPlayer;
                Server.Instance.Say(action);
            }
        }

        void SetData()
        {
            uiPlayerHand.ItemsSource = gameBoardViewPort.CurrentGame.PlayerOnTurn.Resources;
            uiBuildPalette.DataContext = gameBoardViewPort.CurrentGame;
            uiBuildPalette.Board = gameBoardViewPort.Board.Board;
            uiAllPlayerStatus.DataContext = gameBoardViewPort.CurrentGame;
            uiBank.DataContext = gameBoardViewPort.CurrentGame;
            spTrade.DataContext = gameBoardViewPort.CurrentGame;
            uiBankTrade.DataContext = gameBoardViewPort.CurrentGame.PlayingPlayer;
            lbxGameLog.ItemsSource = gameBoardViewPort.CurrentGame.GameLog;
            lbxErrors.ItemsSource = _Errors;
            uiLooseCards.DataContext = gameBoardViewPort.CurrentGame;
        }

        void GameMain2_Loaded(object sender, RoutedEventArgs e)
        {
            ShowOverlay(uiNewHotSeat);
        }

        /// <summary>
        /// Toggle the visibility of th content of the overlay, inlcuding the ovrlay itself
        /// </summary>
        /// <param name="element">Content of the overlay</param>
        private void ShowOverlay(UIElement element)
        {
            SetOverlay(element, true);
        }
        private void RemoveOverlay(UIElement element)
        {
            cvOverlay.Visibility = Visibility.Collapsed;
            element.Visibility = Visibility.Collapsed;
        }
        private void SetOverlay(UIElement element, bool setDark)
        {
            if (setDark)
            {
                cvOverlay.Background = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0));
            }
            else
            {
                cvOverlay.Background = null;
            }
            cvOverlay.Visibility = Visibility.Visible;
            element.Visibility = Visibility.Visible;
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            Server.Instance.Say(new StartGameAction());
        }

        private void txtChat_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // prevent flooding using enter
                if (!String.IsNullOrEmpty(txtChat.Text))
                {
                    // Send chatmessage
                    Server.Instance.Say(new GameChatAction()
                    {
                        GamePlayer = gameBoardViewPort.CurrentGame.PlayingPlayer,
                        ChatMessage = txtChat.Text
                    });
                    // clear textbox afterwards
                    txtChat.Text = string.Empty;
                }
            }
        }

        private void imgGameLog_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lbxGameLog.Visibility = Visibility.Visible;
            lbxChatBox.Visibility = Visibility.Collapsed;
        }

        private void imgChat_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lbxGameLog.Visibility = Visibility.Collapsed;
            lbxChatBox.Visibility = Visibility.Visible;
        }
    }
}
