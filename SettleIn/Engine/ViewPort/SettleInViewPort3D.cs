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
using System.Windows.Media.Effects;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleIn.Classes.EventArgs;
using SettleIn.Engine.ViewPort.Behaviour;
using SettleIn.Engine.Pieces;
using System.ComponentModel;

namespace SettleIn.Engine.ViewPort
{
    /// <summary>
    /// Base class for viewing a board and providing interaction with a board.
    /// </summary>
    [Serializable]
    public partial class SettleInViewPort3D : Viewport3D, INotifyPropertyChanged
    {
        #region Private variables

        // Offset to determine click or move action
        const int _ClickSensitivity = 15;

        // 3D model holding all the submodels
        protected BoardVisual _Board;

        // Whether or not the mouse moved since the last mouse registration
        protected bool _MouseMoved;

        protected Point _CurrentMousePosition;
        protected MouseButtonState _LeftButton;
        protected MouseButtonState _RightButton;

        public delegate void Visual3DClickedHandler(Visual3DClickedEventArgs e);
        public event Visual3DClickedHandler Visual3DClicked;

        public delegate void BoardChangedHandler();

        // Fires when the board has ben changed
        public event BoardChangedHandler BoardChanged;

        private Point _PreviousPosition2D;
        private Vector3D _previousPosition3D = new Vector3D(0, 0, 1);

        // Group holding all transforms
        private Transform3DGroup _Transform = new Transform3DGroup();
        
        private ScaleTransform3D _Scale = new ScaleTransform3D();
        private TranslateTransform3D _Move = new TranslateTransform3D();
        private AxisAngleRotation3D _Rotation = new AxisAngleRotation3D();
        
        private ModelVisual3D light = new ModelVisual3D();

        private Point ff;

        protected BoardVisualBehaviour _InteractionBehaviour;

        #endregion

        #region Constructors
        
        public SettleInViewPort3D()
        {
            this.ClipToBounds = false;
            System.Windows.Media.Color c = System.Windows.Media.Color.FromArgb(255,255,255,255);
            Model3DGroup lights = new Model3DGroup();
            lights.Children.Add(new DirectionalLight(c, new Vector3D(-2, -3, -1)));
            light.Content = lights;
            _Transform.Children.Add(_Scale);
            _Transform.Children.Add(_Move);
            _Transform.Children.Add(new RotateTransform3D(_Rotation));
            _Move.OffsetX = 0;
            _Move.OffsetZ = 0;
            this.MouseWheel += this._eventSource_MouseWheel;
            this.Loaded += new RoutedEventHandler(S3DBoard_Loaded);
        }
        public SettleInViewPort3D(BoardVisual b):this()
        {
            this._Board = b;
            if (b != null) ReInitBoard();
        }
        #endregion

        #region Properties

        /// <summary>
        /// Current behaviour of clicks and mousemoves
        /// </summary>
        public BoardVisualBehaviour InteractionBehaviour
        {
            get { return _InteractionBehaviour; }
            set 
            {
                // Set old behaviour in neutral state
                if (_InteractionBehaviour != null)
                    _InteractionBehaviour.RemoveStartState(Board);
                
                //Change to new behaviour
                _InteractionBehaviour = value;

                //Set starting state of the new behaviour
                if (_InteractionBehaviour != null)
                    _InteractionBehaviour.SetStartState(Board);

                OnPropertyChanged("InteractionBehaviour");
            }
        }

        /// <summary>
        /// The board data structure
        /// </summary>
        public BoardVisual Board
        {
            get { return this._Board; }
            set 
            {
                this._Board = value;
                if (value != null)
                {
                    ReInitBoard();
                    OnBoardChanged();
                }
                else
                {
                    this.Children.Clear();
                    OnBoardChanged();
                }
            }
        }

        #endregion



        #region Methods

        /// <summary>
        /// Re initializes the board. Removes old Board, then adds new board to the 
        /// visual3D collection and finally adds a light to the scene.
        /// </summary>
        private void ReInitBoard()
        {
            //first, clear the board
            this.Children.Clear();

            //Add the board 3d model, which are the hexes+all other stuff
            this.Children.Add(_Board);

            //add a light to the scene
            this.Children.Add(light);
        }

        public void SetNeutral()
        {
            if (_InteractionBehaviour != null)
            {
                _InteractionBehaviour.RemoveStartState(_Board);
            }
        }

        void S3DBoard_Loaded(object sender, RoutedEventArgs e)
        {
            this.Camera.Transform = _Transform;
        }

        private void OnVisual3DClicked(Visual3DClickedEventArgs e)
        {
            if (this.Visual3DClicked != null)
            {
                this.Visual3DClicked(e);
            }
        }

        public void ChangeHex(int w, int h, Hex hex)
        {
            this._Board.Game.Board.Hexes[w, h] = hex;
        }

        public void ChangeHexChit(int w, int h, EChitNumber cn)
        {
            if (_Board.Game.Board.Hexes[w, h] is RawResourceHex) 
            {
                ((RawResourceHex)this._Board.Game.Board.Hexes[w, h]).XmlChit.ChitNumber = cn;
            }
        }

        private void Track(Point currentPosition)
        {
            Vector3D currentPosition3D = ProjectToTrackball(
                this.ActualWidth, this.ActualHeight, currentPosition);

            Vector3D axis = Vector3D.CrossProduct(_previousPosition3D, currentPosition3D);
            double angle = Vector3D.AngleBetween(_previousPosition3D, currentPosition3D);
            if (!(axis.X == 0 && axis.Y == 0 && axis.Z == 0))
            {
                Quaternion delta = new Quaternion(axis, -angle);

                // Get the current orientantion from the RotateTransform3D
                AxisAngleRotation3D r = _Rotation;
                Quaternion q = new Quaternion(_Rotation.Axis, _Rotation.Angle);

                // Compose the delta with the previous orientation
                q *= delta;

                // Write the new orientation back to the Rotation3D
                _Rotation.Axis = q.Axis;
                _Rotation.Angle = q.Angle;
                _previousPosition3D = currentPosition3D;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private Vector3D ProjectToTrackball(double width, double height, Point point)
        {
            // Scale so bounds map to [0,0] - [2,2]
            double x = point.X / (width / 2);    
            double y = point.Y / (height / 2);

            // Translate 0,0 to the center
            x = x - 1;
            
            // Flip so +Y is up instead of down
            y = 1 - y;

            double z2 = 1 - x * x - y * y;       // z^2 = 1 - x^2 - y^2
            double z = z2 > 0 ? Math.Sqrt(z2) : 0;

            return new Vector3D(x, y, z);
        }

        /// <summary>
        /// Zooms related to the drag position of the mouse in
        /// horizontal or vertical position
        /// </summary>
        /// <param name="currentPosition">Mouse position</param>
        private void Zoom(Point currentPosition)
        {
            double yDelta = currentPosition.Y - _PreviousPosition2D.Y;

            double scale = Math.Exp(yDelta / 100);    // 100 some value wet thumbs in wind 

            _Scale.ScaleX *= scale;
            _Scale.ScaleY *= scale;
            _Scale.ScaleZ *= scale;
        }

        /// <summary>
        /// Zooms a step in or out (10%)
        /// </summary>
        /// <param name="ZoomUp">True to zoom in, false to zoom out</param>
        private void Zoom(bool ZoomUp)
        {
            double factor = ZoomUp ? factor = .9 : factor = 1.1;
            _Scale.ScaleX *= factor;
            _Scale.ScaleY *= factor;
            _Scale.ScaleZ *= factor;
        }
        /// <summary>
        /// Moves the board according to the new position
        /// </summary>
        /// <param name="currentPosition">new position of the mouse</param>
        private void Move(Point currentPosition)
        {
            double xDelta = currentPosition.X - _PreviousPosition2D.X;
            double yDelta = currentPosition.Y - _PreviousPosition2D.Y;

            // After some trial by error, division by 5 seemed to deliver
            // quite ok results.
            // TODO: make movement accurate to the mouse pointer
            this._Move.OffsetX += yDelta / 5;
            this._Move.OffsetZ -= xDelta / 5;
        }

        /// <summary>
        /// Resets the state of the board transformations to the starting values
        /// </summary>
        public void Reset()
        {
            // Center the board
            this._Move.OffsetX = 0;
            this._Move.OffsetY = 0;
            this._Move.OffsetZ = 0;

            // Make the board faced as mathematical hexagon drawing
            this._Rotation.Angle = 0;
            
            // Reset zoom
            this._Scale.ScaleX = 1;
            this._Scale.ScaleY = 1;
            this._Scale.ScaleZ = 1;

            // TODO: clean up
            double x = this.Board.Game.Board.Width * Hex.Width;
            double y = this.Board.Game.Board.Height * Hex.Height;
            double d = x > y ? x : y;
            d += 10;
            Point3D p = ((PerspectiveCamera)this.Camera).Position;
            ((PerspectiveCamera)this.Camera).Position = new Point3D(p.X, d, p.Z);
        }

        /// <summary>
        /// Wrapper for the event to prevent NullReferenceExceptions
        /// </summary>
        private void OnBoardChanged()
        {
            if (BoardChanged != null)
            {
                BoardChanged();
            }
        }

        #endregion

        #region Event Handling

        /// <summary>
        /// Fired when the mousewheel rolls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _eventSource_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Zoom(true);
            }
            else
            {
                Zoom(false);
            }
        }

        /// <summary>
        /// Fired when the mouse button is in a 'down' position.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _LeftButton = e.LeftButton;
            _RightButton = e.RightButton;
            ff = e.GetPosition(this);
            Mouse.Capture(this, CaptureMode.Element);

            _PreviousPosition2D = e.GetPosition(this);
            _previousPosition3D = ProjectToTrackball(
                ActualWidth, ActualHeight, _PreviousPosition2D);

            Point mouseposition = e.GetPosition(this);
            PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);
            Console.WriteLine(this.GetType().GUID.ToString());
            Console.WriteLine(e.OriginalSource.ToString());
        }

        /// <summary>
        /// Fired when a mousebutton releases the clicked state
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Mouse.Capture(this, CaptureMode.None);

            if (_LeftButton == MouseButtonState.Pressed)
            {
                Point mouseposition = e.GetPosition(this);

                // make sure to register a click even if the user moved the mouse a bit
                if (mouseposition == ff ||
                    (ff.X + _ClickSensitivity > mouseposition.X &&
                    ff.X - _ClickSensitivity < mouseposition.X &&
                    ff.Y + _ClickSensitivity > mouseposition.Y &&
                    ff.Y - _ClickSensitivity < mouseposition.Y))
                {
                    _MouseMoved = false;
                    Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
                    Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);
                    PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);
                    RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);
                    //test for a result in the Viewport3D
                    _LeftButton = e.LeftButton;
                    _RightButton = e.RightButton;
                    _CurrentMousePosition = mouseposition;
                    VisualTreeHelper.HitTest(this, HittestFilter, HTClickResult, pointparams);
                }
            }
        }

        /// <summary>
        /// TODO: Refactor
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point currentPosition = e.GetPosition(this);

            // Prefer tracking to zooming if both buttons are pressed.
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Track(currentPosition);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                //Zoom(currentPosition);
                Move(currentPosition);
            }
            else
            {
                Point mouseposition = e.GetPosition(this);
                Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
                Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);
                PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);
                
                RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);
                VisualTreeHelper.HitTest(this, null, HTMoveResult, pointparams);
            }

            _PreviousPosition2D = currentPosition;
        }

        #endregion Event Handling

        #region HitTesting

        /// <summary>
        /// the hittest filter applied when performing hit testing on the viewport
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public HitTestFilterBehavior HittestFilter(DependencyObject o)
        {
            // If possible, use th interaction beaviour hittest filter
            if (_InteractionBehaviour != null)
            {
                return _InteractionBehaviour.HittestFilter(o);
            }

            // Default on continuing hittesting
            return HitTestFilterBehavior.Continue;
        }

        /// <summary>
        /// invoked when user clicked with left button on a viewport model
        /// </summary>
        /// <param name="rawresult"></param>
        /// <returns></returns>
        public virtual HitTestResultBehavior HTClickResult(HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;
            RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;
            Visual3D hitvisual = rayResult.VisualHit;
            if (rayMeshResult != null)
            {
                if (_InteractionBehaviour != null)
                {
                    _InteractionBehaviour.Clicked(rayMeshResult, Board);
                }
                // raise the visual3DClicked event
                OnVisual3DClicked(new Visual3DClickedEventArgs(hitvisual, _LeftButton, _RightButton, _MouseMoved, _CurrentMousePosition));
                return HitTestResultBehavior.Stop;
            }
            return HitTestResultBehavior.Continue;
        }

        /// <summary>
        /// Invoked when user hovers over hexes and first clicked a hex to change a port
        /// </summary>
        /// <param name="rawresult"></param>
        /// <returns></returns>
        public HitTestResultBehavior HTMoveResult(HitTestResult rawresult)
        {
            RayMeshGeometry3DHitTestResult rayMeshResult = rawresult as RayMeshGeometry3DHitTestResult;
            if (rayMeshResult != null)
            {
                if (_InteractionBehaviour != null)
                {
                    _InteractionBehaviour.Moved(rayMeshResult, Board);
                }
            }

            // Nothing hit, continue testing on hits
            return HitTestResultBehavior.Continue;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
