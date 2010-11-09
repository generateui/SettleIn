using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Collections;
using System.Collections.Specialized;

namespace SettleInCommon.Board
{
    public class XmlPortList : INotifyPropertyChanged,  IEnumerator<EPortType>, IEnumerable<EPortType>, INotifyCollectionChanged
    {
        private int _Timber = 0;
        private int _Wheat = 0;
        private int _Ore = 0;
        private int _Clay = 0;
        private int _Sheep = 0;
        private int _ThreeToOne = 0;

        private event PropertyChangedEventHandler _PropertyChanged;
        private event NotifyCollectionChangedEventHandler _CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(_PropertyChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _PropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(_PropertyChanged, value); }
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add { _CollectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Combine(_CollectionChanged, value); }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _CollectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Remove(_CollectionChanged, value); }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_CollectionChanged != null)
                _CollectionChanged(sender, e);
        }

        private void OnPropertyChanged(string p)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(p));
                _PropertyChanged(this, new PropertyChangedEventArgs("CountAll"));
                Console.WriteLine(p);
            }
        }

        [DataMember]
        public int Timber
        {
            get { return _Timber; }
            set 
            { 
                if (value != _Timber) 
                { 
                    _Timber = value; OnPropertyChanged("Timber");
                    OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                } 
            }
        }
        
        [DataMember]
        public int Wheat
        {
            get { return _Wheat; }
            set 
            { 
                if (value != _Wheat) 
                { 
                    _Wheat = value; OnPropertyChanged("Wheat");
                    OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                } 
            }
        }
        
        [DataMember]
        public int Ore
        {
            get { return _Ore; }
            set 
            { 
                if (value != _Ore) 
                { 
                    _Ore = value; OnPropertyChanged("Ore");
                    OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                } 
            }
        }
        
        [DataMember]
        public int Clay
        {
            get { return _Clay; }
            set 
            { 
                if (value != _Clay) 
                { 
                    _Clay = value; OnPropertyChanged("Clay");
                    OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        [DataMember]
        public int Sheep
        {
            get 
            { 
                return _Sheep; 
            }
            set 
            { 
                if (value != _Sheep) 
                { 
                    _Sheep = value; OnPropertyChanged("Sheep");
                    OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                } 
            }
        }

        [DataMember]
        public int ThreeToOne
        {
            get 
            { return _ThreeToOne; }
            set 
            { 
                if (value != _ThreeToOne) 
                { 
                    _ThreeToOne = value; OnPropertyChanged("ThreeToOne");
                    OnCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                } 
            }
        }

        public int CountAll
        { get { return _Timber + _Wheat + _Ore + _Clay + _Sheep + _ThreeToOne; } }

        public XmlPortList Copy()
        {
            XmlPortList result = new XmlPortList();

            result.Clay = _Clay;
            result.Ore = _Ore;
            result.Sheep = _Sheep;
            result.ThreeToOne = _ThreeToOne;
            result.Timber = _Timber;
            result.Wheat = _Wheat;

            return result;
        }
        public static XmlPortList Empty()
        {
            XmlPortList result = new XmlPortList();

            result._Clay = 0;
            result._Ore = 0;
            result._Sheep = 0;
            result._ThreeToOne = 0;
            result._Timber = 0;
            result._Wheat = 0;

            return result;
        }
        public void Remove(EPortType type)
        {
            switch (type)
            {
                case EPortType.Clay: Clay--; break;
                case EPortType.Ore: Ore--; break;
                case EPortType.Sheep: Sheep--; break;
                case EPortType.ThreeToOne: ThreeToOne--; break;
                case EPortType.Timber: Timber--; break;
                case EPortType.Wheat: Wheat--; break;
            }
            OnCollectionChanged(this,
               new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, type));
        }
        public void Add(EPortType type)
        {
            switch (type)
            {
                case EPortType.Clay: Clay++; break;
                case EPortType.Ore: Ore++; break;
                case EPortType.Sheep: Sheep++; break;
                case EPortType.ThreeToOne: ThreeToOne++; break;
                case EPortType.Timber: Timber++; break;
                case EPortType.Wheat: Wheat++; break;
            }
            OnCollectionChanged(this,
               new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, type));
        }

        public int this[EResource resource]
        {
            get
            {
                int divider=4;
                if (_ThreeToOne > 0)
                    divider = 3;
                switch (resource)
                {
                    case EResource.Timber:
                        if (_Timber > 0) divider = 2;
                        return divider;
                    case EResource.Wheat:
                        if (_Wheat > 0) divider = 2;
                        return divider;
                    case EResource.Ore:
                        if (_Ore > 0) divider = 2;
                        return divider;
                    case EResource.Clay:
                        if (_Clay > 0) divider = 2;
                        return divider;
                    case EResource.Sheep:
                        if (_Sheep > 0) divider = 2;
                        return divider;
                }
                return 4;
            }
        }

        public EPortType PickPortFromBag()
        {
            int number = (int)(CommonCore.Instance.Random.NextDouble() * CountAll);
            int running = 0;
            if (_Clay <= number)
            {
                running += _Clay;
            }
            else
            {
                _Clay--;
                return EPortType.Clay;
            }

            if ((_Ore + running) <= number)
            {
                running += _Ore;
            }
            else
            {
                _Ore--;
                return EPortType.Ore;
            }

            if ((_Sheep + running) <= number)
            {
                running += _Sheep;
            }
            else
            {
                _Sheep--;
                return EPortType.Sheep;
            }

            if ((_ThreeToOne + running) <= number)
            {
                running += _ThreeToOne;
            }
            else
            {
                _ThreeToOne--;
                return EPortType.ThreeToOne;
            }

            if ((_Timber + running) <= number)
            {
                running += _Timber;
            }
            else
            {
                _Timber--;
                return EPortType.Timber;
            }

            if ((_Wheat + running) <= number)
            {
                running += _Wheat;
                throw new Exception("number bigger then expected in list of ports. Whoops!");
            }
            else
            {
                _Wheat--;
                return EPortType.Wheat;
            }
        }

        #region IEnumerator<EPortType> Members

        public EPortType Current
        {
            get { if (_CurrentIndex != -1) return _TempList[_CurrentIndex]; else throw null; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region IEnumerator Members

        private int _CurrentIndex = -1;
        private List<EPortType> _TempList = new List<EPortType>(); 

        object IEnumerator.Current
        {
            get { if (_CurrentIndex != -1) return _TempList[_CurrentIndex]; else throw null; }
        }

        List<EPortType> GetList()
        {
            List<EPortType> result = new List<EPortType>();

            for (int i = 0; i < _Wheat; i++)
                result.Add(EPortType.Wheat);
            
            for (int i = 0; i < _Timber; i++)
                result.Add(EPortType.Timber);
            
            for (int i = 0; i < _Ore; i++)
                result.Add(EPortType.Ore);
            
            for (int i = 0; i < _Clay; i++)
                result.Add(EPortType.Clay);
            
            for (int i = 0; i < _Sheep; i++)
                result.Add(EPortType.Sheep);
            
            for (int i = 0; i < _ThreeToOne; i++)
                result.Add(EPortType.ThreeToOne);

            return result;
        }

        public bool MoveNext()
        {
            if (_CurrentIndex < _TempList.Count && _CurrentIndex != -1)
            {
                _CurrentIndex++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _CurrentIndex = - 1;
            _TempList = GetList();
        }

        #endregion

        #region IEnumerable<EPortType> Members

        public IEnumerator<EPortType> GetEnumerator()
        {
            return (IEnumerator<EPortType>)this;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }

        #endregion

        #region INotifyCollectionChanged Members


        #endregion
    }
}
