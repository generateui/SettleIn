using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SettleInCommon
{
    public class CollectionPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        private CollectionChangeAction _Action = CollectionChangeAction.Refresh;
        private object _Item;

        public object Item
        {
            get { return _Item; }
            set { _Item = value; }
        }

        public CollectionChangeAction Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        public CollectionPropertyChangedEventArgs(string name) : base(name) { }
    }
}
