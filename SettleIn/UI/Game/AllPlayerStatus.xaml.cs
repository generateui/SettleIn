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

namespace SettleIn.UI.Elements.Game
{
    /// <summary>
    /// Interaction logic for AllPlayerStatus.xaml
    /// </summary>
    public partial class AllPlayerStatus : UserControl
    {
        public AllPlayerStatus()
        {
            InitializeComponent();

            DataContextChanged += new DependencyPropertyChangedEventHandler(AllPlayerStatus_DataContextChanged);
        }

        void AllPlayerStatus_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            XmlGame game = DataContext as XmlGame;
            game.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(game_PropertyChanged);
            this.Height = (game.Players.Count * 80) + 40;
        }

        void game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
    }
}
