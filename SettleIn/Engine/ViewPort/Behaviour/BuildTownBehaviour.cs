using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using SettleInCommon.Actions.TurnActions;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class BuildTownBehaviour : BoardVisualBehaviour
    {
        bool _IsStart = false;

        public BuildTownBehaviour(bool isStart)
        {
            _IsStart = isStart;
        }

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            BuildPointVisual buildPoint = rayMeshResult.VisualHit as BuildPointVisual;
            if (buildPoint != null)
            {
                //board.Game.PlayerOnTurn.Towns.Add(buildPoint.Location);
                board.SetNeutral();
                ((BuildTownAction)_OriginatingAction).Location = buildPoint.Location;
                return BehaviourResult.Success;
            }
            return BehaviourResult.NoSuccess;
        }

        public override void SetStartState(BoardVisual board)
        {
            board.PickPoint(_IsStart);
        }
        public override void RemoveStartState(BoardVisual board)
        {
            board.SetNeutral();
        }
        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
        }
    }
}
