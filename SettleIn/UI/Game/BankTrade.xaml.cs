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
using SettleIn.UI.Elements;
using SettleInCommon.Board;
using SettleInCommon.Gaming;
using System.ComponentModel;
using SettleInCommon.Actions.TurnActions;

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for BankTrade.xaml
    /// </summary>
    public partial class BankTrade : UserControl
    {
        public event GameMain2.DoneHereHandler DoneHere;

        //When in autotrade, the resources missing
        private ResourceList _WantedResources;
        private XmlPortList _Ports;

        public enum ETradeType
        {
            // Autotrade types
            Road,
            Ship,
            Town,
            City,
            Devcard,
            // No autotrade
            None
        }

        /// <summary>
        /// Wantd cards for a bank trade
        /// </summary>
        public ResourceList WantedCards
        {
            get { return uiWanted.Resources1; }
        }

        /// <summary>
        /// Cards player offered for a bank trade
        /// </summary>
        public ResourceList OfferedCards
        {
            get { return uiOffered.PickedResources; }
        }

        private void OnDoneHere(bool success)
        {
            if (DoneHere != null)
                DoneHere(success);
        }
        public BankTrade()
        {
            InitializeComponent();

            uiOffered.ResourcesChanged += new ResourcePicker.ResourcesChangedDelegate(uiOffered_ResourcesChanged);
            uiWanted.ResourcesChanged += new ResourcePicker.ResourcesChangedDelegate(uiWanted_ResourcesChanged);
        }

        void uiWanted_ResourcesChanged(ResourcePicker.EAddRemove addRemove, EResource resource, int amount)
        {
            CheckOffer();
        }

        public void UpdateUI(ResourceList wantedResources, ResourceList bankResources, ResourceList resources, XmlPortList ports, ETradeType tradeType)
        {
            _WantedResources = wantedResources;
            uiWanted.AvailableResources = bankResources;
            _Ports = ports;
            uiOffered.UpdateUI(resources, ports);
            if (tradeType == ETradeType.None)
            {
                pnlAutoTrade.Visibility = Visibility.Collapsed;
                btnCancel.Visibility = Visibility.Visible;
                uiWanted.ReadOnly = false;
            }
            else
            {
                pnlAutoTrade.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                lblItemType.Content = Enum.GetName(typeof(ETradeType), tradeType);
                uiWanted.Resources1 = _WantedResources;
                uiWanted.ReadOnly = true;
                switch (tradeType)
                {
                    case ETradeType.City: imgItemType.Source = (ImageSource)Core.Instance.Icons["Sea3D"]; break;
                    case ETradeType.Town: imgItemType.Source = (ImageSource)Core.Instance.Icons["Town3D"]; break;
                    case ETradeType.Road: imgItemType.Source = (ImageSource)Core.Instance.Icons["Road48"]; break;
                    case ETradeType.Ship: imgItemType.Source = (ImageSource)Core.Instance.Icons["Ship48"]; break;
                    case ETradeType.Devcard: imgItemType.Source = (ImageSource)Core.Instance.Icons["IconBuyDevcard48"]; break;
                }
            }
        }
        void uiOffered_ResourcesChanged(ResourcePicker.EAddRemove addRemove, EResource resource, int amount)
        {
            CheckOffer();
        }

        private void CheckOffer()
        {
            GamePlayer player = DataContext as GamePlayer;
            int gold = 0;
            if (uiOffered.PickedResources.Timber > 0)
                gold += uiOffered.PickedResources.Timber / _Ports[EResource.Timber];
            if (uiOffered.PickedResources.Wheat > 0)
                gold += uiOffered.PickedResources.Wheat / _Ports[EResource.Wheat];
            if (uiOffered.PickedResources.Ore > 0)
                gold += uiOffered.PickedResources.Ore / _Ports[EResource.Ore];
            if (uiOffered.PickedResources.Clay > 0)
                gold += uiOffered.PickedResources.Clay / _Ports[EResource.Clay];
            if (uiOffered.PickedResources.Sheep > 0)
                gold += uiOffered.PickedResources.Sheep / _Ports[EResource.Sheep];
            
            SetUI(gold == uiWanted.Resources1.CountAllExceptDiscovery);
        }

        private void SetUI(bool OK)
        {
            if (OK)
                btnOK.IsEnabled = true;
            else
                btnOK.IsEnabled = false;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (WantedCards.CountAll > 0 &&
                OfferedCards.CountAll > 0)
                OnDoneHere(true);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OnDoneHere(false);
        }

        public TurnAction Action { get; set; }
    }
}
