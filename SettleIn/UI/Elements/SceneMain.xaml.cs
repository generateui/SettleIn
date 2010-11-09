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


using System.Windows.Media.Media3D;
using SettleIn.Engine.Boards;
using SettleInCommon.Board;

namespace SettleIn.UI.Elements
{
    /// <summary>
    /// Interaction logic for SceneMain.xaml
    /// </summary>
    public partial class SceneMain : UserControl
    {
        public SceneMain()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SceneMain_Loaded);

        }

        void SceneMain_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Color c = System.Windows.Media.Color.FromArgb(127, 255, 255, 255);
            mainViewport.Board = null;
            mainViewport.Children.Add(new Ship(new Point2D(0,0), Color.FromRgb(255,0,0), new HexSide(new SettleInCommon.Board.HexLocation(1,1), new SettleInCommon.Board.HexLocation(2,2))));
            //add a light to the scene
            Model3DGroup lights = new Model3DGroup();
            ModelVisual3D light;
            lights.Children.Add(new DirectionalLight(c, new Vector3D(-2, -3, -1)));
            light = new ModelVisual3D();
            light.Content = lights;

            mainViewport.Children.Add(light);
        }

    }
}
