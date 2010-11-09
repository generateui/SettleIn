using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Board;
using System.Windows.Media.Media3D;
using SettleInCommon.Actions.TurnActions;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class MoveShipBehaviour : BoardVisualBehaviour
    {
        private HexSide _ShipToMove = null;
        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexSideVisual hexSide = rayMeshResult.VisualHit as HexSideVisual;
            if (hexSide != null)
            {
                if (_ShipToMove == null)
                {
                    // player has not yet selected a ship to move. Show moveable ships
                    //board.SetMoveShip();
                    _ShipToMove = hexSide.Location;
                    board.SetMoveToShip(_ShipToMove);
                    return BehaviourResult.NoSuccess;
                }
                else
                {
                    MoveShipAction moveShip = _OriginatingAction as MoveShipAction;
                    moveShip.OldLocation = _ShipToMove;
                    moveShip.NewLocation = hexSide.Location;
                    board.SetNeutral();
                    return BehaviourResult.Success;
                }
            }
            return BehaviourResult.NoSuccess;
        }
        public override void SetStartState(BoardVisual board)
        {
            board.SetMoveShip();
        }
    }
}
