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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;

using SettleIn.Engine.ViewPort;
using SettleIn.Engine.ViewPort.Behaviour;
using SettleIn.UI.Elements;

namespace SettleIn.UI.Editor
{
    /// <summary>
    /// Interaction logic for MapCreator.xaml
    /// </summary>
    public partial class MapCreatorMain : UserControl
    {
        BoardRules _BoardRules = new BoardRules();
        private AddIsland _AddIsland = new AddIsland();
        private EPortType _SelectedPortType;
        private Territory _CurrentTerritory;

        /// <summary>
        /// When user is changing chit numbers, this holds the new chitnumber
        /// </summary>
        private EChitNumber _CurrentChitNumber;

        /// <summary>
        /// When user is changing hexes, this holds the new hex type
        /// </summary>
        private Hex _CurrentHexType;
        public delegate void CancelledHandler();


        public MapCreatorMain()
        {
            InitializeComponent();

            mapEditorViewPort.BoardChanged += new SettleInViewPort3D.BoardChangedHandler(mapEditorViewPort_BoardChanged);
            mapEditorViewPort.Board = new BoardVisual(new XmlBoard(8, 8));

            this.Loaded += new RoutedEventHandler(Window2_Loaded);
            this.uiAddTerritory.IslandAdded += new AddIsland.IslandAddedHandler(_AddIsland_IslandAdded);

            nbNewBoard.BoardPicked += new ucNewBoard.BoardPickedEvent(nbNewBoard_BoardPicked);
            
            tmTerritories.CurrentTerritoryChanged +=
                new TerritoryMenu.CurrentTerritoryChangedHandler(tmTerritories_CurrentTerritoryChanged);
            tmTerritories.ViewToggled += new TerritoryMenu.ViewToggledHandler(tmTerritories_ViewToggled);
            tmTerritories.WantAddTerritory += new TerritoryMenu.WantAddTerritoryHandler(tmTerritories_WantAddTerritory);

            uiTerritorySettings.Cancelled += new CancelledHandler(uiTerritorySettings_Cancelled);
            uiTerritorySettings.ViewToggled += new TerritoryMenu.ViewToggledHandler(tmTerritories_ViewToggled);
            uiTerritorySettings.WantAddTerritory += new TerritoryMenu.WantAddTerritoryHandler(tmTerritories_WantAddTerritory);
            spEditState.DataContext = mapEditorViewPort.InteractionBehaviour;
        }

        void uiTerritorySettings_Cancelled()
        {
            SetOverlay(uiTerritorySettings);
        }

        private void nbNewBoard_BoardPicked(XmlBoard board)
        {
            mapEditorViewPort.Board =  new BoardVisual(board);
            _CurrentTerritory = board.Territories[0];
            SetOverlay(nbNewBoard);
        }

        void tmTerritories_CurrentTerritoryChanged(Territory t)
        {
            _CurrentTerritory = t;
            mapEditorViewPort.InteractionBehaviour =
                new AssignTerritoryBehaviour(_CurrentTerritory);
        }

        void _AddIsland_IslandAdded(Territory t)
        {
            if (t != null)
                mapEditorViewPort.Board.Board.Territories.Add(t);
            
            SetOverlay(uiAddTerritory);
        }
        void tmTerritories_ViewToggled()
        {
            //mapEditorViewPort.Board.ShowTerritory = !mapEditorViewPort.Board.ShowTerritory;
            if (_CurrentTerritory != null)
            {
                mapEditorViewPort.InteractionBehaviour = new ShowTerritoryBehaviour(_CurrentTerritory.ID);
                mapEditorViewPort.InteractionBehaviour.SetStartState(mapEditorViewPort.Board);
            }
        }
        void SetOverlay(UIElement element)
        {
            if (gridOverlay.Visibility == Visibility.Collapsed)
            {
                gridOverlay.Visibility = Visibility.Visible;
                element.Visibility = Visibility.Visible;
            }
            else
            {
                gridOverlay.Visibility = Visibility.Collapsed;
                element.Visibility = Visibility.Collapsed;
            }
        }

        void mapEditorViewPort_BoardChanged()
        {
            if (mapEditorViewPort.Board != null)
            {
                bpProperties.DataContext = mapEditorViewPort.Board;
                dsDevs.DataContext = mapEditorViewPort.Board.Board.DevCards;
                uiTerritorySettings.DataContext = mapEditorViewPort.Board;
                tmTerritories.DataContext = mapEditorViewPort.Board.Board.Territories;
                brRules.DataContext = mapEditorViewPort.Board;

                if (mapEditorViewPort.Board.Board.Territories.Count > 0)
                {
                    _CurrentTerritory = mapEditorViewPort.Board.Board.Territories[0];
                }
            }
        }

        void tmTerritories_WantAddTerritory()
        {
            SetOverlay(uiAddTerritory);
            uiAddTerritory.TerritoryID = mapEditorViewPort.Board.Board.Territories.Count + 1;
        }

        void Window2_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mapEditorViewPort.BrokenRules;
        }

        private void ResourceButtonMouseDown(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "btnTimber": _CurrentHexType = new TimberHex(); break;
                case "btnWheat": _CurrentHexType = new WheatHex(); break;
                case "btnOre": _CurrentHexType = new OreHex(); break; 
                case "btnClay": _CurrentHexType = new ClayHex(); break;
                case "btnSheep": _CurrentHexType = new SheepHex(); break;
                case "btnGold": _CurrentHexType = new GoldHex(); break;
                case "btnJungle": _CurrentHexType = new JungleHex(); break;
                case "btnRandom": _CurrentHexType = new RandomHex(); break;
                case "btnSea": _CurrentHexType = new SeaHex(); break;
                case "btnVolcano": _CurrentHexType = new VolcanoHex(); break;
                case "btnDesert": _CurrentHexType = new DesertHex(); break;
                case "btnNone": _CurrentHexType = new NoneHex(); break;
                case "btnDiscover": _CurrentHexType = new DiscoveryHex(); break;
            }
            if (_CurrentHexType is ITerritoryHex)
                ((ITerritoryHex)_CurrentHexType).TerritoryID = _CurrentTerritory.ID;
            this.mapEditorViewPort.InteractionBehaviour =
                new ChangeHexBehaviour(_CurrentHexType);
        }

        private void PortButtonMouseDown(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (((Button)sender).Name)
            {
                case "btnTimberPort": _SelectedPortType = EPortType.Timber; break; 
                case "btnWheatPort": _SelectedPortType = EPortType.Wheat; break;
                case "btnOrePort": _SelectedPortType = EPortType.Ore; break;
                case "btnClayPort": _SelectedPortType = EPortType.Clay; break;
                case "btnSheepPort": _SelectedPortType = EPortType.Sheep; break;
                case "btnNoPort": _SelectedPortType = EPortType.None; break;
                case "btn31Port": _SelectedPortType = EPortType.ThreeToOne; break;
                case "btnRandomPort": _SelectedPortType = EPortType.Random; break;
            }
            this.mapEditorViewPort.InteractionBehaviour = new SetPortBehaviour(_SelectedPortType);
        }

        private void ChitButtonMouseDown(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "btnChit2": _CurrentChitNumber = EChitNumber.N2; break;
                case "btnChit3": _CurrentChitNumber = EChitNumber.N3; break;
                case "btnChit4": _CurrentChitNumber = EChitNumber.N4; break;
                case "btnChit5": _CurrentChitNumber = EChitNumber.N5; break;
                case "btnChit6": _CurrentChitNumber = EChitNumber.N6; break;
                case "btnChit8": _CurrentChitNumber = EChitNumber.N8; break;
                case "btnChit9": _CurrentChitNumber = EChitNumber.N9; break;
                case "btnChit10": _CurrentChitNumber = EChitNumber.N10; break;
                case "btnChit11": _CurrentChitNumber = EChitNumber.N11; break;
                case "btnChit12": _CurrentChitNumber = EChitNumber.N12; break;
                case "btnChitRandom": _CurrentChitNumber = EChitNumber.Random; break;
                case "btnChitNone": _CurrentChitNumber = EChitNumber.None; break;
            }
            this.mapEditorViewPort.InteractionBehaviour =
                new ChooseChitBehaviour(_CurrentChitNumber);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            mapEditorViewPort.Validate();
            XmlBoard xmlBoard = mapEditorViewPort.Board.Board;
            if (xmlBoard.FileExists())
            {
                if (MessageBox.Show("File exists. Overwrite?", "Overwrite file?", MessageBoxButton.YesNo)
                    == MessageBoxResult.Yes)
                    xmlBoard.Save();
            }
            else
            {
                xmlBoard.Save();
            }
        }

        private void btnAddIsland_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnShowTerritories_Click(object sender, RoutedEventArgs e)
        {
            mapEditorViewPort.Board.ShowTerritory = !mapEditorViewPort.Board.ShowTerritory;
        }

        private void btnShowBoardValidation_Click_1(object sender, RoutedEventArgs e)
        {
            CheckRules();
        }

        private void CheckRules()
        {
            mapEditorViewPort.Validate();
            lbxRules.DataContext = mapEditorViewPort.BrokenRules;
            ToggleDock(dpValidation);
        }

        private void btnShowBoardRules_Click(object sender, RoutedEventArgs e)
        { ToggleDock(dpRules); }

        private void btnShowDevStack_Click(object sender, RoutedEventArgs e)
        { ToggleDock(dpDevStack); }

        private void btnShowProperties_Click(object sender, RoutedEventArgs e)
        { ToggleDock(dpProperties); }

        private void btnNewBoard_Click(object sender, RoutedEventArgs e)
        { SetOverlay(nbNewBoard); }

        private void ToggleDock(DockPanel dp)
        {
            if (dp != dpDevStack) dpDevStack.Visibility = Visibility.Collapsed;
            if (dp != dpProperties) dpProperties.Visibility = Visibility.Collapsed;
            //if (dp != dpRandomHexes) dpRandomHexes.Visibility = Visibility.Collapsed;
            if (dp != dpRules) dpRules.Visibility = Visibility.Collapsed;
            if (dp != dpValidation) dpValidation.Visibility = Visibility.Collapsed;
            //if (dp != dpChitBag) dpChitBag.Visibility = Visibility.Collapsed;
            //if (dp != dpPorts) dpPorts.Visibility = Visibility.Collapsed;
            if (dp.Visibility == Visibility.Collapsed)
                dp.Visibility = Visibility.Visible;
            else
                dp.Visibility = Visibility.Collapsed;
        }

        private void btnSave_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnShowInitialRandom_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnShowHidden_Click(object sender, RoutedEventArgs e)
        {
            SetOverlay(uiTerritorySettings);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckRules();
        }

    }
}
