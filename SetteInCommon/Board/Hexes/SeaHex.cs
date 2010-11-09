using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using SettleInCommon.Board;
using System.ComponentModel;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the sea hex. Sea hexes can have a port, and players can build 
    /// ships on it.
    /// </summary>
    [DataContract]
    public class SeaHex : Hex, ICloneable, INotifyPropertyChanged
    {
        private Port _XmlPort;

        [DataMember]
        public Port XmlPort
        {
            get { return _XmlPort; }
            set
            {
                if (value != _XmlPort)
                {
                    if (value != null && _XmlPort !=null)
                    {
                        _XmlPort.PropertyChanged -= _XmlPort_PropertyChanged;
                    }
                    
                    _XmlPort = value;
                    OnPropertyChanged("Port");
                    
                    if (_XmlPort != null)
                    {
                        _XmlPort.PropertyChanged += 
                            new PropertyChangedEventHandler(_XmlPort_PropertyChanged);
                    }
                }
            }
        }

        void _XmlPort_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PortPosition" || e.PropertyName == "PortType")
                // bubble notification through from the port
                OnPropertyChanged(e.PropertyName);
        }

        #region ICloneable Members

        public object Clone()
        {
            return new SeaHex();
        }

        #endregion
    }
}
