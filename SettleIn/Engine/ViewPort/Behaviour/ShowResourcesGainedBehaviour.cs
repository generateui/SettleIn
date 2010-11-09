using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Pieces;
using SettleInCommon.Board;
using System.Windows.Media.Media3D;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class ShowResourcesGainedBehaviour : BoardVisualBehaviour
    {
        public override BehaviourResult Clicked(System.Windows.Media.Media3D.RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            return base.Clicked(rayMeshResult, board);
        }

        public override void SetStartState(BoardVisual board)
        {
            RollDiceAction rollDice = OriginatingAction as RollDiceAction;
            if (rollDice != null)
            {
                // darken each hex which didnt produce resource(s)
                if (rollDice.PlayersResources.Count > 0)
                {
                    List<HexLocation> rolledHexes = new List<HexLocation>();
                    foreach (ResourceHex hex in rollDice.HexesAffected)
                        rolledHexes.Add(hex.Location);

                    foreach (HexVisual hv in board.Children.OfType<HexVisual>())
                    {
                        if (rolledHexes.Contains(hv.Hex.Location))
                        {
                            hv.IsDarkened = false;
                        }
                        else
                        {
                            hv.IsDarkened = true;
                        }
                    }
                }
            }
        }

        public override void RemoveStartState(BoardVisual board)
        {
            foreach (HexVisual hv in board.Children.OfType<HexVisual>())
            {
                hv.IsDarkened = false;
            }
        }

        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
        }

    }
}
