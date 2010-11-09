using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using SettleInCommon;

namespace SettleInCommon.Board
{
    [DataContract]
    public class ExtendedResourceList
    {
        private int _Timber = 0;
        private int _Wheat = 0;
        private int _Ore = 0;
        private int _Clay = 0;
        private int _Sheep = 0;
        private int _Gold = 0;
        private int _Discoveries = 0;

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

        public int Timber
        {
            get { return _Timber; }
            set { if (value != _Timber) { _Timber = value; OnPropertyChanged("Timber"); } }
        }
        public int Wheat
        {
            get { return _Wheat; }
            set { if (value != _Wheat) { _Wheat = value; OnPropertyChanged("Wheat"); } }
        }
        public int Ore
        {
            get { return _Ore; }
            set { if (value != _Ore) { _Ore = value; OnPropertyChanged("Ore"); } }
        }
        public int Clay
        {
            get { return _Clay; }
            set { if (value != _Clay) { _Clay = value; OnPropertyChanged("Clay"); } }
        }
        public int Sheep
        {
            get { return _Sheep; }
            set { if (value != Sheep) { _Sheep = value; OnPropertyChanged("Sheep"); } }
        }
        public int Discoveries
        {
            get { return _Discoveries; }
            set 
            { 
                if (value != _Discoveries) 
                { 
                    _Discoveries = value; 
                    OnPropertyChanged("Discoveries"); 
                } 
            }
        }
        public int Gold
        {
            get { return _Gold; }
            set { if (value != _Gold) { _Gold = value; OnPropertyChanged("Gold"); } }
        }
        /// <summary>
        /// Returns number of hexes in the list, including sea hexes
        /// </summary>
        public int CountAll
        { get { return _Timber + _Wheat + _Ore + _Clay + _Sheep + _Discoveries + _Gold; } }
    }
}
