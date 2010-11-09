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
using SettleInCommon.Gaming;
using SettleInCommon.Board;
using SettleIn.UI.Elements;

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for LooseCards.xaml
    /// </summary>
    public partial class LooseCards : UserControl
    {
        private List<LooseCardWrapper> _LooseCards = new List<LooseCardWrapper>();

        public List<LooseCardWrapper> LostCards
        {
            get { return _LooseCards; }
        }

        XmlGame _Game;
        public event GameMain2.DoneHereHandler DoneHere;

        public LooseCards()
        {
            InitializeComponent();

            DataContextChanged += new DependencyPropertyChangedEventHandler(LooseCards_DataContextChanged);
        }

        private void OnDoneHere(bool success)
        {
            if (DoneHere != null)
                DoneHere(success);
        }

        void  LooseCards_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _Game = (XmlGame) DataContext;
        }

        public void SetActions(IEnumerable<LooseCardsAction> looseCards)
        {  
            _LooseCards = new List<LooseCardWrapper>();
            
            // Add a looseCardsAction for each player. If a player does not need to loos cards, 
            foreach (GamePlayer player in _Game.Players)
            {
                LooseCardsAction lost = looseCards.Where(lc => lc.GamePlayer == player).FirstOrDefault();
                if (lost == null)
                {
                    _LooseCards.Add(new LooseCardWrapper() 
                    { 
                        Player=player, 
                        ResourcesLost = null,
                        PlayerResources = null,
                        ResourcesToLoose = 0
                    });
                }
                else
                {
                    // Make sure ResourcesLost is not null
                    _LooseCards.Add(new LooseCardWrapper()
                    {
                        Player = player,
                        PlayerResources = player.Resources.Copy(),
                        ResourcesLost = new ResourceList(),
                        ResourcesToLoose = player.Resources.HalfCount(),
                        Loosecards = lost
                    });
                }
            }
            uiPlayer.ItemsSource = _LooseCards;
            btnDone.IsEnabled = false;
        }

        /// 
        ///
        ///
        ///
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            
            // Get resource in question
            Image i = (Image)b.Content;
            EResource res = (EResource)i.DataContext;
            

            ItemsControl ic = VisualTreeHelper.GetParent(b) as ItemsControl;
            DependencyObject dos = VisualTreeHelper.GetParent(b);
            DependencyObject dos2 = VisualTreeHelper.GetParent(dos);
            DependencyObject dos3 = VisualTreeHelper.GetParent(dos2);
            DependencyObject dos4 = VisualTreeHelper.GetParent(dos3);
            ItemsControl dos5 = VisualTreeHelper.GetParent(dos4) as ItemsControl;
            DependencyObject dos6 = VisualTreeHelper.GetParent(dos5);
            LooseCardWrapper lca = dos5.DataContext as LooseCardWrapper;
            if (lca.ResourcesLost != null)
            {
                lca.ResourcesLost.AddResource(res);
                lca.PlayerResources.Remove(res);
            }
            SetButtonEnabled();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            // Get resource in question
            Image i = (Image)b.Content;
            EResource res = (EResource)i.DataContext;


            ItemsControl ic = VisualTreeHelper.GetParent(b) as ItemsControl;
            DependencyObject dos = VisualTreeHelper.GetParent(b);
            DependencyObject dos2 = VisualTreeHelper.GetParent(dos);
            DependencyObject dos3 = VisualTreeHelper.GetParent(dos2);
            DependencyObject dos4 = VisualTreeHelper.GetParent(dos3);
            ItemsControl dos5 = VisualTreeHelper.GetParent(dos4) as ItemsControl;
            DependencyObject dos6 = VisualTreeHelper.GetParent(dos5);
            LooseCardWrapper lca = dos5.DataContext as LooseCardWrapper;
            if (lca.ResourcesLost != null)
            {
                lca.ResourcesLost.RemoveResource(res);
                lca.PlayerResources.AddResource(res);
            }
            SetButtonEnabled();
        }
        private void SetButtonEnabled()
        {
            if (IsValid())
            {
                btnDone.IsEnabled = true;
            }
            else
            {
                btnDone.IsEnabled = false;
            }

        }
        private bool IsValid()
        {
            foreach (LooseCardWrapper looseCards in _LooseCards)
            {
                if (looseCards.ResourcesLost != null)
                {
                    if (looseCards.ResourcesLost.CountAllExceptDiscovery != looseCards.ResourcesToLoose) return false;
                }
            }

            // No invalid action can be found, return true
            return true;
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            OnDoneHere(true);
        }
    }
}
