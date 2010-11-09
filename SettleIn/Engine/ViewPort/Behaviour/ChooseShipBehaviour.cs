using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using SettleInCommon.Board;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    /// <summary>
    /// Behaviour for moving a ship to another location
    /// </summary>
    public class ChooseShipBehaviour : BoardVisualBehaviour
    {
        private HexSide _Location;

        public HexSide Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            Ship ship = rayMeshResult.VisualHit as Ship;
            if (ship != null)
            {
                _Location = ship.Location;
                return BehaviourResult.Success;
            }
            return BehaviourResult.NoSuccess;
        }

        public override void SetStartState(BoardVisual board)
        {
            //TODO: Illuminate ships in a method on BoardVisdual class
        }
    }
}
