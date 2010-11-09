using System;
using System.Collections.Generic;
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
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Gaming;
using System.ComponentModel;
using SettleInCommon.Board;
using SettleInCommon.Gaming.GamePhases;
using SettleInCommon.Gaming.TurnPhases;

namespace SettleIn.UI.Elements.Game
{
    /// <summary>
    /// Interaction logic for BuildPallette.xaml
    /// </summary>
    public partial class BuildPallette : UserControl
    {
        public GameMain2.ExecuteGameActionHandler ExecuteGameAction;
        public event GameMain2.DoneHereHandler DoneHere;
        private XmlBoard _Board;
        private XmlGame Game
        {
            get
            {
                return (XmlGame)DataContext;
            }
        }
        private void OnDoneHere(bool success)
        {
            if (DoneHere != null)
                DoneHere(success);
        }
        public XmlBoard Board
        {
            set { _Board = value; }
        }

        private void OnExecuteGameAction(InGameAction action)
        {
            if (ExecuteGameAction != null)
                ExecuteGameAction(action);
        }

        public BuildPallette()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(BuildPallette_Loaded);
            DataContextChanged += new DependencyPropertyChangedEventHandler(BuildPallette_DataContextChanged);
            uiDevcards.ExecuteGameAction += new GameMain2.ExecuteGameActionHandler(uiDevcards_ExecuteGameAction);
        }

        public void SetVolcanoDice()
        {
            btnVolcanoDice.Visibility = Visibility.Visible;
            dices.Visibility = Visibility.Hidden;
        }
        public void ShowVolcanoDiceResult(RollVolcanoDiceAction rollVolcano)
        {
            volcanoDice.Number = rollVolcano.Dice;
            btnVolcanoOK.Visibility = Visibility.Visible;
        }
        public void UnSetVolcanoDice()
        {
            btnVolcanoDice.Visibility = Visibility.Hidden;
            dices.Visibility = Visibility.Visible;
            btnVolcanoOK.Visibility = Visibility.Hidden;
        }

        private XmlGame GetGame
        {
            get { return DataContext as XmlGame; }
        }

        private void Dices_MouseUp(object sender, MouseButtonEventArgs e)
        {
            /*
            if ((GetGame.Phase.GetType() == typeof(PlayTurnsGamePhase) &&
                (GetGame.TurnPhase == ETurnPhase.BeforeDiceRoll ||
                 GetGame.TurnPhase == ETurnPhase.DiceRoll)) ||
                GetGame.Phase == typeof(DetermineFirstPlayerGamePhase))
             */
            {
                OnExecuteGameAction(new RollDiceAction()
                {
                    GamePlayer = GetGame.PlayerOnTurn,
                    DateTime = DateTime.Now,
                    GameID = GetGame.ID
                });
            }
        }

        public void SetStatus(string statusMessage)
        {
            lblGameStatus.Content = statusMessage;
            gridStatusAndDice.Visibility = Visibility.Visible;
        }

        public void SetStatus(InGameAction action)
        {
            RollDiceAction rollDice = action as RollDiceAction;
            if (rollDice != null)
            {

            }
        }

        public void SetStatus(TurnPhase turnPhase)
        {
            if (turnPhase is BeforeDiceRollTurnPhase)
            {
                SetBeforeDiceRoll();
                dices.Visibility = Visibility.Visible;
            }
            if (turnPhase is RollDiceTurnPhase)
            {
                dices.Visibility = Visibility.Hidden;
                SetAfterDiceRoll();
            }
            if (turnPhase is TradingTurnPhase)
            {
                dices.Visibility = Visibility.Hidden;
                SetAfterDiceRoll();
            }
            if (turnPhase is BuildTurnPhase)
            {
                dices.Visibility = Visibility.Hidden;
                SetAfterDiceRoll();
            }
        }
        private void ShowAll()
        {
            gridStatusAndDice.Visibility = Visibility.Visible;
            uiActionsQueue.Visibility = Visibility.Visible;
            uiPorts.Visibility = Visibility.Visible;
            uiDevcards.Visibility = Visibility.Visible;
            uiPlayedDevcards.Visibility = Visibility.Visible;
            imgBank.Visibility = Visibility.Visible;
            cvBuyDevcard.Visibility = Visibility.Visible;
            cvCity.Visibility = Visibility.Visible;
            cvTown.Visibility = Visibility.Visible;
            imgTradePlayer.Visibility = Visibility.Visible;
            imgEndTurn.Visibility = Visibility.Visible;
            imgClaimVictory.Visibility = Visibility.Visible;
            cvShip.Visibility = Visibility.Visible;
            cvMoveShip.Visibility = Visibility.Visible;
            cvRoad.Visibility = Visibility.Visible;
            dices.Visibility = Visibility.Visible;
        }
        private void HideAll()
        {
            gridStatusAndDice.Visibility = Visibility.Hidden;
            uiActionsQueue.Visibility = Visibility.Hidden;
            uiPorts.Visibility = Visibility.Hidden;
            uiDevcards.Visibility = Visibility.Hidden;
            uiPlayedDevcards.Visibility = Visibility.Hidden;
            imgBank.Visibility = Visibility.Hidden;
            cvBuyDevcard.Visibility = Visibility.Hidden;
            cvCity.Visibility = Visibility.Hidden;
            cvTown.Visibility = Visibility.Hidden;
            imgTradePlayer.Visibility = Visibility.Hidden;
            imgEndTurn.Visibility = Visibility.Hidden;
            imgClaimVictory.Visibility = Visibility.Hidden;
            cvShip.Visibility = Visibility.Hidden;
            cvMoveShip.Visibility = Visibility.Hidden;
            cvRoad.Visibility = Visibility.Hidden;
            dices.Visibility = Visibility.Hidden;
        }
        public void SetStatus(GamePhase gamePhase)
        {
            if (gamePhase is DetermineFirstPlayerGamePhase)
            {
                HideAll();
                gridStatusAndDice.Visibility = Visibility.Visible;
                uiActionsQueue.Visibility = Visibility.Visible;
                dices.Visibility = Visibility.Visible;
                return;
            }
            if (gamePhase is PlacePortGamePhase)
            {
                HideAll();
                gridStatusAndDice.Visibility = Visibility.Visible;
                uiActionsQueue.Visibility = Visibility.Visible;
                return;
            } 
            if (gamePhase is PlacementGamePhase)
            {
                HideAll();
                gridStatusAndDice.Visibility = Visibility.Visible;
                uiActionsQueue.Visibility = Visibility.Visible;
                uiPorts.Visibility = Visibility.Visible;
                cvBuyDevcard.Visibility = Visibility.Visible;
                imgTradePlayer.Visibility = Visibility.Visible;
                cvShip.Visibility = Visibility.Visible;
                cvRoad.Visibility = Visibility.Visible;
                return;
            }
            PlayTurnsGamePhase playTurns = gamePhase as PlayTurnsGamePhase;
            if (playTurns != null)
            {
                ShowAll();

                SetStatus(playTurns.TurnPhase);
            }
            if (gamePhase is EndedGamePhase)
            {
                gridStatusAndDice.Visibility = Visibility.Visible;
                uiActionsQueue.Visibility = Visibility.Visible;
                uiPorts.Visibility = Visibility.Visible;
                uiDevcards.Visibility = Visibility.Visible;
                uiPlayedDevcards.Visibility = Visibility.Visible;
                imgBank.Visibility = Visibility.Visible;
                cvBuyDevcard.Visibility = Visibility.Visible;
                cvCity.Visibility = Visibility.Visible;
                cvTown.Visibility = Visibility.Visible;
                imgTradePlayer.Visibility = Visibility.Visible;
                cvShip.Visibility = Visibility.Visible;
                cvMoveShip.Visibility = Visibility.Visible;
                cvRoad.Visibility = Visibility.Visible;
                dices.Visibility = Visibility.Hidden;
                return;
            }
        }

        void BuildPallette_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            XmlGame game = DataContext as XmlGame;
            if (game != null)
            {
                game.PropertyChanged += new PropertyChangedEventHandler(game_PropertyChanged);
                uiActionsQueue.ItemsSource = game.ActionsQueue;
                /*
                GetGame.PlayerOnTurn.Resources.PropertyChanged += new PropertyChangedEventHandler(Resources_PropertyChanged);
                GetGame.PlayerOnTurn.PropertyChanged +=
                    new PropertyChangedEventHandler(PlayingPlayer_PropertyChanged);
                uiDevcards.DataContext = GetGame.PlayerOnTurn.DevCards;
                uiPorts.ItemsSource = GetGame.PlayerOnTurn.Ports;
                uiPlayedDevcards.ItemsSource = GetGame.PlayerOnTurn.PlayedDevcards;
                 */
            }
        }

        void uiDevcards_ExecuteGameAction(InGameAction action)
        {
            //bubble event up from ui devcards
            OnExecuteGameAction(action);
        }

        void BuildPallette_Loaded(object sender, RoutedEventArgs e)
        {
        }

        void game_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //this property only changes in a hotseat game
            if (e.PropertyName == "PlayerOnTurn")
            {
                GetGame.PlayingPlayer = GetGame.PlayerOnTurn;
                GetGame.PlayerOnTurn.Resources.PropertyChanged += new PropertyChangedEventHandler(Resources_PropertyChanged);
                GetGame.PlayerOnTurn.PropertyChanged +=
                    new PropertyChangedEventHandler(PlayingPlayer_PropertyChanged);
                uiDevcards.DataContext = GetGame.PlayerOnTurn.DevCards;
                uiPorts.ItemsSource = GetGame.PlayerOnTurn.Ports;
                uiPlayedDevcards.ItemsSource = GetGame.PlayerOnTurn.PlayedDevcards;
            }
        }

        void SetBeforeDiceRoll()
        {
            // disable anything but devcards (robber) and rolling dice
            imgBank.Opacity = 0.3;
            imgTradePlayer.Opacity = 0.3;
            imgEndTurn.Opacity = 0.3;
            imgBuydevcard.Opacity = 0.3;
            imgShip.Opacity = 0.3;
            imgTown.Opacity = 0.3;
            imgCity.Opacity = 0.3;
            imgRoad.Opacity = 0.3;
            dices.Visibility = Visibility.Visible;
        }

        void SetAfterDiceRoll()
        {
            if (GetGame.PlayingPlayer.CanTradeWithBank)
            {
                imgBank.Opacity = 1.0;
            }
            else
            {
                imgBank.Opacity = 0.3;
            }
            imgTradePlayer.Opacity = 1.0;
            imgEndTurn.Opacity = 1.0;
            
            SetDevRoadShips();

            if (GetGame.PlayingPlayer.CanPayDevcard && 
                GetGame.DevCards.Count > 0)
            {
                imgBuydevcard.Opacity = 1.0;
                imgDevcardTrade1.Visibility = GetVisibility(GetGame.PlayingPlayer.DevcardNeededGold() > 0);
                imgDevcardTrade2.Visibility = GetVisibility(GetGame.PlayingPlayer.DevcardNeededGold() > 1);
            }
            else
            {
                imgBuydevcard.Opacity = 0.3;
                imgDevcardTrade1.Visibility = GetVisibility(false);
                imgDevcardTrade2.Visibility = GetVisibility(false);
            }
            if (GetGame.PlayingPlayer.CanPayShip && 
                GetGame.PlayingPlayer.CanBuildShip(Game, _Board))
            {
                imgShip.Opacity = 1.0;
                imgShipTrade1.Visibility = GetVisibility(GetGame.PlayingPlayer.ShipNeededGold() > 0);
                imgShipTrade2.Visibility = GetVisibility(GetGame.PlayingPlayer.ShipNeededGold() > 1);

            }
            else
            {
                imgShip.Opacity = 0.3;
                imgShipTrade1.Visibility = GetVisibility(false);
                imgShipTrade2.Visibility = GetVisibility(false);
            }
            if (GetGame.PlayingPlayer.CanMoveShip(Game))
            {
                imgMoveShip.Opacity = 1.0;
            }
            else
            {
                imgMoveShip.Opacity = 0.3;
            }

            if (GetGame.PlayingPlayer.CanPayRoad &&
                GetGame.PlayingPlayer.CanBuildRoad(Game,_Board))
            {
                imgRoad.Opacity = 1.0;
                imgRoadTrade1.Visibility = GetVisibility(GetGame.PlayingPlayer.RoadNeededGold() > 0);
                imgRoadTrade2.Visibility = GetVisibility(GetGame.PlayingPlayer.RoadNeededGold() > 1);

            }
            else
            {
                imgRoad.Opacity =0.3;
                imgRoadTrade1.Visibility = GetVisibility(false);
                imgRoadTrade2.Visibility = GetVisibility(false);
            }
            if (GetGame.PlayingPlayer.CanPayTown &&
                GetGame.PlayingPlayer.CanBuildTown(Game, _Board))
            {
                imgTown.Opacity = 1.0;
                imgTownTrade1.Visibility = GetVisibility(GetGame.PlayingPlayer.TownNeededGold() > 0);
                imgTownTrade2.Visibility = GetVisibility(GetGame.PlayingPlayer.TownNeededGold() > 1);
                imgTownTrade3.Visibility = GetVisibility(GetGame.PlayingPlayer.TownNeededGold() > 2);
                imgTownTrade4.Visibility = GetVisibility(GetGame.PlayingPlayer.TownNeededGold() > 3);
            }
            else
            {
                imgTown.Opacity = 0.3;
                imgTownTrade1.Visibility = GetVisibility(false);
                imgTownTrade2.Visibility = GetVisibility(false);
                imgTownTrade3.Visibility = GetVisibility(false);
                imgTownTrade4.Visibility = GetVisibility(false);
            }

            if (GetGame.PlayingPlayer.CanPayCity &&
                GetGame.PlayingPlayer.CanBuildCity())
            {
                imgCity.Opacity = 1.0;
                imgCityTrade1.Visibility = GetVisibility(GetGame.PlayingPlayer.CityNeededGold() > 0);
                imgCityTrade2.Visibility = GetVisibility(GetGame.PlayingPlayer.CityNeededGold() > 1);
                imgCityTrade3.Visibility = GetVisibility(GetGame.PlayingPlayer.CityNeededGold() > 2);
                imgCityTrade4.Visibility = GetVisibility(GetGame.PlayingPlayer.CityNeededGold() > 3);
                imgCityTrade5.Visibility = GetVisibility(GetGame.PlayingPlayer.CityNeededGold() > 4);
            }
            else
            {
                imgCity.Opacity = 0.3;
                imgCityTrade1.Visibility = GetVisibility(false);
                imgCityTrade2.Visibility = GetVisibility(false);
                imgCityTrade3.Visibility = GetVisibility(false);
                imgCityTrade4.Visibility = GetVisibility(false);
                imgCityTrade5.Visibility = GetVisibility(false);
            }

            dices.Visibility = Visibility.Hidden;
            if (GetGame.PlayingPlayer.CanTradeWithBank)
            {
                imgBank.Opacity = 1.0;
            }
            else
            {
                imgBank.Opacity = 0.3;
            }
        }

        private Visibility GetVisibility(bool b)
        {
            if (b)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        void Resources_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CountAll")
            {
                SetAfterDiceRoll();
            }
        }

        void PlayingPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            XmlGame game = DataContext as XmlGame;
            if (e.PropertyName == "VictoryPoints")
            {
                if (game.PlayingPlayer.VictoryPoints >= game.Settings.VpToWin)
                    imgClaimVictory.Opacity = 1;
            }
            if (e.PropertyName == "DevRoadShips")
            {
                SetAfterDiceRoll();
            }
        }

        private void SetDevRoadShips()
        {
            if (GetGame.PlayingPlayer.DevRoadShips == 0)
            {
                imgRoadRb1.Visibility = GetVisibility(false);
                imgRoadRb2.Visibility = GetVisibility(false);
                imgShipRb1.Visibility = GetVisibility(false);
                imgShipRb2.Visibility = GetVisibility(false);
            }
            if (GetGame.PlayingPlayer.DevRoadShips > 0)
            {
                imgRoadRb1.Visibility = GetVisibility(true);
                imgShipRb1.Visibility = GetVisibility(true);
                imgRoadRb2.Visibility = GetVisibility(false);
                imgShipRb2.Visibility = GetVisibility(false);
                
            }
            if (GetGame.PlayingPlayer.DevRoadShips > 1)
            {
                imgRoadRb2.Visibility = GetVisibility(true);
                imgShipRb2.Visibility = GetVisibility(true);
            }
        }
        private void imgTown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.PlayingPlayer.CanPayTown)
            {
                OnExecuteGameAction(new BuildTownAction());
                e.Handled = true;
            }
        }

        private void imgRoad_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.PlayingPlayer.CanPayRoad)
            {
                OnExecuteGameAction(new BuildRoadAction());
                e.Handled = true;
            }
        }

        private void imgShip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.PlayingPlayer.CanPayShip)
            {
                OnExecuteGameAction(new BuildShipAction());
                e.Handled = true;
            }
        }

        private void imgCity_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            if (Game.PlayingPlayer.CanPayCity)
            {
                OnExecuteGameAction(new BuildCityAction());
                e.Handled = true;
            }
        }

        private void imgBuydevcard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.PlayingPlayer.CanPayDevcard)
            {
                BuyDevcardAction buyDev = new BuyDevcardAction()
                {
                    GamePlayer = GetGame.PlayingPlayer,
                    Resources = GetGame.PlayingPlayer.GetDevcardResources()
                };
                OnExecuteGameAction(buyDev);
                e.Handled = true;
            }
        }

        private void imgBank_MouseDown(object sender, MouseButtonEventArgs e)
        {
            XmlGame game = DataContext as XmlGame;
            //if (game.Phase == EGamePhase.InGame)
            {
                OnExecuteGameAction(new TradeBankAction());
                e.Handled = true;
            }
        }

        private void imgEndTurn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            XmlGame game = DataContext as XmlGame;
            //if (game.Phase.GetType() == typeof())
            {
                OnExecuteGameAction(new EndTurnAction());
                e.Handled = true;
            }
        }

        private void imgClaimVictory_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Game.PlayingPlayer.CanClaimVictory(Game))
            {
                OnExecuteGameAction(new ClaimVictoryAction());
                e.Handled = true;
            }
        }

        private void imgTradePlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            XmlGame game = DataContext as XmlGame;
            if (game.Phase is PlayTurnsGamePhase)
            {
                if (Game.PlayingPlayer.Resources.Count > 0)
                    OnExecuteGameAction(new TradeOfferAction());
                e.Handled = true;
            }
        }

        public void HideActions()
        {
            spActions.Visibility = Visibility.Collapsed;
            uiDevcards.Visibility = Visibility.Collapsed;
        }
        public void ShowActions()
        {
            spActions.Visibility = Visibility.Visible;
            uiDevcards.Visibility = Visibility.Visible;
        }

        private void imgMoveShip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            XmlGame game = DataContext as XmlGame;
            if (game.PlayerOnTurn.CanMoveShip(game))
            {
                OnExecuteGameAction(new MoveShipAction());
                e.Handled = true;
            }
        }

        private void btnVolcanoOK_Click(object sender, RoutedEventArgs e)
        {
            OnDoneHere(true);
        }

        private void btnVolcanoDice_Click(object sender, RoutedEventArgs e)
        {
            OnExecuteGameAction(new RollVolcanoDiceAction());
            e.Handled = true;
        }
    }
}
