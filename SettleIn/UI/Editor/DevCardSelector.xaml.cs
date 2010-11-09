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

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for DevCardSelector.xaml
    /// </summary>
    public partial class DevCardSelector : UserControl
    {
        public DevCardSelector()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty DevStackProperty =
            DependencyProperty.Register("DevCardStack", typeof(StandardDevCardStack), typeof(DevCardSelector));

        public StandardDevCardStack DevCardStack
        {
            get { return (StandardDevCardStack)GetValue(DevCardSelector.DevStackProperty); }
            set { SetValue(DevCardSelector.DevStackProperty, value); }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbStandard_Click(object sender, RoutedEventArgs e)
        {
            ((StandardDevCardStack)DataContext).IsStandard = (bool)rbStandard.IsChecked;
        }

        private void rbExtended_Click(object sender, RoutedEventArgs e)
        {
            ((StandardDevCardStack)DataContext).IsExtended = (bool)rbExtended.IsChecked;
        }
    }
}
