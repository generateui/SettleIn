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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SettleIn.Engine.Boards;

namespace SettleIn
{
    /// <summary>
    /// base class for all items in the playing field
    /// </summary>
    public abstract class Piece : ModelVisual3D
    {
        protected Point2D _Point;

        public Piece(Point2D point)
        {
            _Point = point;
        }

        public ModelVisual3D Parent { get; set; }
    }
}
