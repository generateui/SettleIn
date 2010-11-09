using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class ShowPointsBehaviour : BoardVisualBehaviour
    {
        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            Road side = rayMeshResult.VisualHit as Road;
            if (side != null)
            {
                List<BuildPointVisual> x = new List<BuildPointVisual>();
                foreach (BuildPointVisual bpv in board.Children.OfType<BuildPointVisual>())
                    x.Add(bpv);
                foreach (BuildPointVisual bp in x)
                    board.Children.Remove(bp);

                BuildPointVisual bpv1 = new BuildPointVisual(board.CalculatePosition(side.Location.HexPoint1), side.Location.HexPoint1);
                BuildPointVisual bpv2 = new BuildPointVisual(board.CalculatePosition(side.Location.HexPoint2), side.Location.HexPoint2);
                board.Children.Add(bpv1);
                board.Children.Add(bpv2);
            }
            return BehaviourResult.NoSuccess;
        }
    }
}
