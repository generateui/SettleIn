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
    public partial class ExtendedResourcePicker : UserControl
    {
        public event ResourcePicker.ResourcesChangedDelegate ResourcesChanged;
        
        private XmlPortList _Ports;
        private ResourceList _ItemResources;
        private ResourceList _AvailableResources;

        private void OnResourcesChanged(ResourcePicker.EAddRemove addRemove, EResource resource, int amount)
        {
            if (ResourcesChanged != null)
                ResourcesChanged(addRemove, resource, amount);
        }

        public ResourceList PickedResources
        {
            get { return uiResources.Resources1; }
        }

        public void UpdateUI(ResourceList availableResources, XmlPortList ports)
        {
            _Ports = ports;
            uiResources.Ports = ports;
            _AvailableResources = availableResources;
            uiResources.Resources1 = new ResourceList();
            FillResources();
        }

        private void FillResources()
        {
            foreach (StackPanel panel in spPanels.Children.OfType<StackPanel>())
            {
                RefreshPanel(panel);
            }
        }

        private void RefreshPanel(StackPanel panel)
        {
            // remove old stuff if present
            foreach (Image i in panel.Children.OfType<Image>())
                i.MouseUp -= this.i_MouseUp;
            panel.Children.Clear();

            EResource resource = (EResource)panel.Tag;
            int count = 1;
            foreach (EResource timber in _AvailableResources.Where(r => r == resource))
            {
                int divider = _Ports[resource];

                //determine if resource is tradeable
                int amountPossibleTradeCards = _AvailableResources[resource, true] -
                    (_AvailableResources[resource, true] % divider);

                bool isEnabled = false;

                // image is enabled when it is tradeable
                if (count <= amountPossibleTradeCards) isEnabled = true;

                Image i = new Image()
                {
                    Source = (ImageSource)Core.Instance.Icons["Icon" + Enum.GetName(typeof(EResource), resource)],
                    Tag = resource,
                    Opacity = isEnabled ? 1.0 : 0.3,
                    Margin = new Thickness(0, -10, 0, 0)

                };
                i.MouseUp += new MouseButtonEventHandler(i_MouseUp);
                panel.Children.Add(i);
                count++;
            }
        }

        void i_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                EResource res = (EResource)image.Tag;
                if (_Ports != null)
                {
                    int divider = _Ports[res];
                    if (_AvailableResources[res, true] >= divider)
                    {
                        for (int i = 0; i < divider; i++)
                        {
                            uiResources.AddResource(image);
                            _AvailableResources.Remove(res);
                        }
                        RemoveResourcesFromPanel(divider, res);
                        OnResourcesChanged(ResourcePicker.EAddRemove.Add, res, divider);
                    }
                }
                else
                {
                    if (_AvailableResources[res, true] > 0)
                    {
                        uiResources.AddResource((Image)sender);
                        _AvailableResources.Remove(res);
                        RemoveResourcesFromPanel(1, res);
                        OnResourcesChanged(ResourcePicker.EAddRemove.Remove, res, 1);
                    }
                }
            }

        }

        private void RemoveResourcesFromPanel(int amount, EResource resource)
        {
            StackPanel panel = spPanels.Children.OfType<StackPanel>().Where(p => (EResource)p.Tag == resource).First();

            for (int i = 0; i < amount; i++)
            {
                panel.Children.RemoveAt(0);
            }
        }

        public ExtendedResourcePicker()
        {
            InitializeComponent();

            uiResources.ResourcesChanged += new ResourcePicker.ResourcesChangedDelegate(uiResources_ResourcesChanged);
        }

        void uiResources_ResourcesChanged(ResourcePicker.EAddRemove addRemove, EResource resource, int amount)
        {
            StackPanel panel = spPanels.Children.OfType<StackPanel>().Where(p => (EResource)p.Tag == resource).First();

            _AvailableResources.AddResources(resource, amount);
            if (addRemove == ResourcePicker.EAddRemove.Remove)
            {
                RefreshPanel(panel);
            }
        }

    }
}
