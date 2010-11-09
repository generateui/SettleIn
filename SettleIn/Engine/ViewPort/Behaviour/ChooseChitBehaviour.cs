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
    public class ChooseChitBehaviour : BoardVisualBehaviour
    {
        private EChitNumber _Number;

        public EChitNumber Number
        {
            get { return _Number; }
        }
        public ChooseChitBehaviour(EChitNumber number)
        {
            _Number = number;
        }
        #region IViewPortBehaviour Members

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            //find an appropriate visual user clicked, 
            // can be hex or simply the chitnumber
            ResourceHex resourceHex = null;
            HexVisual hexVisual = rayMeshResult.VisualHit as HexVisual;
            if (hexVisual != null)
            {
                resourceHex = hexVisual.Hex as ResourceHex;
            }
            ChitVisual chitVisual = rayMeshResult.VisualHit as ChitVisual;
            if (chitVisual != null)
            {
                resourceHex = (ResourceHex)((HexVisual)chitVisual.Parent).Hex;
            }

            //when we found a suitable visual, update the chit number.
            if (resourceHex != null)
            {
                resourceHex.XmlChit = new Chit() { ChitNumber = _Number };
                return BehaviourResult.Success;
            }
            else
            {
                return BehaviourResult.NoSuccess;
            }
        }

        public override void Moved(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            if (rayMeshResult != null)
            {
                GeometryModel3D hitgeo = rayMeshResult.ModelHit as GeometryModel3D;
                HexVisual hex = rayMeshResult.VisualHit as HexVisual;
                if (hex != null)
                {
                    // only show mouseover when we have a resourcehex
                    if (hex.Hex is ResourceHex)
                    {
                        // when we are removing chits, show mouseover when there is
                        // something to remove
                        if (_Number == EChitNumber.None)
                        {
                            ResourceHex resourceHex = hex.Hex as ResourceHex;
                            if (resourceHex.XmlChit.ChitNumber != EChitNumber.None)
                            {
                                SetSelectionState(hex);
                            }
                        }
                        else
                        {
                            //default on showing a mouseover
                            SetSelectionState(hex);
                        }
                    }
                }
            }
        }

        private void SetSelectionState(HexVisual hex)
        {
            if (_OldMouseOverHex != null)
                _OldMouseOverHex.Selected = false;
            hex.Selected = true;

            _OldMouseOverHex = hex;

        }

        #endregion
    }
}
