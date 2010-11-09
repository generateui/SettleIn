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

namespace SettleIn.UI.Elements.Game
{
    /// <summary>
    /// Interaction logic for PickGoldResource.xaml
    /// </summary>
    public partial class PickGoldResource : UserControl
    {
        public event GameMain2.DoneHereHandler DoneHere;

        private void OnDoneHere(bool success)
        {
            if (DoneHere != null)
                DoneHere(success);
        }

        private ResourceList _PickedResources = new ResourceList();
        private int _MaxResources;
        private bool IsEven
        {
            get
            {
                return _PickedResources.CountAll == _MaxResources;
            }
        }

        public int MaxResources
        {
            get { return _MaxResources; }
            set { _MaxResources = value; }
        }

        public PickGoldResource()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PickGoldResource_Loaded);
            _PickedResources.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_PickedResources_PropertyChanged);
        }

        void _PickedResources_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (IsEven) btnDone.IsEnabled = true;
            else btnDone.IsEnabled = false;
        }

        void PickGoldResource_Loaded(object sender, RoutedEventArgs e)
        {
            spPickedResources.DataContext = _PickedResources;
        }

        private void imgSheep_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Sheep++; }

        private void imgTimber_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Timber++; }

        private void imgWheat_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Wheat++; }

        private void imgOre_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Ore++; }

        private void imgClay_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Clay++;  }

        private void imgWheatPicked_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Wheat--; }

        private void imgOrePicked_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Ore--; }

        private void imgSheepPicked_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Sheep--; }

        private void imgClayPicked_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Clay--; }

        private void imgTimberPicked_MouseUp(object sender, MouseButtonEventArgs e)
        { _PickedResources.Timber--; }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
