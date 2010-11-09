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
using SettleInCommon.Actions.InGame;
using SettleInCommon.Gaming;
using SettleInCommon.Board.Hexes;
using SettleIn.UI.Converters;
using SettleInCommon.Actions.TurnActions;

namespace SettleIn.UI.Game
{
    /// <summary>
    /// Interaction logic for ShowResourcesGained.xaml
    /// </summary>
    public partial class ShowResourcesGained : UserControl
    {
        public event GameMain2.DoneHereHandler DoneHere;

        public ShowResourcesGained()
        {
            InitializeComponent();

            this.DataContextChanged += 
                new DependencyPropertyChangedEventHandler(ShowResourcesGained_DataContextChanged);
        }

        private void ShowResourcesGained_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RollDiceAction rollDice = e.NewValue as RollDiceAction;
            Dictionary<GamePlayer, ResourceList> result = rollDice.PlayersResources;

            basePanel.Children.Clear();

            foreach (KeyValuePair<GamePlayer, ResourceList> kvp in result)
            {
                // add list of resources
                ItemsControl resources = new ItemsControl();
                
                FrameworkElementFactory stackPanelElement = new FrameworkElementFactory(typeof(StackPanel));
                stackPanelElement.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                resources.ItemsPanel = new ItemsPanelTemplate() { VisualTree = stackPanelElement};
                resources.Height = 90;

                Binding binding = new Binding()
                {
                    Converter = new ResourceConverter()
                };
                FrameworkElementFactory imageElement = new FrameworkElementFactory(typeof(Image));
                imageElement.SetBinding(Image.SourceProperty, binding);
                imageElement.SetValue(Image.HeightProperty, 80.0);
                imageElement.SetValue(Image.WidthProperty, 80.0);

                resources.ItemTemplate = new DataTemplate() 
                {
                    VisualTree = imageElement
                };
                basePanel.Children.Add(resources);
                resources.ItemsSource = kvp.Value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnDoneHere(true);
        }
        private void OnDoneHere(bool success)
        {
            if (DoneHere != null)
                DoneHere(success);
        }
    }
}
