using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ServiceModel;


using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SettleInCommon
{
    [KnownType(typeof(SeaHex))]
    [KnownType(typeof(WheatHex))]
    [KnownType(typeof(TimberHex))]
    [KnownType(typeof(ClayHex))]
    [KnownType(typeof(OreHex))]
    [KnownType(typeof(SheepHex))]
    [KnownType(typeof(RandomHex))]
    [KnownType(typeof(DiscoveryHex))]
    [KnownType(typeof(DesertHex))]
    [KnownType(typeof(JungleHex))]
    [KnownType(typeof(VolcanoHex))]
    [KnownType(typeof(GoldHex))]
    [KnownType(typeof(LandHex))]
    [KnownType(typeof(ResourceHex))]
    [KnownType(typeof(RawResourceHex))]
    [KnownType(typeof(SpecialResourceHex))]
    [KnownType(typeof(RuleHex))]
    [KnownType(typeof(NoneHex))]
    public class ListArrayOfT : List<Hex> 
    {
        private event HexChangedEventHandler _HexChanged;
        public delegate void HexChangedEventHandler(Hex oldHex, Hex newHex);
        
        private int _Width;
        private int _Height;
        
        [DataMember]
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        [DataMember]
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        public ListArrayOfT(int w, int h)
        {
            for (int i = w * h; i < w * h; i++)
            {
                this[i] = null;
            }
            this.Capacity = w * h;
            _Width = w;
            _Height = h;
        }
        
        public Hex this[int w, int h]
        {
            get
            {
                if (!CheckInput(w, h)) return null;
                return this[(_Width * h) + w];
            }
            set
            {
                CheckInput(w, h);
                if (Count - 1 < (_Width * h) + w)
                {
                    Insert((_Width * h) + w, value);
                }
                else
                {
                    Hex temp = this[(Width * h) + w];
                    this[(Width * h) + w] = value;
                    this[(Width * h) + w].Location = new HexLocation(w, h);
                    OnHexChanged(temp, value);
                }
            }
        }
        public Hex this[HexLocation location]
        {
            get
            {
                if (!CheckInput(location)) return null;
                return this[location.W, location.H];
            }
            set 
            {
                CheckInput(location);
                this[location.W, location.H] = value;
            }
        }
        
        public bool CheckInput(int w, int h)
        {
            return true;
            /*
            if (w < 0) return false;
            if (h < 0) return false;
            if (w >= Width) return false;
            if (h >= Height) return false;
             */
        }
        public bool CheckInput(HexLocation loc)
        {
            return CheckInput(loc.W, loc.H);
        }

        public ListArrayOfT() { }

        /// <summary>
        /// Returns total amount of random hexes on the board
        /// </summary>
        public int RandomHexCount
        { get { return (from Hex h in this where h is RandomHex select h).Count(); } }

        /// <summary>
        /// Returns total amount of hidden hexes on the board
        /// </summary>
        public int HiddenHexCount
        { get { return (from Hex h in this where h is DiscoveryHex select h).Count(); } }

        /// <summary>
        /// Returns 
        /// </summary>
        public int HiddenResourceHexCount
        { get { return (from Hex h in this where h is DiscoveryHex select h).Count(); } }

        /// <summary>
        /// returns total amount of hexes with a random port
        /// </summary>
        public int RandomPortCount
        {
            get
            {
                return (from Hex h in this
                        where
                            h is SeaHex &&
                            ((SeaHex)h).XmlPort != null &&
                            ((SeaHex)h).XmlPort.PortType == EPortType.Random
                        select h).Count();
            }
        }

        /// <summary>
        /// Returns total amount of hexes without a chit
        /// </summary>
        public int ChitlessHexCount
        {
            get
            {
                return (from Hex h in this
                        where
                            h is ResourceHex &&
                            ((ResourceHex)h).XmlChit.ChitNumber == EChitNumber.None
                        //h is ResourceHex && 
                        //((ResourceHex)h).Chit == null
                        select h)
                                .Count();
            }
        }

        /// <summary>
        /// Returns total number of hexes with a random chit number on it
        /// </summary>
        public int RandomChitsCount
        {
            get
            {
                return
                    (from Hex h in this where
                         h is ResourceHex &&
                         ((ResourceHex)h).XmlChit.ChitNumber == EChitNumber.Random
                     select h).Count();
            }
        }


        /// <summary>
        /// Returns total amount of hexes on this board
        /// </summary>
        public int TotalHexes
        {
            // or simply return Count;
            get { return Count; }
        }


        public event HexChangedEventHandler HexChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _HexChanged = (HexChangedEventHandler)Delegate.Combine(_HexChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _HexChanged = (HexChangedEventHandler)Delegate.Remove(_HexChanged, value); }
        }

        private void OnHexChanged(Hex oldHex, Hex newHex)
        {
            if (_HexChanged != null)
            {
                _HexChanged(oldHex,newHex);
            }
        }
    }
}
