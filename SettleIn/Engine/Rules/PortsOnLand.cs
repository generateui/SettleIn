using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;

namespace SettleIn.Engine.Rules
{
    public class PortsOnLand : IRule
    {
        int badHexCount = 0;

        #region IRule Members

        public bool Invoke(XmlBoard b)
        {
            badHexCount = 0;

            // Check each seahex with a port if the port is attached to 
            // a landhex
            foreach (SeaHex h in (from hh in b.Hexes.OfType<SeaHex>()
                                  where ((SeaHex)hh).XmlPort != null
                                  select hh))
            {
                Hex neighbour = null;

                // if we have an uneven row, we must add an offset
                int offset = h.Location.H % 2 == 0 ? 0 : -1;
                switch (h.XmlPort.PortPosition)
                {
                    case ERotationPosition.Deg0:
                        neighbour = b.Hexes[h.Location.W + offset, h.Location.H + 1];
                        break;
                    case ERotationPosition.Deg60:
                        neighbour = b.Hexes[h.Location.W + offset + 1, h.Location.H + 1];
                        break;
                    case ERotationPosition.Deg120:
                        neighbour = b.Hexes[h.Location.W + 1, h.Location.H];
                        break;
                    case ERotationPosition.Deg180:
                        neighbour = b.Hexes[h.Location.W + offset + 1, h.Location.H - 1];
                        break;
                    case ERotationPosition.Deg240:
                        neighbour = b.Hexes[h.Location.W + offset, h.Location.H + 1];
                        break;
                    case ERotationPosition.Deg300:
                        neighbour = b.Hexes[h.Location.W - 1, h.Location.H];
                        break;
                }
                // when neighbour is null here, it means we tried to get an hex outside 
                // the bounds of the array. In turn this means the port is aimed towards
                //  the edge, which is a faulted state.
                if (neighbour == null)
                {
                    badHexCount++;
                }
                else
                {
                    ITerritoryHex terrHex = neighbour as ITerritoryHex;
                    if (terrHex==null)
                    {
                        badHexCount++;
                    }
                }
            }
            if (badHexCount > 0)
                return false;
            else
                return true;
        }

        public string Problem
        {
            get
            {
                return String.Format("{0} ports are not connected to a landhex", badHexCount);
            }
        }

        public RuleSeverity Severity
        {
            get { return RuleSeverity.Error; }
        }

        #endregion
    }
}
