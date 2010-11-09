using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

using SettleInCommon.Board;

namespace SettleInCommon.Board.Hexes
{
    /// <summary>
    /// Represents the base type for a hex that produces resources
    /// </summary>
    [DataContract]
    [KnownType(typeof(RawResourceHex))]
    [KnownType(typeof(SpecialResourceHex))]
    public class ResourceHex : LandHex
    {
        private Chit _XmlChit;
        protected EResource _Resource;

        [DataMember]
        public Chit XmlChit
        {
            get { return _XmlChit; }
            set
            {
                if (value != _XmlChit)
                {
                    _XmlChit = value;
                    OnPropertyChanged("Chit");
                }
            }
        }

        public virtual EResource Resource
        {
            get { throw new NotImplementedException(); }
        }

        public override string ToString()
        {
            string chit = string.Empty;
            if (_XmlChit != null)
            {
                chit = _XmlChit.ToString();
            }
            return Resource.ToString() + " " + chit;
        }

    }
}
