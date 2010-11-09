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
using System.Collections.ObjectModel;
using SettleIn.UI.Editor;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for TerritoryMenu.xaml
    /// </summary>
    public partial class TerritoryMenu : UserControl
    {

        Grid _PreviousGrid;
        public TerritoryMenu()
        {
            InitializeComponent();
        }

        Territory _CurrentTerritory;

        public static readonly DependencyProperty TerritoriesProperty =
            DependencyProperty.Register("Territories", typeof(List<Territory>), typeof(TerritoryMenu));
        public List<Territory> Territories
        {
            get { return (List<Territory>)GetValue(TerritoryMenu.TerritoriesProperty); }
            set { SetValue(TerritoryMenu.TerritoriesProperty, value); }
        }

        public delegate void CurrentTerritoryChangedHandler(Territory t);
        public delegate void WantAddTerritoryHandler();
        public delegate void ViewToggledHandler();
        public event WantAddTerritoryHandler WantAddTerritory;
        public event ViewToggledHandler ViewToggled;
        public event CurrentTerritoryChangedHandler CurrentTerritoryChanged;
        private void OnViewToggled()
        {
            if (ViewToggled !=null) ViewToggled();
        }
        private void OnCurrentTerritoryChanged(Territory t)
        {
            if (CurrentTerritoryChanged != null)
            {
                CurrentTerritoryChanged(t);
            }
        }
 
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (WantAddTerritory != null) WantAddTerritory();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (_CurrentTerritory !=null)
            {
                ObservableCollection<Territory> ts = DataContext as ObservableCollection<Territory>;
                ts.Remove(_CurrentTerritory);
            }
        }

        private void btnMainland_Click(object sender, RoutedEventArgs e)
        {
            List<Territory> ts = DataContext as List<Territory>;
            if (ts.Count > 0)
            {
                //if (ts[0] is MainLand) 
                OnCurrentTerritoryChanged(ts[0]);
            }
        }

        private void btnToggleView_Click(object sender, RoutedEventArgs e)
        {
            ViewToggled();
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_PreviousGrid != null)
                _PreviousGrid.Background=new SolidColorBrush(Color.FromRgb(255,255,255));

            ((Grid)sender).Background = new SolidColorBrush(Color.FromRgb(255,0,0));
            _PreviousGrid = ((Grid)sender);
            _CurrentTerritory = (Territory)((Grid)sender).DataContext;
            OnCurrentTerritoryChanged(_CurrentTerritory);
        }

        private void btnButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Territory t = b.DataContext as Territory;
            _CurrentTerritory = t;
            OnCurrentTerritoryChanged(_CurrentTerritory);
        }
    }
}
