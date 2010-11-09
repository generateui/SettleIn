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

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for ResourcePicker.xaml
    /// </summary>
    public partial class ResourcePicker : UserControl
    {
        public delegate void ResourcesChangedDelegate(EAddRemove addRemove, EResource resource, int amount);
        public event ResourcesChangedDelegate ResourcesChanged;
        private ResourceList _AvailableResources;

        public ResourceList AvailableResources
        {
            get { return _AvailableResources; }
            set 
            { 
                _AvailableResources = value;
                imgTimber.Opacity = _AvailableResources.Timber > 0 ? 1.0 : 0.3;
                imgWheat.Opacity =  _AvailableResources.Wheat  > 0 ? 1.0 : 0.3;
                imgOre.Opacity =    _AvailableResources.Ore    > 0 ? 1.0 : 0.3;
                imgClay.Opacity =   _AvailableResources.Clay   > 0 ? 1.0 : 0.3;
                imgSheep.Opacity =  _AvailableResources.Sheep  > 0 ? 1.0 : 0.3;
                lblTimber.Content = _AvailableResources.Timber;
                lblWheat.Content = _AvailableResources.Wheat;
                lblOre.Content = _AvailableResources.Ore;
                lblClay.Content = _AvailableResources.Clay;
                lblSheep.Content = _AvailableResources.Sheep;
            }
        }

        public enum EAddRemove { Add, Remove };
        private bool _ReadOnly=false;
        private void OnResourcesChanged(EAddRemove addRemove, EResource resource, int amount)
        {
            if (ResourcesChanged != null)
                ResourcesChanged(addRemove, resource, amount);
        }
        public ResourceList Resources1
        {
            get
            {
                return uiResources.Resources1;
            }
            set
            {
                uiResources.Resources1 = value;
            }
        }
        public bool ReadOnly
        {
            set 
            {
                uiResources.ReadOnly = value;
                _ReadOnly = value;
                spSelector.Visibility = value ? Visibility.Hidden : Visibility.Visible;
                lblPickResources.Visibility = value ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public ResourcePicker()
        {
            InitializeComponent();
        }

        private void imgTimber_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_ReadOnly)
            {
                Image image = sender as Image;
                if (image != null)
                {
                    EResource res = (EResource)image.Tag;
                    uiResources.AddResource((Image)sender);
                    OnResourcesChanged(EAddRemove.Remove, res, 1);
                }
            }
        }
    }
}
