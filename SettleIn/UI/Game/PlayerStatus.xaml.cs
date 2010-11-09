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

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for PlayerStatus.xaml
    /// </summary>
    public partial class PlayerStatus : UserControl
    {
        public PlayerStatus()
        {
            InitializeComponent();
        }
        /*
        public static readonly DependencyProperty GameProperty = 
            DependencyProperty.Register("Game", typeof(Game), typeof(PlayerStatus));
        public Game Game
        {
            get { return (Game)GetValue(PlayerStatus.GameProperty); }
            set { SetValue(PlayerStatus.GameProperty, value); }
        }
         */
    }
}
