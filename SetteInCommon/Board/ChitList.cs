using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;

namespace SettleInCommon.Board
{
    [DataContract]
    [Serializable]
    public class XmlChitList : INotifyPropertyChanged
    {
        int _N2 = 1;
        int _N3 = 2;
        int _N4 = 2;
        int _N5 = 2;
        int _N6 = 2;
        int _N8 = 2;
        int _N9 = 2;
        int _N10 = 2;
        int _N11 = 2;
        int _N12 = 1;

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

        [DataMember]
        public int N2
        {
            get { return _N2; }
            set { if (value != _N2) { _N2 = value; OnPropertyChanged("N2"); } }
        }

        [DataMember]
        public int N3
        {
            get { return _N3; }
            set { if (value != _N3) { _N3 = value; OnPropertyChanged("N3"); } }
        }

        [DataMember]
        public int N4
        {
            get { return _N4; }
            set { if (value != _N4) { _N4 = value; OnPropertyChanged("N4"); } }
        }

        [DataMember]
        public int N5
        {
            get { return _N5; }
            set { if (value != _N5) { _N5 = value; OnPropertyChanged("N5"); } }
        }

        [DataMember]
        public int N6
        {
            get { return _N6; }
            set { if (value != _N6) { _N6 = value; OnPropertyChanged("N6"); } }
        }

        [DataMember]
        public int N8
        {
            get { return _N8; }
            set { if (value != _N8) { _N8 = value; OnPropertyChanged("N8"); } }
        }

        [DataMember]
        public int N9
        {
            get { return _N9; }
            set { if (value != _N9) { _N9 = value; OnPropertyChanged("N9"); } }
        }
        
        [DataMember]
        public int N10
        {
            get { return _N10; }
            set { if (value != _N10) { _N10 = value; OnPropertyChanged("N10"); } }
        }

        [DataMember]
        public int N11
        {
            get { return _N11; }
            set { if (value != _N11) { _N11 = value; OnPropertyChanged("N11"); } }
        }

        [DataMember]
        public int N12
        {
            get { return _N12; }
            set { if (value != _N12) { _N12 = value; OnPropertyChanged("N12"); } }
        }

        public int CountAll
        { get { return N2 + N3 + N4 + N5 + N6 + N8 + N9 + N10 + N11 + N12; } }

        public XmlChitList()
        {
        }

        /// <summary>
        /// Bag of chitnumber to swap on the board
        /// </summary>
        /// <returns></returns>
        public static XmlChitList SwapBag()
        {
            return new XmlChitList()
            {
                N2 = 1,
                N3 = 1,
                N4 = 1,
                N5 = 1,
                N6 = 0,
                N8 = 0,
                N9 = 1,
                N10 = 1,
                N11 = 1,
                N12 = 0
            };
        }        
        /// <summary>
        /// Returns list of numbers of a standard board
        /// </summary>
        /// <returns></returns>
        public static XmlChitList Standard()
        {
            return new XmlChitList()
            {
                N2 = 1,
                N3 = 2,
                N4 = 2,
                N5 = 2,
                N6 = 2,
                N8 = 2,
                N9 = 2,
                N10 = 2,
                N11 = 2,
                N12 = 1
            };
        }

        public int[] ToArray()
        {
            int[] result = new int[11];
            result[0] = _N2;
            result[1] = _N3;
            result[2] = _N4;
            result[3] = _N5;
            result[4] = _N6;
            result[5] = 0; //7, so must be empty.
            result[6] = _N8;
            result[7] = _N9;
            result[8] = _N10;
            result[9] = _N11;
            result[10] = _N12;

            return result;
        }

        public XmlChitList Copy()
        {
            XmlChitList result = new XmlChitList();

            result._N2 = _N2;
            result._N3 = _N3;
            result._N4 = _N4;
            result._N5 = _N5;
            result._N6 = _N6;
            result._N8 = _N8;
            result._N9 = _N9;
            result._N10 = _N10;
            result._N11 = _N11;
            result._N12 = _N12;

            return result;
        }

        public EChitNumber PickNumberFromBag()
        {
            int number = (int)(CommonCore.Instance.Random.NextDouble() * CountAll);
            int running = 0;

            //Fallthrough algorithm since we store an int for each type
            if (_N2 <= number)
            {
                running += _N2;
            }
            else
            {
                _N2--;
                return EChitNumber.N2;
            }
            if ((_N3 + running) <= number)
            {
                running += _N3;
            }
            else
            {
                _N3--;
                return EChitNumber.N3;
            }
            if ((_N4 + running) <= number)
            {
                running += _N4;
            }
            else
            {
                _N4--;
                return EChitNumber.N4;
            }
            if ((_N5 + running) <= number)
            {
                running += _N5;
            }
            else
            {
                _N5--;
                return EChitNumber.N5;
            }
            if ((_N6 + running) <= number)
            {
                running += _N6;
            }
            else
            {
                _N6--;
                return EChitNumber.N6;
            }
            if ((_N8 + running) <= number)
            {
                running += _N8;
            }
            else
            {
                _N8--;
                return EChitNumber.N8;
            }
            if ((_N9 + running) <= number)
            {
                running += _N9;
            }
            else
            {
                _N9--;
                return EChitNumber.N9;
            }
            if ((_N10 + running) <= number)
            {
                running += _N10;
            }
            else
            {
                _N10--;
                return EChitNumber.N10;
            }
            if ((_N11 + running) <= number)
            {
                running += _N11;
            }
            else
            {
                _N11--;
                return EChitNumber.N11;
            }
            if ((_N12 + running) <= number)
            {
                running += _N12;
                throw new Exception("whoops! picking number higher then amount of chits in bag");
            }
            else
            {
                _N12--;
                return EChitNumber.N12;
            }
        }

        public EChitNumber PickRandomChit()
        {
            int chitno = (int)(CommonCore.Instance.Random.NextDouble() * 10);
            switch (chitno)
            {
                case 1: return EChitNumber.N2;
                case 2: return EChitNumber.N3;
                case 3: return EChitNumber.N4;
                case 4: return EChitNumber.N5;
                case 5: return EChitNumber.N6;
                case 6: return EChitNumber.N8;
                case 7: return EChitNumber.N9;
                case 8: return EChitNumber.N10;
                case 9: return EChitNumber.N11;
                case 0: return EChitNumber.N12;
            }
            throw new Exception("Hmm, something went wrong here");
        }
    }
}
