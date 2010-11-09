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
using SettleIn.UI.Game;
using System.Collections.ObjectModel;
using SettleInCommon.Board;

namespace SettleIn.UI.Elements.Game
{
    /// <summary>
    /// Interaction logic for Trade.xaml
    /// </summary>
    public partial class Trade : UserControl
    {
        private TradeOfferAction _TradeOffer;
        public GameMain2.ExecuteGameActionHandler ExecuteGameAction;
        public event GameMain2.DoneHereHandler DoneHere;
        private void OnExecuteGameAction(InGameAction action)
        {
            if (ExecuteGameAction != null)
                ExecuteGameAction(action);
        }
        public TradeOfferAction TradeOffer
        {
            set { _TradeOffer = value; }
        }
        private void OnDoneHere(bool success)
        {
            if (DoneHere != null)
                DoneHere(success);
        }
        public Trade()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(Trade_Loaded);
            DataContextChanged += new DependencyPropertyChangedEventHandler(Trade_DataContextChanged);
        }

        void Trade_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            DataContext = new ObservableCollection<OfferResponse>()
            {
                new OfferResponse()
                {
                    Player = new 
                }
            }
             */
        }

        void Trade_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            XmlGame game = DataContext as XmlGame;
            ObservableCollection<OfferResponse> responses = new ObservableCollection<OfferResponse>();
            foreach (GamePlayer player in game.Players)
            {
                if (player != game.PlayingPlayer)
                {
                    OfferResponse response = new OfferResponse()
                    {
                        Player = player
                    };
                    responses.Add(response);
                }
            }
            responses[0].OfferedResources = new ResourceList(0, 0, 1, 0, 1, 1);
            responses[0].WantedResources = new ResourceList(0, 0, 0, 1, 1, 0);
            responses[0].Status = OfferStatus.Accepted;
            responses[1].OfferedResources = new ResourceList(1, 0, 0, 0, 0, 1);
            responses[1].WantedResources = new ResourceList(0, 0, 0, 0, 1, 0);
            responses[1].Status = OfferStatus.Declined;
            //responses[2].OfferedResources = new ResourceList(1, 0, 0, 1, 0, 1);
            //responses[2].WantedResources = new ResourceList(0, 2, 0, 0, 1, 0);
            //responses[2].Status = OfferStatus.CounterOffer;
            lbxOfferResponses.ItemsSource = responses;
        }

        private void btnOffer_Click(object sender, RoutedEventArgs e)
        {
            _TradeOffer.OfferedCards = uiOfferedCards.Resources1;
            _TradeOffer.WantedCards = uiWantedCards.Resources1;
            if (_TradeOffer.WantedCards.CountAll > 0 &&
                _TradeOffer.OfferedCards.CountAll > 0)
            {
                OnExecuteGameAction(_TradeOffer);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OnDoneHere(false);
        }

        public void SetNeutral()
        {

        }
    }
}
