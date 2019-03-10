using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Endless.ComponentModel
{
    public class ObservableCollectionFilter<T> where T : class //where T : INotifyPropertyChanged 
    {
        private IReadOnlyCollection<T> source;
        private readonly ICollection<T> filtered;
        private readonly Func<T,bool> filterfunc;

        public ObservableCollectionFilter(IReadOnlyCollection<T> source,
            ICollection<T> filtered, Func<T, bool> filter)
        {
            this.source = source;
            this.filtered = filtered;
            this.filterfunc = filter;

            (source as INotifyCollectionChanged).CollectionChanged += SourceCollectionChanged;

            foreach (var item in source)
            {
                //item.PropertyChanged += ItemPropertyChanged;
                ApplyFilter(item);
            }
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:

                    if (e.OldItems != null)
                    {
                        foreach (T item in e.OldItems)
                        {
                            if (filtered.Contains(item))
                            {
                                filtered.Remove(item);
                                //item.PropertyChanged -= ItemPropertyChanged;
                            }
                        }
                    }
                    if (e.NewItems != null)
                    {
                        foreach (T item in e.NewItems)
                        {
                            ApplyFilter(item);
                            //item.PropertyChanged += ItemPropertyChanged;
                        }
                    }
                    break;
                default:
                    Debugger.Break();
                    break;
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            T item = sender as T;

            Contract.Assume(item != null);

            ApplyFilter(item);
        }

        private void ApplyFilter(T item)
        {
            bool isFiltered = filterfunc(item);

            if (isFiltered)
            {
                if (!filtered.Contains(item))
                {
                    filtered.Add(item);
                }
            }
            else
            {
                if (filtered.Contains(item))
                {
                    filtered.Remove(item);
                }
            }
        }
    }
}