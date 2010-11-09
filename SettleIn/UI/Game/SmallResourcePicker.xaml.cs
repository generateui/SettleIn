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
using SettleIn.UI.Elements;

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for SmallResourcePicker.xaml
    /// </summary>
    public partial class SmallResourcePicker : UserControl
    {
        private ResourceList _PickedResources = new ResourceList();

        public ResourceList PickedResources
        {
          get { return _PickedResources; }
        }

        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent PlayClickedEvent = EventManager.RegisterRoutedEvent(
            "PlayClicked", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SmallResourcePicker));

        // Provide CLR accessors for the event
        public event RoutedEventHandler PlayClicked
        {
            add { AddHandler(PlayClickedEvent, value); }
            remove { RemoveHandler(PlayClickedEvent, value); }
        }

        // This method raises the Tap event
        void RaisePlayClickedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SmallResourcePicker.PlayClickedEvent);
            RaiseEvent(newEventArgs);
        }
        
        public SmallResourcePicker()
        {
            InitializeComponent();
        }

        private void imgTimber_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Only add resources when we have less then 2; YoP can only pick two gold
            if (_PickedResources.Count < 2)
            {
                AddResource((Image)sender);
            }
            if (_PickedResources.Count == 2)
            {
                btnYoP.IsEnabled = true;
            }
            else
            {
                btnYoP.IsEnabled = false;
            }
            e.Handled = true;
        }

        private void btnYoP_Click(object sender, RoutedEventArgs e)
        {
            RaisePlayClickedEvent();
        }

        void add_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EResource resource = (EResource)((Image)sender).Tag;
            spPickedResources.Children.Remove((UIElement)sender);
            _PickedResources.RemoveResource((EResource)((Image)sender).Tag);
            btnYoP.IsEnabled = false;
            e.Handled = true;
        }

        private void AddResource(Image image)
        {
            _PickedResources.AddResource((EResource)image.Tag);
            Image add = new Image()
            {
                Source = image.Source,
                Tag = (EResource)image.Tag,
                Margin = new Thickness(0, 0, 0, 0)
            };
            add.MouseUp += new MouseButtonEventHandler(add_MouseUp);
            spPickedResources.Children.Add(add);
        }
    }
}
