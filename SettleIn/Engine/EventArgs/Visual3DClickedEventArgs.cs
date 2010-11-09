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
using System.Windows.Media.Animation;

namespace SettleIn.Classes.EventArgs
{
    public class Visual3DClickedEventArgs
    {
        private Visual3D _Visual;

        public Visual3D Visual
        {
            get { return _Visual; }
            set { _Visual = value; }
        }
        protected MouseButtonState _LeftButton;
        protected MouseButtonState _RightButton;
        protected bool _HasMoved;
        protected Point _MousePosition;

        public Visual3DClickedEventArgs() { }
        public Visual3DClickedEventArgs(Visual3D visual, MouseButtonState left, MouseButtonState right, bool hasMoved, Point position)
        {
            _Visual = visual;
            _LeftButton = left;
            _RightButton = right;
            _HasMoved = hasMoved;
            _MousePosition = position;
        }

        public MouseButtonState LeftButton
        {
            get { return _LeftButton; }
            set { _LeftButton = value; }
        }
        public MouseButtonState RightButton
        {
            get { return _RightButton; }
            set { _RightButton = value; }
        }

        public bool HasMoved
        { get { return _HasMoved; } set { _HasMoved = value; } }

        public Point MousePosition
        { get { return _MousePosition; } set { _MousePosition = value; } }
    }
}
