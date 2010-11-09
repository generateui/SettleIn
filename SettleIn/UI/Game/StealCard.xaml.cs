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
using SettleInCommon.Gaming;
using SettleIn.UI.Elements;

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for StealCard.xaml
    /// </summary>
    public partial class StealCard : UserControl
    {
        public event GameMain2.DoneHereHandler DoneHere;
        private void OnDoneHere(bool success)
        {
            if (DoneHere != null)
                DoneHere(success);
        }

        Image _ActiveImage = null;
        GamePlayer _Victim = null;

        public GamePlayer Victim
        {
            get { return _Victim; }
        }
        public StealCard()
        {
            InitializeComponent();

            btnOK.IsEnabled = false;
        }

        public void SetUI(List<GamePlayer> players)
        {
            spPlayer1.Children.Clear();
            spPlayer2.Children.Clear();
            spPlayer3.Children.Clear();
            if (players.Count > 0)
            {
                lblPlayer1.Content = players[0].XmlPlayer.Name;
                for (int i = 0; i < players[0].Resources.Count; i++)
                {
                    spPlayer1.Children.Add(CreateImage(players[0]));
                }
            } 
            if (players.Count > 1)
            {
                lblPlayer2.Content = players[1].XmlPlayer.Name;
                for (int i = 0; i < players[1].Resources.Count; i++)
                {
                    spPlayer2.Children.Add(CreateImage(players[1]));
                }
            } 
            if (players.Count > 2)
            {
                lblPlayer3.Content = players[2].XmlPlayer.Name;
                for (int i = 0; i < players[2].Resources.Count; i++)
                {
                    spPlayer3.Children.Add(CreateImage(players[2]));
                }
            }
        }
        private Image CreateImage(GamePlayer player)
        {
            Image image = new Image()
            {
                Source = (ImageSource)Core.Instance.Icons["IconTimber"],
                Margin = new Thickness(0, 10, 0, 0),
                Tag=player
            };
            image.MouseUp += new MouseButtonEventHandler(image_MouseUp);

            return image;
        }

        void image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_ActiveImage != null)
            {
                _ActiveImage.Margin = new Thickness(0, 10, 0, 0);
            }
            _ActiveImage = (Image)sender;
            _ActiveImage.Margin = new Thickness(0, 0, 0, 0);
            _Victim = (GamePlayer)_ActiveImage.Tag;
            btnOK.IsEnabled = true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            OnDoneHere(true);
        }

        private void btnNoStal_Click(object sender, RoutedEventArgs e)
        {
            OnDoneHere(false);
        }
    }
}
