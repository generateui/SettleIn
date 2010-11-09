using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using SettleInCommon.User;
using SettleInCommon.Board;

namespace SettleInCommon.Gaming
{
    [DataContract]
    public class XmlGameSettings : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _PropertyChanged;

        private int _MinPlayers = 3;
        private int _MaxPlayers = 6;
        private bool _ReplaceDesertWithJungles = false;
        private bool _ReplaceDesertWithVolcanos = false;
        private bool _DoNotReplaceDeserts = true;
        private bool _TournamentStart = false;
        private int _NoSevensFirstRound = 0;
        private bool _No2VPPlayersRobbing = false;
        private bool _TradingAfterBuilding = false;
        private bool _ShowChitsAfterPlacing = false;
        private int _MaximumCardsInHandWhenSeven = 7;
        private int _MaximumTradesPerTurn = 2;
        private string _Name = string.Empty;
        private int _Host;
        private int _VpToWin = 10;
        private EGameType _GameType = EGameType.Standard;
        private Guid _BoardGuid;
        private bool _IsLadder = true;
        private XmlBoard _Board;

        public XmlBoard Board
        {
            get { return _Board; }
            set 
            {
                _Board = value;
                OnPropertyChanged("Board");
                if (_IsLadder)
                {
                    OnPropertyChanged("MinPlayers");
                    OnPropertyChanged("MaxPlayers");
                    OnPropertyChanged("VpToWin");
                    OnPropertyChanged("MaximumCardsInHandWhenSeven");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }

        public int MaximumTradesPerTurn
        {
            get { return _MaximumTradesPerTurn; }
            set
            {
                if (value != _MaximumTradesPerTurn)
                {
                    _MaximumTradesPerTurn = value;
                    OnPropertyChanged("MaximumTradesPerTurn");
                }
            }
        }

        [DataMember]
        public bool ShowChitsAfterPlacing
        {
            get { return _ShowChitsAfterPlacing; }
            set
            {
                if (_ShowChitsAfterPlacing != value)
                {
                    _ShowChitsAfterPlacing = value;
                    OnPropertyChanged("ShowChitsAfterPlacing");
                }
            }
        }

        [DataMember]
        public int MaximumCardsInHandWhenSeven
        {
            get 
            {
                if (_IsLadder && _Board!=null) 
                    return _Board.MaximumCardsInHandWhenSeven;
                else
                    return _MaximumCardsInHandWhenSeven; 
            }
            set { if (_MaximumCardsInHandWhenSeven != value) { _MaximumCardsInHandWhenSeven = value; OnPropertyChanged("MaximumCardsInHandWhenSeven"); } }
        }

        [DataMember]
        public bool ReplaceDesertWithJungles
        {
            get { return _ReplaceDesertWithJungles; }
            set { if (_ReplaceDesertWithJungles != value) { _ReplaceDesertWithJungles = value; OnPropertyChanged("ReplaceDesertWithJungles"); } }
        }
        
        [DataMember]
        public bool ReplaceDesertWithVolcanos 
        {
            get { return _ReplaceDesertWithVolcanos; }
            set { if (_ReplaceDesertWithVolcanos != value) { _ReplaceDesertWithVolcanos = value; OnPropertyChanged("ReplaceDesertWithVolcanos"); } }
        }

        [DataMember]
        public bool DoNotReplaceDeserts 
        {
            get { return _DoNotReplaceDeserts; }
            set { if (_DoNotReplaceDeserts != value) { _DoNotReplaceDeserts = value; OnPropertyChanged("DoNotReplaceDeserts"); } }
        }

        [DataMember]
        public bool TournamentStart
        {
            get { return _TournamentStart; }
            set { if (_TournamentStart != value) { _TournamentStart = value; OnPropertyChanged("TournamentStart"); } }
        }

        [DataMember]
        public int NoSevensFirstRound 
        {
            get { return _NoSevensFirstRound; }
            set { if (_NoSevensFirstRound != value) { _NoSevensFirstRound = value; OnPropertyChanged("NoSevensFirstRound"); } }
        }

        [DataMember]
        public bool No2VPPlayersRobbing 
        {
            get { return _No2VPPlayersRobbing; }
            set { if (_No2VPPlayersRobbing != value) { _No2VPPlayersRobbing = value; OnPropertyChanged("No2VPPlayersRobbing"); } }
        }

        [DataMember]
        public bool TradingAfterBuilding 
        {
            get { return _TradingAfterBuilding; }
            set { if (_TradingAfterBuilding != value) { _TradingAfterBuilding = value; OnPropertyChanged("TradingAfterBuilding"); } }
        }
        
        [DataMember]
        public string Name 
        {
            get { return _Name; }
            set { if (_Name != value) { _Name = value; OnPropertyChanged("Name"); } }
        }

        [DataMember]
        public int HostID
        {
            get { return _Host; }
            set { if (_Host != value) { _Host = value; OnPropertyChanged("Host"); } }
        }

        [DataMember]
        public int VpToWin
        {
            get 
            {
                if (_IsLadder && _Board !=null) return _Board.VpToWin;
                return _VpToWin; 
            }
            set { if (_VpToWin != value) { _VpToWin = value; OnPropertyChanged("VpToWin"); } }
        }

        [DataMember]
        public EGameType GameType
        {
            get { return _GameType; }
            set { if (_GameType != value) { _GameType = value; OnPropertyChanged("GameType"); } }
        }

        [DataMember]
        public Guid Map 
        {
            get { return _BoardGuid; }
            set { if (_BoardGuid != value) { _BoardGuid = value; OnPropertyChanged("Map"); } }
        }

        [DataMember]
        public int MaxPlayers 
        {
            get 
            {
                if (_IsLadder && _Board !=null)
                    return _Board.MaxPlayers;
                else
                    return _MaxPlayers; }
            set 
            { 
                if (_MaxPlayers != value) 
                { 
                    _MaxPlayers = value; 
                    OnPropertyChanged("MaxPlayers"); 
                } 
            }
        }

        [DataMember]
        public int MinPlayers
        {
            get 
            {
                if (_IsLadder && _Board != null) 
                    return _Board.MinPlayers;
                else
                    return _MinPlayers; 
            }
            set 
            { 
                if (_MinPlayers != value) 
                { 
                    _MinPlayers = value; 
                    OnPropertyChanged("MinPlayers"); 
                } 
            }
        }

        [DataMember]
        public bool IsLadder 
        {
            get { return _IsLadder; }
            set 
            { 
                if (_IsLadder != value) 
                { 
                    _IsLadder = value; 
                    OnPropertyChanged("IsLadder");
                    OnPropertyChanged("MinPlayers");
                    OnPropertyChanged("MaxPlayers");
                    OnPropertyChanged("VpToWin");
                    OnPropertyChanged("MaximumCardsInHandWhenSeven");
                } 
            }
        }

        public XmlGameSettings() { }

        public XmlGameSettings(string name, int maxPlayers)
        {
            Name = name;
            MaxPlayers = maxPlayers;
        }

        public override bool Equals(object obj)
        {
            XmlGameSettings settings = obj as XmlGameSettings;
            if (settings != null)
            {
                return true;
            }
            return false;
        }
        
        private void OnPropertyChanged(string p)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        /// <summary>
        /// Updates the object with the new settings.
        /// Used when a player changes his settings of the game
        /// </summary>
        /// <param name="settings"></param>
        public void UpdateSettings(XmlGameSettings settings)
        {
            if (_DoNotReplaceDeserts != settings.DoNotReplaceDeserts)
                DoNotReplaceDeserts = settings.DoNotReplaceDeserts;

            if (_GameType != settings.GameType)
                GameType = settings.GameType;

            if (_IsLadder != settings.IsLadder)
                IsLadder = settings.IsLadder;

            if (_BoardGuid != settings.Map)
                Map = settings.Map;

            if (_MaximumCardsInHandWhenSeven != settings.MaximumCardsInHandWhenSeven)
                MaximumCardsInHandWhenSeven = settings.MaximumCardsInHandWhenSeven;

            if (_MaxPlayers != settings.MaxPlayers)
                MaxPlayers = settings.MaxPlayers;

            if (_MinPlayers != settings.MinPlayers)
                MinPlayers = settings.MinPlayers;

            if (_Name != settings.Name)
                Name = settings.Name;

            if (_No2VPPlayersRobbing != settings.No2VPPlayersRobbing)
                No2VPPlayersRobbing = settings.No2VPPlayersRobbing;

            if (_NoSevensFirstRound != settings.NoSevensFirstRound)
                NoSevensFirstRound = settings.NoSevensFirstRound;

            if (_ReplaceDesertWithJungles != settings.ReplaceDesertWithJungles)
                ReplaceDesertWithJungles = settings.ReplaceDesertWithJungles;

            if (_ReplaceDesertWithVolcanos != settings.ReplaceDesertWithVolcanos)
                ReplaceDesertWithVolcanos = settings.ReplaceDesertWithVolcanos;

            if (_TournamentStart != settings.TournamentStart)
                TournamentStart = settings.TournamentStart;

            if (_TradingAfterBuilding != settings.TradingAfterBuilding)
                TradingAfterBuilding = settings.TradingAfterBuilding;

            if (_VpToWin != settings.VpToWin)
                VpToWin = settings.VpToWin;

        }

        public void SetLadder(XmlBoard board)
        {
            MaximumCardsInHandWhenSeven = board.MaximumCardsInHandWhenSeven;
            MinPlayers = board.MinPlayers;
            MaxPlayers = board.MaxPlayers;

        }

    }
}
