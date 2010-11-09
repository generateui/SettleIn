using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SettleInCommon.Board
{
    /// <summary>
    /// Represents a stack of hexes, in where the amount of each hex type is kept.
    /// </summary>
    public class ResourceList : ObservableCollection<EResource>
    {
        private event PropertyChangedEventHandler _PropertyChanged;

        public new event PropertyChangedEventHandler PropertyChanged
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
                if (p != "Discoveries") _PropertyChanged(this, new PropertyChangedEventArgs("CountAllExceptDiscovery"));
                Console.WriteLine(p);
            }
        }


        /// <summary>
        /// Returns amount of resources for given resource type
        /// "bool Overload" is to not let the C# compiler complain.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="Overload"></param>
        /// <returns></returns>
        public int this[EResource resource, bool Overload]
        {
            get
            {
                switch (resource)
                {
                    case EResource.Clay: return Clay;
                    case EResource.Discovery: return Discoveries;
                    case EResource.Ore: return Ore;
                    case EResource.Sheep: return Sheep;
                    case EResource.Timber: return Timber;
                    case EResource.Wheat: return Wheat;
                }
                return 0;
            }
            set
            {
                switch (resource)
                {
                    case EResource.Clay: Clay = value; return;
                    case EResource.Discovery: Discoveries = value; return;
                    case EResource.Ore: Ore = value; return;
                    case EResource.Sheep: Sheep = value; return;
                    case EResource.Timber: Timber = value; return;
                    case EResource.Wheat: Wheat = value; return;
                }
            }
        }

        public int Timber
        {
            get { return this.Count(r => r == EResource.Timber); }
            set
            {
                if (value < 0) value = 0;
                if (value != Timber && value > -1)
                {
                    if (value < Timber)
                    {
                        int temp = Timber;
                        for (int i = 0; i < temp - value; i++)
                            this.Remove(EResource.Timber);
                    }
                    if (value > Timber)
                    {
                        int temp = Timber; 
                        for (int i = 0; i < value - temp; i++)
                            this.Add(EResource.Timber);
                    }
                }
            }
        }
        public int Wheat
        {
            get { return this.Count(r => r == EResource.Wheat); }
            set
            {
                if (value < 0) value = 0;
                if (value != Wheat && value > -1)
                {
                    if (value < Wheat)
                    {
                        int temp = Wheat;
                        for (int i = 0; i < temp - value; i++)
                            this.Remove(EResource.Wheat);
                    }
                    if (value > Wheat)
                    {
                        int temp = Wheat;
                        for (int i = 0; i < value - temp; i++)
                            this.Add(EResource.Wheat);
                    }
                }
            }
        }
        public int Ore
        {
            get { return this.Count(r => r == EResource.Ore); }
            set
            {
                if (value < 0) value = 0;
                if (value != Ore && value > -1)
                {
                    if (value < Ore)
                    {
                        int temp = Ore;
                        for (int i = 0; i < temp - value; i++)
                            this.Remove(EResource.Ore);
                    }
                    if (value > Ore)
                    {
                        int temp = Ore;
                        for (int i = 0; i < value - temp; i++)
                            this.Add(EResource.Ore);
                    }
                }
            }
        }
        public int Clay
        {
            get { return this.Count(r => r == EResource.Clay); }
            set
            {
                if (value < 0) value = 0;
                if (value != Clay && value > -1)
                {
                    if (value < Clay)
                    {
                        int temp = Clay;
                        for (int i = 0; i < temp - value; i++)
                            this.Remove(EResource.Clay);
                    }
                    if (value > Clay)
                    {
                        int temp = Clay;
                        for (int i = 0; i < value - temp; i++)
                            this.Add(EResource.Clay);
                    }
                }
            }
        }
        public int Sheep
        {
            get { return this.Count(r => r == EResource.Sheep); }
            set
            {
                if (value < 0) value = 0;
                if (value != Sheep && value > -1)
                {
                    if (value < Sheep)
                    {
                        int temp = Sheep;
                        for (int i = 0; i < temp - value; i++)
                            this.Remove(EResource.Sheep);
                    }
                    if (value > Sheep)
                    {
                        int temp = Sheep;
                        for (int i = 0; i < value - temp; i++)
                            this.Add(EResource.Sheep);
                    }
                }
            }
        }
        public int Discoveries
        {
            get { return this.Count(r => r == EResource.Discovery); }
            set
            {
                if (value < 0) value = 0;
                if (value != Discoveries && value > -1)
                {
                    if (value < Discoveries)
                    {
                        int temp = Discoveries;
                        for (int i = 0; i < temp - value; i++)
                            this.Remove(EResource.Discovery);
                    }
                    if (value > Discoveries)
                    {
                        int temp = Discoveries;
                        for (int i = 0; i < value - temp; i++)
                            this.Add(EResource.Discovery);
                    }
                }
            }
        }
        public int Gold
        {
            get { return this.Count(r => r == EResource.Gold); }
            set
            {
                if (value < 0) value = 0;
                if (value != Gold && value > -1)
                {
                    if (value < Gold)
                    {
                        int temp = Gold;
                        for (int i = 0; i < temp - value; i++)
                            this.Remove(EResource.Gold);
                    }
                    if (value > Gold)
                    {
                        int temp = Gold;
                        for (int i = 0; i < value - temp; i++)
                            this.Add(EResource.Gold);
                    }
                }
            }
        }

        public static ResourceList City
        {
            get { return new ResourceList() { Wheat = 2, Ore = 3 }; }
        }

        public static ResourceList Town
        {
            get { return new ResourceList() { Wheat = 1, Timber = 1, Clay = 1, Sheep = 1 }; }
        }

        public static ResourceList Road
        {
            get { return new ResourceList() { Timber = 1, Clay = 1 }; }
        }

        public static ResourceList Ship
        {
            get { return new ResourceList() { Timber = 1, Sheep = 1 }; }
        }

        public static ResourceList Devcard
        {
            get { return new ResourceList() { Ore = 1, Sheep = 1, Wheat = 1 }; }
        }

        /// <summary>
        /// Returns number of resources in the list, ignoring discoveries
        /// </summary>
        public int CountAllExceptDiscovery
        { get { return Count - this.Count(r => r == EResource.Discovery); } }

        /// <summary>
        /// Returns number of resources in the list, counting discoveries also
        /// </summary>
        public int CountAll
        { get { return Count; } }

        public ResourceList(int timber, int wheat, int ore, int clay, int sheep, int discoveries)
        {
            for (int i = 0; i < timber; i++)
                Add(EResource.Timber);
            for (int i = 0; i < wheat; i++)
                Add(EResource.Wheat);
            for (int i = 0; i < ore; i++)
                Add(EResource.Ore);
            for (int i = 0; i < clay; i++)
                Add(EResource.Clay);
            for (int i = 0; i < sheep; i++)
                Add(EResource.Sheep);
            
            for (int i = 0; i < discoveries; i++)
                Add(EResource.Discovery);
        }

        /// <summary>
        /// adds specified resource from the list 
        /// </summary>
        /// <param name="resource"></param>
        public void AddResource(EResource resource)
        {
            Add(resource);
        }
        /// <summary>
        /// adds specified resource from the list 
        /// </summary>
        /// <param name="resource"></param>
        public void AddResources(EResource resource, int amount)
        {
            for (int i = 0; i < amount; i++)
                Add(resource);
        }

        public ResourceList()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(ResourceList_CollectionChanged);
        }

        void ResourceList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                string name = Enum.GetName(typeof(EResource), e.NewItems[0]);
                OnPropertyChanged(name);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
                OnPropertyChanged(Enum.GetName(typeof(EResource), e.OldItems[0]));
        }

        /// <summary>
        /// removes specified resource from the list when contained
        /// </summary>
        /// <param name="resource"></param>
        public void RemoveResource(EResource resource)
        {
            if (Contains(resource)) Remove(resource);

        }

        public void AddCards(ResourceList list)
        {
            foreach (EResource r in list)
                    Add(r);
        }

        public void SubtractCards(ResourceList list)
        {
            foreach (EResource r in list)
                if (Contains(r)) Remove(r);
            
        }

        public ResourceList Copy()
        {
            ResourceList result = new ResourceList();

            result.AddCards(this);

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Timber > 0) sb.Append(" " + Timber + " timber{0}");
            if (Wheat > 0) sb.Append(" " + Wheat + " wheat{0}");
            if (Ore > 0) sb.Append(" " + Ore + " ore{0}");
            if (Sheep > 0) sb.Append(" " + Sheep + " sheep{0}");
            if (Clay > 0) sb.Append(" " + Clay + " clay{0}");
            if (Discoveries > 0) sb.Append(" " + Discoveries + " discoveries{0}");

            string result = String.Format(sb.ToString(), ",");
            return result.Length > 0 ?
                result.Substring(0, result.Length - 1) : "";
        }

        public bool HasAtLeast(ResourceList toHave)
        {
            return
                Clay >= toHave.Clay &&
                Ore >= toHave.Ore &&
                Sheep >= toHave.Sheep &&
                Timber >= toHave.Timber &&
                Wheat >= toHave.Wheat &&
                Discoveries >= toHave.Discoveries;
        }


        public int HalfCount()
        {
            int count = CountAllExceptDiscovery;
            // Make numbr even
            if (count % 2 == 1) count--;
            return count / 2;
        }
    }
}
