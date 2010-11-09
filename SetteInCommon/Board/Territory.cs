using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace SettleInCommon.Board
{
    [DataContract]
    public class Territory : IEquatable<int>, IEquatable<Territory>, INotifyPropertyChanged
    {
        // 1-based index for usability reasons; its not really polite
        // to demand users to work with a zero based number index 
        // for territories
        protected int _ID = 1;

        private string _Name = string.Empty;

        private bool _InitialPlacementAllowed = true;
        private bool _SecondaryPlacementAllowed = true;
        private bool _IsMainland = false;
        private XmlChitList _ChitList = new XmlChitList();
        private XmlHexList _HexList = new XmlHexList();
        private XmlPortList _PortList = new XmlPortList();

        #region Properties

        [DataMember]
        public XmlChitList ChitList
        {
            get { return _ChitList; }
            set { _ChitList = value; }
        }

        [DataMember]
        public XmlHexList HexList
        {
            get { return _HexList; }
            set { _HexList = value; }
        }

        [DataMember]
        public XmlPortList PortList
        {
            get { return _PortList; }
            set { _PortList = value; }
        }

        /// <summary>
        /// 1-based ID for the territories
        /// </summary>
        [DataMember]
        public int ID
        {
            get { return _ID; }
            set 
            {
                
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        /// <summary>
        /// Returns true if the territory should be treated as mainland
        /// </summary>
        [DataMember]
        public bool IsMainland
        {
            get { return _IsMainland; }
            set
            {
                _IsMainland = value;
                OnPropertyChanged("IsMainland");
                OnPropertyChanged("IsIsland");
            }
        }
        
        /// <summary>
        /// Returns true if the territory should be treated as mainland
        /// </summary>
        [DataMember]
        public bool IsIsland
        {
            get { return !_IsMainland; }
            set
            {
                if (value == _IsMainland)
                {
                    _IsMainland = !value;
                    OnPropertyChanged("IsMainland");
                    OnPropertyChanged("IsIsland");
                }
            }
        }

        /// <summary>
        /// Name of the territory
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set 
            { 
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Can we place our initial settlement on this territory?
        /// </summary>
        [DataMember]
        public bool InitialPlacementAllowed
        {
            get { return _InitialPlacementAllowed; }
            set 
            { 
                _InitialPlacementAllowed = value;
                OnPropertyChanged("InitialPlacementAllowed");
            }
        }

        /// <summary>
        /// Can we place the second settlement on this territory?
        /// </summary>
        [DataMember]
        public bool SecondaryPlacementAllowed
        {
            get { return _SecondaryPlacementAllowed; }
            set 
            { 
                _SecondaryPlacementAllowed = value;
                OnPropertyChanged("SecondaryPlacementAllowed");
            }
        }

        #endregion

        /// <summary>
        /// Constructs a new list and presets data
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Territory> CreateList()
        {
            ObservableCollection<Territory> result = 
                new ObservableCollection<Territory>()
            {
                new Territory()
                {
                    ID = 1,
                    Name = "Main island",
                    IsMainland = true,
                    InitialPlacementAllowed = true,
                    SecondaryPlacementAllowed = true
                },
                new Territory()
                {
                    ID=2,
                    Name = "Gold island",
                    IsIsland=true,
                    InitialPlacementAllowed=false,
                    SecondaryPlacementAllowed=false
                }
            };
            
            return result;
        }

        public Territory Copy()
        {
            Territory result = new Territory();
            
            result.ID = _ID;
            result.Name = _Name;
            
            result.IsMainland = IsMainland;
            result.IsIsland = IsIsland;
            result.InitialPlacementAllowed = _InitialPlacementAllowed;
            result.SecondaryPlacementAllowed = _SecondaryPlacementAllowed;

            result.ChitList = _ChitList.Copy();
            result.HexList = HexList.Copy();
            result.PortList = _PortList.Copy();

            return result;
        }

        #region IEquatable<int> Members

        public bool Equals(int other)
        {
            return _ID == other;
        }

        #endregion

        #region IEquatable<Territory> Members

        public bool Equals(Territory other)
        {
            return other._ID == _ID;
        }

        #endregion

        #region INotifyPropertyChanged Members

        private PropertyChangedEventHandler _PropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
