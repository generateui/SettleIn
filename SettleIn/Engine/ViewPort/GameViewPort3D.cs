using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Drawing;

using SettleInCommon.Actions;
using SettleIn.Classes.EventArgs;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Gaming;
using SettleIn.Engine.Pieces.ControlPieces;
using SettleInCommon.Actions.InGame;
using SettleIn.Engine.ViewPort.Behaviour;
using SettleIn.Engine.Pieces;
using System.Windows.Media;

namespace SettleIn.Engine.ViewPort
{
    /// <summary>
    /// represents a board 3D to play the game on. 
    /// </summary>
    public class GameViewPort3D : SettleInViewPort3D
    {
        private XmlGame _CurrentGame = new XmlGame();

        private ModelVisual3D _RoadModel = new ModelVisual3D();
        private InGameAction _CurrentAction;
        public delegate void ClickAction();
        public delegate void ActionCompletedHandler(InGameAction action);
        public event ActionCompletedHandler ActionCompleted;

        private void OnActionCompleted(InGameAction action)
        {
            if (ActionCompleted != null)
                ActionCompleted(action);
        }

        public XmlGame CurrentGame
        {
            get { return _CurrentGame; }
            set
            {
                if (_CurrentGame != value)
                {
                    _CurrentGame = value;
                    if (value != null)
                        Board = new BoardVisual(value);
                    OnPropertyChanged("CurrentGame");
                }
            }
        }

        public GameViewPort3D()
            : base()
        {
            this.Loaded += new System.Windows.RoutedEventHandler(S3DGameBoard_Loaded);
        }

        void S3DGameBoard_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 360;
            da.RepeatBehavior = RepeatBehavior.Forever;
            da.Duration = TimeSpan.FromSeconds(10);
            AxisAngleRotation3D r = new AxisAngleRotation3D();
            r.Axis = new Vector3D(0, 0, 0);
        }

        public bool BeginAction(InGameAction action)
        {
            if (_InteractionBehaviour != null)
            {
                _InteractionBehaviour.RemoveStartState(Board);
            }
            _InteractionBehaviour = BoardVisualBehaviour.CreateBehaviour(action, CurrentGame);
            if (_InteractionBehaviour != null)
            {
                _InteractionBehaviour.SetStartState(Board);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override HitTestResultBehavior HTClickResult(System.Windows.Media.HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;
            RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;
            if (rayMeshResult != null)
            {
                if (_InteractionBehaviour != null)
                {
                    if (_InteractionBehaviour.Clicked(rayMeshResult, Board) == BehaviourResult.Success)
                    {
                        OnActionCompleted(_InteractionBehaviour.OriginatingAction);
                        //_InteractionBehaviour.RemoveStartState(Board);
                    }
                }

                return HitTestResultBehavior.Continue;
            }
            return HitTestResultBehavior.Continue;
        }
    }
}
