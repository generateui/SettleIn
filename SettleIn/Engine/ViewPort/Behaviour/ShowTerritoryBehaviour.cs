using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Pieces;
using System.Windows.Media.Media3D;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class ShowTerritoryBehaviour : BoardVisualBehaviour
    {
        private int _TerritoryID = 0;
        public ShowTerritoryBehaviour(int TerritoryID)
        {
            _TerritoryID = TerritoryID;
        }
        public override void SetStartState(BoardVisual board)
        {
            foreach (HexVisual hv in from h in board.Children.OfType<HexVisual>()
                                     select h)
            {
                ITerritoryHex terrHex = hv.Hex as ITerritoryHex;
                if (terrHex != null)
                    if (terrHex.TerritoryID == _TerritoryID)
                        hv.IsDarkened = false;
                    else
                        hv.IsDarkened = true;
                else
                    hv.IsDarkened= true;
            }
        }
        public override void RemoveStartState(BoardVisual board)
        {
            foreach (HexVisual hv in board.Children.OfType<HexVisual>())
                hv.Selected = false;
        }

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            return BehaviourResult.NoSuccess;
        }
        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
        }
    }
}
