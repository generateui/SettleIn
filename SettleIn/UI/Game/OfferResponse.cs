using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using SettleInCommon.Actions.InGame;
using SettleInCommon.Board;
using SettleInCommon.Gaming;

namespace SettleIn.UI.Game
{
    public class OfferResponse : INotifyPropertyChanged
    {
        private GamePlayer _Player;
        private ResourceList _WantedResources;
        private ResourceList _OfferedResources;
        private OfferStatus _Status = OfferStatus.NotOfferedYet;

        public OfferStatus Status
        {
            get { return _Status; }
            set 
            { 
                _Status = value;
                OnPropertyChanged("Status");
            }
        }

        public GamePlayer Player
        {
            get { return _Player; }
            set { _Player = value; }
        }

        public ResourceList WantedResources
        {
            get { return _WantedResources; }
            set 
            {
                _WantedResources = value;
                OnPropertyChanged("WantedResources");
            }
        }

        public ResourceList OfferedResources
        {
            get { return _OfferedResources; }
            set 
            { 
                _OfferedResources = value;
                OnPropertyChanged("WantedResources");
            }
        }

        public void UpdateResponse(InGameAction action)
        {
            if (Player != null)
            {
                if (action.Sender == Player.XmlPlayer.ID)
                {
                    AcceptOfferAction accept = action as AcceptOfferAction;
                    if (accept != null)
                    {
                        UpdateAccept(accept);
                    }

                    RejectOfferAction reject = action as RejectOfferAction;
                    if (reject != null)
                    {
                        UpdateReject(reject);
                    }

                    CounterTradeOfferAction counter = action as CounterTradeOfferAction;
                    if (counter != null)
                    {
                        UpdateCounter(counter);
                    }
                }
            }
        }

        private void UpdateCounter(CounterTradeOfferAction action)
        {
            Status = OfferStatus.CounterOffer;
            WantedResources = action.WantedCards;
            OfferedResources = action.OfferedCards;
        }

        private void UpdateReject(RejectOfferAction action)
        {
            Status = OfferStatus.Declined;
        }

        private void UpdateAccept(AcceptOfferAction action)
        {
            Status = OfferStatus.Accepted;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
