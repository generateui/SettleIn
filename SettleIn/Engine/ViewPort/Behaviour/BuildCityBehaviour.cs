using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using SettleInCommon.Board;
using SettleInCommon.Actions.TurnActions;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class BuildCityBehaviour : BoardVisualBehaviour
    {
        BuildPointVisual _OldBuildPoint;
        private HexPoint _Location;
        bool _IsStart = false;

        public HexPoint Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        public BuildCityBehaviour(bool isStart)
        {
            _IsStart = isStart;
        }

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            Road hsv = rayMeshResult.VisualHit as Road;
            if (hsv != null)
            {
                hsv.IsRoadSelected = true;
            }
            BuildPointVisual buildPoint = rayMeshResult.VisualHit as BuildPointVisual;
            if (buildPoint != null)
            {
                //board.Game.PlayerOnTurn.Cities.Add(buildPoint.Location);
                _Location = buildPoint.Location;
                board.SetNeutral();
                ((BuildCityAction)_OriginatingAction).Location = buildPoint.Location;
                return BehaviourResult.Success;
            }
            return BehaviourResult.NoSuccess;
        }

        public override void SetStartState(BoardVisual board)
        {
            board.PickCity(_IsStart);
            foreach (Road r in board.Children.OfType<Road>())
            {
                r.IsRoadSelected = true;
            }
        }
        public override void RemoveStartState(BoardVisual board)
        {
            board.SetNeutral();
        }
        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            BuildPointVisual buildPoint = rayMeshResult.VisualHit as BuildPointVisual;
            if (buildPoint != null)
            {
                buildPoint.Scale.ScaleX = 2;
                buildPoint.Scale.ScaleY = 2;
                buildPoint.Scale.ScaleZ = 2;
                _OldBuildPoint = buildPoint;
            }
            else
            {
                if (_OldBuildPoint != null)
                {
                    _OldBuildPoint.Scale.ScaleX = 1;
                    _OldBuildPoint.Scale.ScaleY = 1;
                    _OldBuildPoint.Scale.ScaleZ = 1;
                }
            }
        }
    }
}
