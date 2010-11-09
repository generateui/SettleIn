using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Runtime.Serialization;
using SettleInCommon.Board.Hexes;

namespace SettleInCommon.Board
{
    /// <summary>
    /// Represents a stack of hexes, in where the amount of each hex type is kept.
    /// </summary>
    [DataContract]
    public class XmlHexList : INotifyPropertyChanged
    {
        private int _Timber = 0;
        private int _Wheat = 0;
        private int _Ore = 0;
        private int _Clay = 0;
        private int _Sheep = 0;
        private int _Volcano = 0;
        private int _Jungle = 0;
        private int _Desert = 0;
        private int _Sea = 0;
        private int _Gold = 0;

        private event PropertyChangedEventHandler _PropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }

        private void OnPropertyChanged(string p)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(p));
                _PropertyChanged(this, new PropertyChangedEventArgs("CountAll"));
                if (p != "Sea") _PropertyChanged(this, new PropertyChangedEventArgs("CountAllExceptSea"));
                Console.WriteLine(p);
            }
        }

        [DataMember]
        public int Timber
        {
            get { return _Timber; }
            set { if (value != _Timber) { _Timber = value; OnPropertyChanged("Timber"); } }
        }

        [DataMember]
        public int Wheat
        {
            get { return _Wheat; }
            set { if (value != _Wheat) { _Wheat = value; OnPropertyChanged("Wheat"); } }
        }

        [DataMember]
        public int Ore
        {
            get { return _Ore; }
            set { if (value != _Ore) { _Ore = value; OnPropertyChanged("Ore"); } }
        }

        [DataMember]
        public int Clay
        {
            get { return _Clay; }
            set { if (value != _Clay) { _Clay = value; OnPropertyChanged("Clay"); } }
        }

        [DataMember]
        public int Sheep
        {
            get { return _Sheep; }
            set { if (value != Sheep) { _Sheep = value; OnPropertyChanged("Sheep"); } }
        }
        [DataMember]
        public int Volcano
        {
            get { return _Volcano; }
            set { if (value != _Volcano) { _Volcano = value; OnPropertyChanged("Volcano"); } }
        }

        [DataMember]
        public int Jungle
        {
            get { return _Jungle; }
            set { if (value != _Jungle) { _Jungle = value; OnPropertyChanged("Jungle"); } }
        }

        [DataMember]
        public int Desert
        {
            get { return _Desert; }
            set { if (value != _Desert) { _Desert = value; OnPropertyChanged("Desert"); } }
        }

        [DataMember]
        public int Sea
        {
            get { return _Sea; }
            set { if (value != _Sea) { _Sea = value; OnPropertyChanged("Sea"); } }
        }

        [DataMember]
        public int Gold
        {
            get { return _Gold; }
            set { if (value != _Gold) { _Gold = value; OnPropertyChanged("Gold"); } }
        }
        /// <summary>
        /// Returns number of hexes in the list, including sea hexes
        /// </summary>
        public int CountAll
        {
            get
            {
                return _Timber + _Wheat + _Ore + _Clay + _Sheep +
                    _Volcano + _Jungle + _Desert + _Sea + _Gold;
            }
        }
        /// <summary>
        /// Returns number of (non-sea) -hexes
        /// </summary>
        public int CountAllExceptSea
        {
            get
            {
                return _Timber + _Wheat + _Ore + _Clay + _Sheep +
                    _Volcano + _Jungle + _Desert + _Gold;
            }
        }

        public XmlHexList Copy()
        {
            XmlHexList result = new XmlHexList();
            
            result.Clay = this.Clay;
            result.Desert = this.Desert;
            result.Gold = this.Gold;
            result.Jungle = this.Jungle;
            result.Ore = this.Ore;
            result.Sea = this.Sea;
            result.Sheep = this.Sheep;
            result.Timber = this.Timber;
            result.Volcano = this.Volcano;
            result.Wheat = this.Wheat;

            return result;
        }
        public Hex PickHexFromList(int number)
        {
            int running = 0;

            //Clay
            if (this.Clay <= number)
            {
                running += this.Clay;
            }
            else
            {
                this.Clay--;
                return new ClayHex();
            }

            //Desert
            if (this.Desert + running <= number)
            {
                running += this.Desert;
            }
            else
            {
                this.Desert--;
                return new DesertHex();
            }

            if (this.Gold + running <= number)
            {
                running += this.Gold;
            }
            else
            {
                this.Gold--;
                return new GoldHex();
            }
            if (this.Jungle + running <= number)
            {
                running += this.Jungle;
            }
            else
            {
                this.Jungle--;
                return new JungleHex();
            }
            if (this.Ore + running <= number)
            {
                running += this.Ore;
            }
            else
            {
                return new OreHex();
            }
            if (this.Sea + running <= number)
            {
                running += this.Sea;
            }
            else
            {
                this.Sea--;
                return new SeaHex();

            }
            if (this.Sheep + running <= number)
            {
                running += this.Sheep;
            }
            else
            {
                this.Sheep--;
                return new SheepHex();

            }
            if (this.Timber + running <= number)
            {
                running += this.Timber;
            }
            else
            {
                this.Timber--;
                return new TimberHex();

            }
            if (this.Volcano + running <= number)
            {
                running += this.Volcano;
            }
            else
            {
                this.Volcano--;
                return new VolcanoHex();

            }
            this.Wheat--;
            return new WheatHex();

        }
    }
}
