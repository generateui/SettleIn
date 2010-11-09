using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows;

using SettleInCommon.Board;
using SettleInCommon.Gaming;
using SettleInCommon;
using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Pieces;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class SetPortCornerBehaviour : BoardVisualBehaviour
    {
        // Hex we are dealing with
        private SeaHex _SelectedHex;

        //porttype to instantiate correct new SetPortbehaviour
        private EPortType _PortType;
        
        public SetPortCornerBehaviour(SeaHex seaHex, EPortType portType)
        {
            _PortType = portType;
            _SelectedHex = seaHex;
        }
        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexVisual hexVisual = rayMeshResult.VisualHit as HexVisual;
            if (hexVisual != null)
            {
                SeaHex seaHex = hexVisual.Hex as SeaHex;
                if (seaHex != null)
                {
                    if (((SeaHex)seaHex).XmlPort != null)
                    {
                        // we confirmed the port here, make a small animation
                        hexVisual.AnimatePortSetted();
                        return BehaviourResult.Success;
                    }
                }
            }

            // we need a seahex to move on
            return BehaviourResult.Success;
        }


    }
}
