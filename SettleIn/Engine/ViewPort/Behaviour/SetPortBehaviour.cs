using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
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

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleInCommon.Gaming;
using SettleInCommon;
using SettleIn.Engine.Pieces;

namespace SettleIn.Engine.ViewPort.Behaviour
{
    public class SetPortBehaviour : BoardVisualBehaviour
    {
        private EPortType _SelectedPortType;

        public EPortType SelectedPortType
        {
            get { return _SelectedPortType; }
        }

        public SetPortBehaviour(EPortType selectedPortType)
        {
            _SelectedPortType = selectedPortType;
        }

        public override HitTestFilterBehavior HittestFilter(DependencyObject o)
        {
            if (o.GetType() == typeof(PortVisual)) return HitTestFilterBehavior.Continue;
            return base.HittestFilter(o);
        }

        // when null, the editor doesnt have a hex to change its mouseover behaviour
        // Hex we are putting a port onto
        // when not-null, user is selecting the position of the port
        private SeaHex _SelectedHex;

        //porttype to instantiate correct new SetPortbehaviour
        private EPortType _PortType;

        public override BehaviourResult Clicked(RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexVisual hexVisual = rayMeshResult.VisualHit as HexVisual;
            if (hexVisual != null)
            {
                SeaHex seaHex = hexVisual.Hex as SeaHex;
                if (seaHex != null)
                {
                    if (_SelectedPortType == EPortType.None)
                    {
                        seaHex.XmlPort = null;
                        return BehaviourResult.Success;
                    }
                    else
                    {
                        if (seaHex.XmlPort == null)
                            seaHex.XmlPort = new Port()
                            {
                                Location = seaHex.Location,
                            };
                        seaHex.XmlPort.PortType = _SelectedPortType;
                        switch (rayMeshResult.VertexIndex3)
                        {
                            case 0: seaHex.XmlPort.PortPosition = ERotationPosition.Deg240; break;
                            case 1: seaHex.XmlPort.PortPosition = ERotationPosition.Deg180; break;
                            case 2: seaHex.XmlPort.PortPosition = ERotationPosition.Deg120; break;
                            case 3: seaHex.XmlPort.PortPosition = ERotationPosition.Deg60; break;
                            case 4: seaHex.XmlPort.PortPosition = ERotationPosition.Deg0; break;
                            case 5: seaHex.XmlPort.PortPosition = ERotationPosition.Deg300; break;
                        }
                        hexVisual.AnimatePortSetted();
                        if (_SelectedHex == null)
                            _SelectedHex = seaHex;
                        else
                            _SelectedHex = null;
                        return BehaviourResult.Success;
                    }
                }
            }
            return BehaviourResult.NoSuccess;
        }

        public override void Moved(
            RayMeshGeometry3DHitTestResult rayMeshResult, BoardVisual board)
        {
            HexVisual hexVisual = rayMeshResult.VisualHit as HexVisual;
            if (hexVisual != null)
            {
                // make sure we are applying mouseovers only to sea hexes
                SeaHex seaHex = hexVisual.Hex as SeaHex;
                if (seaHex != null)
                {
                    // user has clicked a hex, and wants to set a port on a certain direction
                    if (_SelectedHex != null)
                    {
                        if (seaHex == _SelectedHex)
                        {
                            seaHex.XmlPort.PortType = _SelectedPortType;

                            // map the vertex to the RotationPosition
                            switch (rayMeshResult.VertexIndex3)
                            {
                                case 0: seaHex.XmlPort.PortPosition = ERotationPosition.Deg240; break;
                                case 1: seaHex.XmlPort.PortPosition = ERotationPosition.Deg180; break;
                                case 2: seaHex.XmlPort.PortPosition = ERotationPosition.Deg120; break;
                                case 3: seaHex.XmlPort.PortPosition = ERotationPosition.Deg60; break;
                                case 4: seaHex.XmlPort.PortPosition = ERotationPosition.Deg0; break;
                                case 5: seaHex.XmlPort.PortPosition = ERotationPosition.Deg300; break;
                            }
                        }
                    }
                    // user simply hovers over a hex 
                    else
                    {
                        // user is removing ports, so only show mouseover when
                        // there is a port present
                        if (_SelectedPortType == EPortType.None &&
                            seaHex.XmlPort != null)
                        {
                            base.Moved(rayMeshResult, board);
                        }
                        // user is setting any other port type, so show mouseover
                        // on each seahex
                        if (_SelectedPortType != EPortType.None)
                        {
                            base.Moved(rayMeshResult, board);
                        }
                    }

                }
            }
        }
    }
}
