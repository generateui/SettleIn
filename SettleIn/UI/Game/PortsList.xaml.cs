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
using SettleInCommon.Gaming;
using SettleInCommon.Board;

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for PortsList.xaml
    /// </summary>
    public partial class PortsList : UserControl
    {
        XmlPortList _Ports;

        public PortsList()
        {
            InitializeComponent();

            DataContextChanged += new DependencyPropertyChangedEventHandler(PortsList_DataContextChanged);

            SetupPorts();
        }

        void PortsList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _Ports = ((GamePlayer)DataContext).Ports;
            ((GamePlayer)DataContext).Ports.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Ports_PropertyChanged);
            SetupPorts();
        }

        void Ports_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetupPorts();
        }

        private void Replace(int position, Image i)
        {
            spPorts.Children.RemoveAt(position);
            spPorts.Children.Insert(position, i);
        }

        private void SetupPorts()
        {
            if (spPorts.Children.Count > 0)
            {
                if (_Ports != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (_Ports.ThreeToOne > 0)
                        {
                            Replace(i, new Image() { Source = (ImageSource)Core.Instance.Icons["Icon31Port"] });
                        }
                        else
                        {
                            spPorts.Children[i].Opacity = 0.3;
                        }
                    }
                    if (_Ports.ThreeToOne > 0)
                    {
                    }
                    if (_Ports.Timber > 0) Replace(0, new Image() { Source = (ImageSource)Core.Instance.Icons["IconTimberPort"] });
                    if (_Ports.Wheat > 0) Replace(1, new Image() { Source = (ImageSource)Core.Instance.Icons["IconWheatPort"] });
                    if (_Ports.Ore > 0) Replace(2, new Image() { Source = (ImageSource)Core.Instance.Icons["IconOrePort"] });
                    if (_Ports.Clay > 0) Replace(3, new Image() { Source = (ImageSource)Core.Instance.Icons["IconClayPort"] });
                    if (_Ports.Sheep > 0) Replace(4, new Image() { Source = (ImageSource)Core.Instance.Icons["IconSheepPort"] });
                    this.Visibility = Visibility.Visible;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Replace(i, new Image() { Source = (ImageSource)Core.Instance.Icons["IconRandomPort"] });
                        spPorts.Children[i].Opacity = 0.3;
                    }
                }
            }

        }
    }
}
