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
using SettleIn.UI.Game;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for ucResourceList.xaml
    /// </summary>
    public partial class ucResourceList : UserControl
    {
        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty.Register("ShowText", typeof(bool),
            typeof(ucResourceList));

        public bool ShowText
        {
            get { return (bool)GetValue(ShowTextProperty); }
            set 
            {
                SetValue(ShowTextProperty, value);
                if (value)
                {
                    lblSummary.Visibility = Visibility.Visible;
                }
                else
                {
                    lblSummary.Visibility = Visibility.Collapsed;
                }
            }
        }


        public event ResourcePicker.ResourcesChangedDelegate ResourcesChanged;
        private void OnResouresChanged(SettleIn.UI.Game.ResourcePicker.EAddRemove addRemove, EResource resource, int amount)
        {
            if (ResourcesChanged != null)
                ResourcesChanged(addRemove, resource,amount);
        }
        ResourceList _Resources = new ResourceList();

        private XmlPortList _Ports;
        private  bool _ReadOnly;

        public XmlPortList Ports
        {
            get { return _Ports; }
            set { _Ports = value; }
        }
        public ResourceList Resources1
        {
            get { return _Resources; }
            set 
            { 
                _Resources = value;
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            spPickedResources.Children.Clear();
            foreach (EResource r in _Resources)
            {
                Image add = new Image()
                {
                    Source = ucResourceList.GetIcon(r),
                    Tag = r,
                    Margin = new Thickness(-10, 0, 0, 0)
                };
                add.MouseUp += new MouseButtonEventHandler(add_MouseUp);
                spPickedResources.Children.Add(add);
            }
            lblSummary.Content = _Resources.ToString();
        }

        public ucResourceList()
        {
            InitializeComponent();

        }
        void add_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_ReadOnly)
            {
                EResource resource = (EResource)((Image)sender).Tag;
                if (_Ports != null)
                {
                    int divider = _Ports[resource];
                    List<Image> toRemove = new List<Image>();
                    for (int i = 0; i < divider; i++)
                    {
                        foreach (Image image in spPickedResources.Children.OfType<Image>())
                        {
                            if ((EResource)image.Tag == resource)
                            {
                                if (!toRemove.Contains(image))
                                {
                                    toRemove.Add(image);
                                    break;
                                }
                            }
                        }

                        _Resources.RemoveResource((EResource)((Image)sender).Tag);
                    }
                    int count = 0;
                    foreach (Image img in toRemove)
                    {
                        count++;
                        spPickedResources.Children.Remove(img);
                    }
                    if (count > 0) OnResouresChanged(ResourcePicker.EAddRemove.Remove, resource, count);
                }
                else
                {
                    spPickedResources.Children.Remove((UIElement)sender);
                    _Resources.RemoveResource((EResource)((Image)sender).Tag);
                    OnResouresChanged(ResourcePicker.EAddRemove.Remove, resource, 1);
                }
            }
            lblSummary.Content = _Resources.ToString();
        }

        public void AddResource(Image image)
        {
            _Resources.AddResource((EResource)image.Tag);
            Image add = new Image()
            {
                Source = image.Source,
                Tag = (EResource)image.Tag,
                Margin = new Thickness(-10, 0, 0, 0)
            };
            add.MouseUp += new MouseButtonEventHandler(add_MouseUp);
            spPickedResources.Children.Add(add);
            lblSummary.Content = _Resources.ToString();
        }


        public bool ReadOnly { set { _ReadOnly = value; } }

        public static ImageSource GetIcon(EResource resource)
        {
            switch (resource)
            {
                case EResource.Timber: return (ImageSource)Core.Instance.Icons["IconTimber"];
                case EResource.Wheat: return (ImageSource)Core.Instance.Icons["IconWheat"];
                case EResource.Ore: return (ImageSource)Core.Instance.Icons["IconOre"];
                case EResource.Clay: return (ImageSource)Core.Instance.Icons["IconClay"];
                case EResource.Sheep: return (ImageSource)Core.Instance.Icons["IconSheep"];
                case EResource.Gold: return (ImageSource)Core.Instance.Icons["IconGold"];
            }
            return null;
        }
    }
}
