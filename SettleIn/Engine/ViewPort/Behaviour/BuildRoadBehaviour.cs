using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SettleInCommon.Board;
using SettleInCommon.Actions.TurnActions;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class BuildRoadBehaviour : BoardVisualBehaviour
    {
        private HexSideVisual _OldMouseOver;
        private HexSide _Location;
        private HexPoint _StartingTownOrCity;

        public HexPoint StartingTownOrCity
        {
            get { return _StartingTownOrCity; }
            set { _StartingTownOrCity = value; }
        }

        public HexSide Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexSideVisual hexSideVisual = rayMeshResult.VisualHit as HexSideVisual;
            if (hexSideVisual != null)
            {
                _Location = hexSideVisual.Location;
                //board.Game.PlayingPlayer.Roads.Add(_Location);
                board.SetNeutral();
                ((BuildRoadAction)_OriginatingAction).Intersection = hexSideVisual.Location;
                return BehaviourResult.Success;
            }
            return BehaviourResult.NoSuccess;
        }
        public override void RemoveStartState(BoardVisual board)
        {
            board.SetNeutral();
        }
        public override void SetStartState(BoardVisual board)
        {
            board.PickRoad(_StartingTownOrCity);
        }
        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexSideVisual hexSideVisual = rayMeshResult.VisualHit as HexSideVisual;
            if (_OldMouseOver != null)
            {
                _OldMouseOver.Scale.ScaleX = 1;
                _OldMouseOver.Scale.ScaleY = 1;
                _OldMouseOver.Scale.ScaleZ = 1;
                _OldMouseOver = null;
            } 
            if (hexSideVisual != null)
            {

                hexSideVisual.Scale.ScaleX = 2;
                hexSideVisual.Scale.ScaleY = 2;
                hexSideVisual.Scale.ScaleZ = 2;
                _OldMouseOver = hexSideVisual;
            }
        }
    }
}
