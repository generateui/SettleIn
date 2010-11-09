using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Pieces;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class AssignTerritoryBehaviour : BoardVisualBehaviour
    {
        /// <summary>
        /// When user is changing territory, this holds the new territory
        /// </summary>
        private Territory _SelectedTerritory;

        public Territory SelectedTerritory
        {
            get { return _SelectedTerritory; }
        }

        public AssignTerritoryBehaviour(Territory territory)
        {
            _SelectedTerritory = territory;
        }

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            //Check if clicked hex is a LandHex, if so, put current territory
            //on the hex. 
            HexVisual hexVisual = rayMeshResult.VisualHit as HexVisual;
            if (hexVisual != null)
            {
                ITerritoryHex terrHex = hexVisual.Hex as ITerritoryHex;
                if (terrHex != null)
                {
                    terrHex.TerritoryID = _SelectedTerritory.ID;
                    return BehaviourResult.Success;
                }
            }
            return BehaviourResult.NoSuccess;
        }
        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexVisual hex = rayMeshResult.VisualHit as HexVisual;
            if (hex != null)
            {
                if (hex.Hex is ITerritoryHex)
                {
                    if (_OldMouseOverHex != null)
                        _OldMouseOverHex.Selected = false;
                    hex.Selected = true;

                    _OldMouseOverHex = hex;
                }
            }
        }
    }
}
