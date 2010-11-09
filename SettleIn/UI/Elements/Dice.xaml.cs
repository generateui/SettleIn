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
    /// Interaction logic for Dice.xaml
    /// </summary>
    public partial class Dice : UserControl
    {
        public int Number
        {
            get { return (int)this.GetValue(NumberProperty); }
            set 
            {
                if (value != Number)
                {
                    switch ((int)value)
                    {
                        case 1:
                            e1.Visibility = Visibility.Collapsed;
                            e2.Visibility = Visibility.Collapsed;
                            e3.Visibility = Visibility.Collapsed;
                            e4.Visibility = Visibility.Collapsed;
                            e5.Visibility = Visibility.Collapsed;
                            e6.Visibility = Visibility.Collapsed;
                            e7.Visibility = Visibility.Visible;
                            break;
                        case 2:
                            e1.Visibility = Visibility.Visible;
                            e2.Visibility = Visibility.Collapsed;
                            e3.Visibility = Visibility.Collapsed;
                            e4.Visibility = Visibility.Collapsed;
                            e5.Visibility = Visibility.Collapsed;
                            e6.Visibility = Visibility.Visible;
                            e7.Visibility = Visibility.Collapsed;
                            break;
                        case 3:
                            e1.Visibility = Visibility.Visible;
                            e2.Visibility = Visibility.Collapsed;
                            e3.Visibility = Visibility.Collapsed;
                            e4.Visibility = Visibility.Collapsed;
                            e5.Visibility = Visibility.Collapsed;
                            e6.Visibility = Visibility.Visible;
                            e7.Visibility = Visibility.Visible;
                            break;
                        case 4:
                            e1.Visibility = Visibility.Visible;
                            e2.Visibility = Visibility.Collapsed;
                            e3.Visibility = Visibility.Visible;
                            e4.Visibility = Visibility.Visible;
                            e5.Visibility = Visibility.Collapsed;
                            e6.Visibility = Visibility.Visible;
                            e7.Visibility = Visibility.Collapsed;
                            break;
                        case 5:
                            e1.Visibility = Visibility.Visible;
                            e2.Visibility = Visibility.Collapsed;
                            e3.Visibility = Visibility.Visible;
                            e4.Visibility = Visibility.Visible;
                            e5.Visibility = Visibility.Collapsed;
                            e6.Visibility = Visibility.Visible;
                            e7.Visibility = Visibility.Visible;
                            break;
                        case 6:
                            e1.Visibility = Visibility.Visible;
                            e2.Visibility = Visibility.Visible;
                            e3.Visibility = Visibility.Visible;
                            e4.Visibility = Visibility.Visible;
                            e5.Visibility = Visibility.Visible;
                            e6.Visibility = Visibility.Visible;
                            e7.Visibility = Visibility.Collapsed;
                            break;
                    }
                this.SetValue(NumberProperty, value); 
                }
            } 
        }
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
            "Number", typeof(int), typeof(Dice));

        Random random = new Random();
        public Dice()
        {
            InitializeComponent();
        }

        public void Roll()
        {
            //int num = (int)(Core.Instance.Random.NextDouble() * 6);
            //Number = num;
        }
    }
}
