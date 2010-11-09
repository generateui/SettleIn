using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SettleInCommon.Board
{
    [DataContract]
    public class Port : INotifyPropertyChanged
    {
        [NonSerialized]
        private PropertyChangedEventHandler _PropertyChanged;
        
        private EPortType _PortType;
        private ERotationPosition _PortPosition;
        private HexLocation _Location;
        private HexSide _SideLocation;

        public HexSide SideLocation
        {
            get
            {
                //if (_SideLocation == null)
                {
                    _SideLocation = _Location.GetSideLocation(_PortPosition);
                }

                return _SideLocation;
            }
        }

        [DataMember]
        public EPortType PortType 
        { 
            get { return _PortType; }
            set 
            {
                if (value != _PortType)
                {
                    _PortType = value; 
                    OnPropertyChanged("PortType");
                }
            }
        }

        [DataMember]
        public ERotationPosition PortPosition 
        { 
            get { return _PortPosition; }
            set 
            { 
                if (value != _PortPosition)
                {
                    _PortPosition = value; 
                    OnPropertyChanged("PortPosition");
                }
            }
        }

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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }
        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property to raise</param>
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
