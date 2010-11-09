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
using System.Windows.Threading;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for Dices.xaml
    /// </summary>
    public partial class Dices : UserControl
    {
        public DispatcherTimer time = new DispatcherTimer();
        public Dices()
        {
            InitializeComponent();
            time.Interval = new TimeSpan(0, 0, 0, 0, 250);
            time.Tick += new EventHandler(time_Tick);
            time.IsEnabled = true;
        }

        void time_Tick(object sender, EventArgs e)
        {
            //RollDices();
        }
        public delegate DiceRoll Rolledhandler(DiceRoll roll);
        public event Rolledhandler Rolled;

        private void OnRolled(DiceRoll roll)
        {
            if (Rolled != null) Rolled(roll);
        }

        private void spDices_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RollDices();
            DiceRoll roll = new DiceRoll();
            roll.Roll1 = dice1.Number;
            roll.Roll2 = dice2.Number;
            OnRolled(roll);
            time.IsEnabled = false;
            spDices.Background = new SolidColorBrush(Color.FromArgb(50,255,0,0));
        }
        private void RollDices()
        {
            dice1.Roll();
            dice2.Roll();
        }

    }
}
