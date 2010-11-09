using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace SettleIn.Engine.Pieces
{
    public interface IMoveable
    {
        TranslateTransform3D Move
        {
            get;
        }
    }
}
