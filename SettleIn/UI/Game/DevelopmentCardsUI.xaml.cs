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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SettleInCommon.Board;
using SettleInCommon.Gaming.DevCards;
using SettleIn.UI.Elements;
using SettleInCommon.Actions.InGame;
using SettleInCommon.Actions.TurnActions;
using System.Windows.Media.Effects;
using System.Windows.Interop;
using System.Diagnostics;

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for DevelopmentCardsUI.xaml
    /// </summary>
    public partial class DevelopmentCardsUI : UserControl
    {
        public event GameMain2.ExecuteGameActionHandler ExecuteGameAction;
        private void OnExecuteGameAction(InGameAction action)
        {
            if (ExecuteGameAction != null)
                ExecuteGameAction(action);
        }
        public DevelopmentCardsUI()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DevelopmentCard dc = (DevelopmentCard)((Button)sender).Tag;

            ExecuteAction(dc);
        }

        private void ExecuteAction(DevelopmentCard devCard)
        {
            PlayDevcardAction playDevcard = new PlayDevcardAction()
            {
                DevCard = devCard
            };
            OnExecuteGameAction(playDevcard);
            uiDevcards.Height = double.NaN;
        }

        private void spmain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (uiDevcards.Height.Equals(Double.NaN))
                uiDevcards.Height = 0;
            else
                uiDevcards.Height = double.NaN;
        }

        /// <summary>
        /// Executes when user selected a resource and clicks "play" button on a monopoly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mono_Click(object sender, RoutedEventArgs e)
        {
            // Sinc we mak use of datatemplates of which multiple instance can coxeist, we need
            // to retrieve the checked radiobutton within the same panel as the button

            // get button where player clicked
            Button b = sender as Button;
            Debug.Assert(b != null);

            // get the parent stackpanel (spMonoContainr)
            StackPanel parent = VisualTreeHelper.GetParent(b) as StackPanel;
            Debug.Assert(parent != null);

            // get the stackpanel within spMonoContainer (spMonoOptions)
            StackPanel monoOptions = parent.FindName("spMonoOptions") as StackPanel;
            Debug.Assert(monoOptions != null);

            RadioButton checkedRadioButton = null;
            
            foreach (UIElement ui in monoOptions.Children)
            {
                RadioButton rb = ui as RadioButton;
                if (rb != null)
                {
                    if (rb.IsChecked == true)
                    {
                        // found our checked radiobutton!
                        checkedRadioButton = rb;

                        //exit loop, we can have only one checked radiobutton
                        break;
                    }
                }
            }
            Debug.Assert(checkedRadioButton != null);
                
            // create a new monopoly action using our retrieved resource and send it 
            Monopoly mono = (Monopoly)monoOptions.DataContext;
            mono.Resource = (EResource)checkedRadioButton.Tag;

            ExecuteAction(mono);
        }

        private void btnVP_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            StackPanel p = (StackPanel)VisualTreeHelper.GetParent(b);
            StackPanel px = (StackPanel)VisualTreeHelper.GetParent(p);
            DevelopmentCard dc = (DevelopmentCard)px.DataContext;
            ExecuteAction(dc);
        }

        private void btnRoadBuilding_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            StackPanel p = (StackPanel)VisualTreeHelper.GetParent(b);
            StackPanel px = (StackPanel)VisualTreeHelper.GetParent(p);
            DevelopmentCard dc = (DevelopmentCard)px.DataContext;

            ExecuteAction(dc);
        }

        private void btnSoldier_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            StackPanel p = (StackPanel)VisualTreeHelper.GetParent(b);
            StackPanel px = (StackPanel)VisualTreeHelper.GetParent(p);
            DevelopmentCard dc = (DevelopmentCard)px.DataContext;
            ExecuteAction(dc);
        }

        private void playClicked(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            SmallResourcePicker picker = sender as SmallResourcePicker;

            StackPanel p = (StackPanel)VisualTreeHelper.GetParent(picker);

            YearOfPlenty yop = p.DataContext as YearOfPlenty;

            yop.Resources = picker.PickedResources;

            ExecuteAction(yop);
        }
    }
}
