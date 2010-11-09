using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using SettleInCommon;
using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using System.Windows.Controls.Primitives;
using SettleIn.Classes.EventArgs;
using SettleIn.Engine.ViewPort.Behaviour;
using SettleIn.Engine.Rules;

namespace SettleIn.Engine.ViewPort
{
    /// <summary>
    /// Represents a 3D board for map editing purposes. 
    /// Encapsulates UI interaction logic like changing hexes, setting ports
    /// </summary>
    public class MapEditorViewPort3D : SettleInViewPort3D
    {
        /// <summary>
        /// Rules which are broken after validation
        /// </summary>
        private ObservableCollection<IRule> _BrokenRules = new ObservableCollection<IRule>();
        
        /// <summary>
        /// Rules to be checked for when validating the board
        /// </summary>
        private ObservableCollection<IRule> _Rules = new ObservableCollection<IRule>();
        
        /// <summary>
        /// List of brokenrules, populated after the board gets validated.
        /// </summary>
        public ObservableCollection<IRule> BrokenRules
        {
            get { return _BrokenRules; }
        }
        
        public MapEditorViewPort3D()
            : base()
        {
            //Add some rules
            this._Rules.Add(new NoNameRule());
            this._Rules.Add(new SurroundingSeaRule());
            this._Rules.Add(new MinMaxPlayers());
            this._Rules.Add(new NoneHexRule());
            this._Rules.Add(new MinDiscoverChits());
            this._Rules.Add(new MinRandomChits());
            this._Rules.Add(new PortsOnLand());
        }


        void Board_HexChanged(HexChangedEventArgs e)
        {
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="b"></param>
        public MapEditorViewPort3D(BoardVisual b):base(b)
        {
        }
        /// <summary>
        /// Validate the board. Invokes each rule, and adds it to _BrokenRules
        /// if rules is broken.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            _BrokenRules.Clear();
            foreach (IRule rule in _Rules)
            {
                if (!rule.Invoke(_Board.Board)) _BrokenRules.Add(rule);
            }
            return _BrokenRules.Count == 0;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Point3D testpoint3D = new Point3D(e.GetPosition(this).X, e.GetPosition(this).Y, 0);
                Vector3D testdirection = new Vector3D(e.GetPosition(this).X, e.GetPosition(this).Y, 10);
                PointHitTestParameters pointparams = new PointHitTestParameters(e.GetPosition(this));
                RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);
                
                //test for a result in the Viewport3D
                VisualTreeHelper.HitTest(this, null, HTClickResult, pointparams);
            }
        }

        public override HitTestResultBehavior HTClickResult(System.Windows.Media.HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;
            RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;
            Visual3D hitvisual = rayResult.VisualHit;
            if (rayMeshResult != null)
            {
                _InteractionBehaviour.Clicked(rayMeshResult, _Board); 
            }
            return HitTestResultBehavior.Continue;
        }

        public void PlaySound()
        {
            MediaPlayer mp = new MediaPlayer();
            mp.Open(new Uri("file:///c:/windows/media/tada.wav"));
            //mp.Open(new Uri("\\Sounds\\ploop.wav", UriKind.Relative));
            mp.Play();
        }


    }
}
