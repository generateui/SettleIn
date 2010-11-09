using System;
using System.Collections.Generic;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SettleIn.UI.Elements;
using System.Collections.ObjectModel;
using SettleIn.UI.Editor;

namespace SettleIn
{
	/// <summary>
	/// Interaction logic for TerritorySettings.xaml
	/// </summary>
	public partial class TerritorySettings : UserControl, INotifyPropertyChanged
	{
        public static readonly DependencyProperty BoardProperty =
            DependencyProperty.Register("Board", typeof(BoardVisual), typeof(TerritorySettings));

        public event TerritoryMenu.WantAddTerritoryHandler WantAddTerritory;
        public event TerritoryMenu.ViewToggledHandler ViewToggled;
        public event MapCreatorMain.CancelledHandler Cancelled;

        public BoardVisual Board
        {
            get { return (BoardVisual)GetValue(TerritorySettings.BoardProperty); }
            set { SetValue(TerritorySettings.BoardProperty, value); }
        }
        private Territory _CurrentTerritory;
        private PropertyChangedEventHandler _PropertyChanged;

        public Territory CurrentTerritory
        {
            get { return _CurrentTerritory; }
            set 
            { 
                _CurrentTerritory = value; 
                OnPropertyChanged("CurrentTerritory");
            }
        }

        private void OnPropertyChanged(string p)
        {
            if (_PropertyChanged !=null)
                _PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

		public TerritorySettings()
		{
			this.InitializeComponent();
		}

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxTerritories.SelectedIndex != -1)
            {
                CurrentTerritory = (Territory)lbxTerritories.SelectedItem;
                SetUI(true);
            }
            else
            {
                SetUI(false);
            }
        }

        private void SetUI(bool p)
        {
            uiPortList.IsEnabled = p;
            uiChitList.IsEnabled = p;
            uiHexList.IsEnabled = p;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }

        #endregion

        private void btnShowAttachedHexes_Click(object sender, RoutedEventArgs e)
        {
            if (ViewToggled != null) ViewToggled();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            BoardVisual bv = DataContext as BoardVisual;
            bv.Board.Territories.Remove(_CurrentTerritory);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (WantAddTerritory != null) WantAddTerritory();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Cancelled != null) Cancelled();
        }
    }
}