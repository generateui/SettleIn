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
using SettleInCommon.Board;
using SettleIn.UI.Elements;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Gaming;

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for PickGoldAll.xaml
    /// </summary>
    public partial class PickGoldAll : UserControl
    {
        private ResourceList _BankResources;
        public event GameMain2.DoneHereHandler DoneHere;
        private List<PickGoldAction> _PickGoldActions;

        private void OnDoneHere(bool success)
        {
            if (DoneHere != null)
                DoneHere(success);
        }
        public ResourceList BankResources
        {
            get { return _BankResources; }
            set { _BankResources = value; }
        }
        public List<PickGoldAction> PickedGold
        {
            get { return _PickGoldActions; }
        }
        public PickGoldAll()
        {
            InitializeComponent();
        }
        private void imgTimber_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                PickGoldAction pickGold = image.DataContext as PickGoldAction;
                pickGold.Resources.Add((EResource)image.Tag);

                // Add resource to list of resources
            }
        }
        public void SetData(XmlGame game)
        {
            // Create a list of pickgoldActions. If player should not pick gold, add it anyways but 
            // set the amount to 0.
            List<PickGoldAction> pickGoldActions = new List<PickGoldAction>();
            foreach (GamePlayer player in game.Players)
            {
                var shouldPickGold = game.ActionsQueue.OfType<PickGoldAction>()
                    .Where(pga => pga.Sender == player.XmlPlayer.ID)
                    .FirstOrDefault();
                if (shouldPickGold == null)
                {
                    pickGoldActions.Add(new PickGoldAction()
                    {
                        Sender = player.XmlPlayer.ID,
                        Amount = 0
                    });
                }
                else
                {
                    pickGoldActions.Add(shouldPickGold);
                }
            }
            _PickGoldActions = pickGoldActions;
            uiPlayer.ItemsSource = _PickGoldActions;
            _BankResources = game.Bank;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            DependencyObject el = VisualTreeHelper.GetParent(b);
            DependencyObject el1 = VisualTreeHelper.GetParent(el);
            DependencyObject el2 = VisualTreeHelper.GetParent(el1);
            DependencyObject el3 = VisualTreeHelper.GetParent(el2);
            DependencyObject el4 = VisualTreeHelper.GetParent(el3);
            PickGoldAction pickGold = ((ItemsControl)el4).DataContext as PickGoldAction;
            pickGold.Resources.Remove((EResource)b.DataContext);
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            OnDoneHere(true);
        }
    }
}
