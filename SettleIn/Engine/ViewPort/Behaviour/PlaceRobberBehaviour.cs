using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SettleInCommon.Actions.TurnActions;
using SettleInCommon.Gaming.DevCards;
using SettleIn.Engine.Pieces;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Board;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class PlaceRobberBehaviour : BoardVisualBehaviour
    {
        private bool _IsRobber = true;
        private XmlBoard _Board;
        private Hex _SelectedHexType;
        private List<HexVisual> _ForbiddenHexes = new List<HexVisual>();

        public bool IsRobber
        {
            get { return _IsRobber; }
            set { _IsRobber = value; }
        }

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexVisual hexVisual = rayMeshResult.VisualHit as HexVisual;
            if (hexVisual != null)
            {
                PlaceRobberPirateAction placeRobberPirate = _OriginatingAction as PlaceRobberPirateAction;
                if (placeRobberPirate != null)
                {
                    if (hexVisual.Hex is SeaHex)
                        _IsRobber = false;

                    placeRobberPirate.Location = hexVisual.Hex.Location;
                    RemoveStartState(board);
                    return BehaviourResult.Success;
                }
            }
            return BehaviourResult.NoSuccess;
        }

        public override void SetStartState(BoardVisual board)
        {
            if (_ForbiddenHexes.Count != 0)
                _ForbiddenHexes.Clear();

            foreach (HexVisual hv in board.Children.OfType<HexVisual>())
            {
                // TODO: check for edges, not allowe dto put robber or pirate there
                // Edges should be created at board design so we can iterate over them easily
                // Hex.IsEdge?

                if (hv.Hex.Location.W == 0 ||
                    hv.Hex.Location.H == 0 ||
                    hv.Hex.Location.W == board.Board.Width - 1 ||
                    hv.Hex.Location.H == board.Board.Height - 1)
                {
                    _ForbiddenHexes.Add(hv);
                    hv.IsDarkened = true;
                }
            }
        }

        public override void RemoveStartState(BoardVisual board)
        {
            foreach (HexVisual hv in board.Children.OfType<HexVisual>())
                hv.IsDarkened = false;
        }

        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexVisual hv = rayMeshResult.VisualHit as HexVisual;
            if (hv != null)
            {
                if (!_ForbiddenHexes.Contains(hv) ||
                    hv.Hex.Location.Equals(board.Game.Robber) ||
                    hv.Hex.Location.Equals(board.Game.Pirate))
                {
                    if (_OldMouseOverHex != null)
                        _OldMouseOverHex.Selected = false;
                    hv.Selected = true;

                    _OldMouseOverHex = hv;
                }
            }
        }

    }
}
