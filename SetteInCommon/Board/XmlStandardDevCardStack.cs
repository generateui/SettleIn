using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.ServiceModel;

namespace SettleInCommon.Board
{
    /// <summary>
    /// represents the stack of devcards for the seafarers and original game
    /// </summary>
    [DataContract]
    public class StandardDevCardStack : INotifyPropertyChanged //, ISerializable
    {
        private int _VpCount;
        private int _RbCount;
        private int _RobberCount;
        private int _MonoCount;
        private int _YopCount;
        private bool _IsStandard;
        private bool _IsExtended;

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
            }
        }

        /// <summary>
        /// returns amount of victory point cards
        /// </summary>
        [DataMember]
        public int VpCount
        {
            get { return _VpCount; }
            set { _VpCount = value; OnPropertyChanged("VpCount"); }
        }

        /// <summary>
        /// Returns amount of robber cards
        /// </summary>
        [DataMember]
        public int RbCount
        {
            get { return _RbCount; }
            set { _RbCount = value; OnPropertyChanged("RbCount"); }
        }

        [DataMember]
        public int RobberCount
        {
            get { return _RobberCount; }
            set { _RobberCount = value; OnPropertyChanged("RobberCount"); }
        }

        [DataMember]
        public int MonoCount
        {
            get { return _MonoCount; }
            set { _MonoCount = value; OnPropertyChanged("MonoCount"); }
        }

        [DataMember]
        public int YopCount
        {
            get { return _YopCount; }
            set { _YopCount = value; OnPropertyChanged("YopCount"); }
        }

        public int CountAll
        { get { return _VpCount + _RbCount + _RobberCount + _MonoCount + _YopCount; } }

        /// <summary>
        /// Returns true if devcardstack is suitable for 3-4 players
        /// </summary>
        public bool IsStandard
        {
            get { return _IsStandard; }
            set
            {
                if (value)
                {
                    VpCount = 5;
                    YopCount = 2;
                    RobberCount = 14;
                    RbCount = 2;
                    MonoCount = 2;
                }
                _IsStandard = value;
                _IsExtended = !value;
                OnPropertyChanged("IsStandard");
                OnPropertyChanged("IsExtended");
            }
        }

        /// <summary>
        /// Returns true if the devcardstack is suitable for 5-6 players
        /// </summary>
        public bool IsExtended
        {
            get { return _IsExtended; }
            set
            {
                if (value)
                {
                    VpCount = 5;
                    YopCount = 3;
                    RobberCount = 20;
                    RbCount = 3;
                    MonoCount = 3;
                }
                _IsExtended = value;
                _IsStandard = !value;
                OnPropertyChanged("IsStandard");
                OnPropertyChanged("IsExtended");
            }
        }

        public StandardDevCardStack()
        {
        }
        public StandardDevCardStack(int vp, int rb, int robber, int mono, int yop)
        {
            _VpCount = vp;
            _RbCount = rb;
            _RobberCount = robber;
            _MonoCount = mono;
            _YopCount = yop;
        }

        public static StandardDevCardStack NormalStack
        { get { return new StandardDevCardStack(10, 10, 10, 10, 10); } }

        public static StandardDevCardStack ExtendedStack
        { get { return new StandardDevCardStack(5, 3, 19, 3, 3); } }

        public StandardDevCardStack Copy()
        {
            StandardDevCardStack result = new StandardDevCardStack();

            result.IsExtended = _IsExtended;
            result.IsStandard = _IsStandard;

            result.MonoCount = _MonoCount;
            result.RbCount = _RbCount;
            result.RobberCount = _RobberCount;
            result.VpCount = _VpCount;
            result.YopCount = _YopCount;

            return result;
        }
    }
}
