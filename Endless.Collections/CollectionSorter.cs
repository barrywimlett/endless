using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Endless
{

    
    public class CollectionSorter<T>
    {
        protected Func<ObservableCollection<T>, IEnumerable<T>> SortCollection { get; set; }
        protected ObservableCollection<T> Observable { get; set; }
        protected bool IsSorting { get; set; }

        public CollectionSorter(ObservableCollection<T> observable, Func<ObservableCollection<T>, IEnumerable<T>> sorter)
        {
            this.Observable = observable;
            this.SortCollection = sorter;

            this.Observable.CollectionChanged += Observable_CollectionChanged;

            
            Sort();
        }

        private void Observable_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!IsSorting)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Move:
                        break;

                    case NotifyCollectionChangedAction.Add:
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Replace:
                        var wasSorting = IsSorting;
                        IsSorting = true;
                        Sort(); 
                        IsSorting = wasSorting;
                        break;
                }
            }
        }

        public void Sort()
        {
            Sort(Observable, SortCollection);
        }

        public static void Sort(ObservableCollection<T> observable, Func<ObservableCollection<T>, IEnumerable<T>> sorter) 
        {
            List<T> sorted = new List<T>(sorter(observable));

            int ptr = 0;
            while (ptr < sorted.Count)
            {
                if (!observable[ptr].Equals(sorted[ptr]))
                {
                    T t = sorted[ptr];
                    //observable.RemoveAt(ptr);
                    int oldPos = observable.IndexOf(t);
                    observable.Move(oldPos,ptr);
                    //observable.Insert(newPos,t);
                }
                ptr++;
                
            }
        }

        
    }
}