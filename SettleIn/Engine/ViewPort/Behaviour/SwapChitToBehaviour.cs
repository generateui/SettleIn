using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleIn.Engine.Pieces;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using System.Windows.Media.Media3D;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class SwapChitToBehaviour : BoardVisualBehaviour
    {
        private HexLocation _Location;

        public HexLocation Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexVisual hexVisual = rayMeshResult.VisualHit as HexVisual;
            if (hexVisual != null)
            {
                ResourceHex resourceHex = hexVisual.Hex as ResourceHex;
                if (resourceHex != null)
                {
                    _Location = resourceHex.Location;
                    return BehaviourResult.Success;
                }
            }
            return BehaviourResult.NoSuccess;

        }
    }
}
