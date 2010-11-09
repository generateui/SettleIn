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
using System.Windows.Shapes;

using SettleInCommon.Board;

namespace SettleIn
{
    /// <summary>
    /// Interaction logic for AddIsland.xaml
    /// </summary>
    public partial class AddIsland : UserControl
    {
        private int _CurrentTerritoryID;

        public int TerritoryID
        {
            set { _CurrentTerritoryID = value; }
        }
        
        public AddIsland()
        {
            InitializeComponent();
        }

        public delegate void IslandAddedHandler(Territory t);
        public event IslandAddedHandler IslandAdded;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseSave();
        }
        private void CloseSave()
        {
            Territory t = new Territory()
            {
                Name = txtName.Text,
                IsMainland = (bool)rbMainland.IsChecked,
                ID = _CurrentTerritoryID,
                //InitialPlacementAllowed = 
            };
            OnIslandAdded(t);
        }

        private void OnIslandAdded(Territory t)
        {
            if (IslandAdded != null)
            {
                IslandAdded(t);
            }
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) CloseSave();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OnIslandAdded(null);
        }
    }
}
