using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleIn.Engine.Pieces;
using System.Windows.Media.Media3D;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    /// <summary>
    /// UI test class 
    /// </summary>
    public class TestBehaviour: BoardVisualBehaviour
    {
        bool _IsStart = false;

        public TestBehaviour()
        {
        }

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            PortVisual port = rayMeshResult.VisualHit as PortVisual;
            if (port !=null)
            {
                board.ShowSideNeighbours(port.XmlPort.SideLocation);
                return BehaviourResult.NoSuccess;
            }

            HexVisual hex = rayMeshResult.VisualHit as HexVisual;
            if (hex != null)
            {
                List<HexPoint> temp = new List<HexPoint>();
                List<HexPoint> temp2 = hex.Hex.Location.GetNeighbourPoints();
                temp.Add(temp2[0]);
                temp.Add(temp2[1]);
                temp.Add(temp2[4]);
                board.ShowPoints(temp);
            }

            
            Road road = rayMeshResult.VisualHit as Road;
            if (road != null)
            {
                //board.Game.PlayerOnTurn.Towns.Add(buildPoint.Location);
                //board.ShowNeighbours(road.Location);
                board.ShowSideNeighbours(road.Location);
                return BehaviourResult.NoSuccess;
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
