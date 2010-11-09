using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using SettleInCommon;
using SettleInCommon.Board;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the base type for each hex.
    /// <seealso cref="http://www.codeproject.com/KB/cs/hexagonal_part1.aspx"/>
    /// <seealso cref="http://gmc.yoyogames.com/index.php?showtopic=336183"/>
    /// </summary>
    [DataContract]
    [KnownType(typeof(LandHex))]
    [KnownType(typeof(NoneHex))]
    [KnownType(typeof(SeaHex))]
    [KnownType(typeof(RuleHex))]
    public class Hex : INotifyPropertyChanged
    {
        #region Fields

        private HexLocation _Location;
        private event PropertyChangedEventHandler _PropertyChanged;

        #endregion

        #region Properties

        [DataMember]
        public HexLocation Location
        {
            get { return _Location; }
            set
            {
                if (value != _Location)
                {
                    _Location = value;
                    OnPropertyChanged("Location");
                }
            }
        }

        #endregion

        #region Static size calculation

        private static double s = 10;
        private static double h;
        private static double r;
        private static double b;
        private static double a;

        /// <summary>
        /// The width of the hex measured from outer left to the middle
        /// </summary>
        public static double HalfWidth
        { get { return r; } }

        /// <summary>
        /// Total width of the hex
        /// </summary>
        public static double Width
        { get { return a; } }

        /// <summary>
        /// Total height of the hex
        /// </summary>
        public static double Height
        { get { return b; } }

        /// <summary>
        /// Height measured from top to the first line
        ///      __         _
        ///     /  \        _ } PartialHeight
        ///    |    |
        ///     \  /
        ///      --
        /// </summary>
        public static double PartialHeight
        { get { return s + h; } }

        /// <summary>
        /// Height measured from the top to the second line
        ///      __         _
        ///     /  \          } BottomHeight
        ///    |    |       _
        ///     \  /
        ///      --
        /// </summary>
        public static double BottomHeight
        { get { return h; } }

        /// <summary>
        /// Size of the hex, measured one line
        ///     |  | --> size
        ///      __         
        ///     /  \    _    
        ///    |    |   _ } --> size    
        ///     \  /
        ///      --
        /// </summary>
        public static int Size
        { get { return (int)s; } }

        /// <summary>
        /// Helper function for size calculation
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        static private double DegreesToRadians(double degrees)
        {
            return degrees * System.Math.PI / 180;
        }

        /// <summary>
        /// Calculates the absolute size in the 3D coordinate system
        /// </summary>
        static Hex()
        {
            h = Math.Sin(DegreesToRadians(30)) * s;
            r = Math.Cos(DegreesToRadians(30)) * s;
            b = s + 2 * h;
            a = 2 * r;
        }

        #endregion

        #region Methods

        public Hex Copy()
        {
            ICloneable thisClone = this as ICloneable;
            Hex clone = (Hex)thisClone.Clone();
            ITerritoryHex thisTerrHex = this as ITerritoryHex;
            if (thisTerrHex != null)
                ((ITerritoryHex)clone).TerritoryID = thisTerrHex.TerritoryID;
            return clone;
        }


        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }

        protected void OnPropertyChanged(string p)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        #endregion

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
