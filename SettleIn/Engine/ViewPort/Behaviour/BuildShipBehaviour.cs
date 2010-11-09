using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SettleInCommon.Board;
using SettleInCommon.Actions.TurnActions;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class BuildShipBehaviour : BoardVisualBehaviour
    {
        private HexSide _Location;

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
                board.SetNeutral();
                ((BuildShipAction)_OriginatingAction).Intersection = hexSideVisual.Location;
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
            board.PickShip();
        }
        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
        }
    }
}
