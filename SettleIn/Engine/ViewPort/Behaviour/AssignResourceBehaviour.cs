using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;
using SettleIn.Engine.Pieces;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class ChangeHexBehaviour : BoardVisualBehaviour
    {
        private Hex _SelectedHexType;

        public Hex SelectedHexType
        {
            get { return _SelectedHexType; }
        }

        public ChangeHexBehaviour(Hex selectedHexType)
        {
            _SelectedHexType = selectedHexType;
        }

        #region IViewPortBehaviour Members

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexVisual hexVisual = rayMeshResult.VisualHit as HexVisual;
            if (hexVisual != null)
            {
                Hex newHex = _SelectedHexType.Copy();
                newHex.Location = hexVisual.Hex.Location;
                board.Board.Hexes[newHex.Location] = newHex;
                return BehaviourResult.Success;
            }
            return BehaviourResult.NoSuccess;
        }

        #endregion
    }
}
