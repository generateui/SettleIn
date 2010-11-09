using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace SettleInCommon.Gaming.DevCards
{
    [KnownType(typeof(Soldier))]
    [KnownType(typeof(VictoryPoint))]
    [KnownType(typeof(RoadBuilding))]
    [KnownType(typeof(Monopoly))]
    [KnownType(typeof(YearOfPlenty))]
    [KnownType(typeof(UnknownDevelopmentCard))]
    [DataContract]
    public class DevelopmentCard : INotifyPropertyChanged
    {
        private PropertyChangedEventHandler _PropertyChanged;
        protected string _InvalidMessage = string.Empty;
        protected string _Message = string.Empty;
        protected int _TurnBought = 0;
        private int _ID = 0;
        private bool _IsPlayable = false;

        [DataMember]
        public bool IsPlayable
        {
            get { return _IsPlayable; }
            set 
            { 
                if (value != _IsPlayable)
                {
                    _IsPlayable = value; 
                    OnPropertyChanged("IsPlayable");
                }
            }
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public virtual void Execute(XmlGame game, GamePlayer player)
        {
            _IsPlayable = false;
            //player.PlayedDevcards.Add(this);
            //player.DevCards.Remove(this);
        }

        public string Message 
        {
            get
            {
                return _Message;
            }
        }
        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public virtual bool IsValid(XmlGame game)
        {
            return true;
        }
        public virtual DevelopmentCard Copy()
        {
            return (DevelopmentCard)this.MemberwiseClone();
        }
        public virtual bool WaitOneTurn
        {
            get { return true; }
        }

        [DataMember]
        public int TurnBought
        {
            get { return _TurnBought;}
            set { _TurnBought = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
