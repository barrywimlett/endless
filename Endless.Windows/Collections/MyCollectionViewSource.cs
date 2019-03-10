using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Endless.Collections;

namespace Endless.Windows.Collections
{
    public class MyCollectionViewSource<T> where T:class
    {
        IReadOnlyCollection<T> SourceCollection;

        

        readonly ObservableCollectionWithReadOnly<GroupingDescription<T, object>> groupingDescriptions =
            new ObservableCollectionWithReadOnly<GroupingDescription<T, object>>();

        public ReadOnlyCollection<GroupingDescription<T, object>> GroupingDescriptions => groupingDescriptions.ReadOnly;

        public IObservableCollection<object> Groupings =new ObservableCollectionWithReadOnly<object>();

        private ObservableCollectionWatcher<object> watcher;
        
        public MyCollectionViewSource(IReadOnlyCollection<T> sourceCollection)
        {
            SourceCollection = sourceCollection;
            var asNotifyCollectionChanged = sourceCollection as INotifyCollectionChanged;
            watcher = new ObservableCollectionWatcher<object>(asNotifyCollectionChanged);
            watcher.PropertyChanged += ItemPropertyChangedEventHandler;
            if (asNotifyCollectionChanged != null)
            {
                asNotifyCollectionChanged.CollectionChanged += CollectionChangedEventHandler;
            }

            
        }

        public MyCollectionViewSource(Collection<T> sourceCollection)
        {
            SourceCollection = sourceCollection;
            var asNotifyCollectionChanged = sourceCollection as INotifyCollectionChanged;
            watcher = new ObservableCollectionWatcher<object>(asNotifyCollectionChanged);
            watcher.PropertyChanged += ItemPropertyChangedEventHandler;
            if (asNotifyCollectionChanged != null)
            {
                asNotifyCollectionChanged.CollectionChanged += CollectionChangedEventHandler;
            }

            foreach (var item in sourceCollection)
            {
                AddItem(item);
            }
        }

        public MyCollectionViewSource(IReadOnlyCollection<T> sourceCollection, GroupingDescription<T, object>[] groupingDescriptions) : this(sourceCollection)
        {
            this.groupingDescriptions.Add(groupingDescriptions);

            foreach (var item in sourceCollection)
            {
                AddItem(item);
            }
        }

        private void CollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (this.Groupings)
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    this.Groupings.Clear();
                }
                if (e.NewItems != null)
                {
                    foreach (object item in e.NewItems)
                    {
                        AddItem(item);
                    }

                }

                if (e.OldItems != null)
                {
                    foreach (object item in e.OldItems)
                    {
                        ICollection<object> collectionToAddTo = this.Groupings;

                        foreach (var groupDescription in groupingDescriptions)
                        {
                            var key = groupDescription.GenerateKey(item as T);
                            var enumerable = collectionToAddTo.Cast<GroupedObservableCollection<object, object>>();
                            var grouping = enumerable.SingleOrDefault(g => g.Key.Equals(key));

                            if (grouping == null)
                            {
                                Debug.Assert(RecursivelyRemove(collectionToAddTo, item));
                                collectionToAddTo = null;
                                break;
                            }
                            else
                            {
                                collectionToAddTo = grouping;
                                
                            }
                        }

                        if (collectionToAddTo != null)
                        {
                            try
                            {
                                if (collectionToAddTo.Contains(item))
                                {
                                    collectionToAddTo.Remove(item);
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.Assert(false);
                            }
                        }
                    }
                    
                }

                RemoveEmpties(this.Groupings);
            }
        }

        private void AddItem(object item)
        {
            ICollection<object> collectionToAddTo = this.Groupings;

            foreach (var groupDescription in groupingDescriptions)
            {
                var key = groupDescription.GenerateKey(item as T);
                if (key == null)
                {
                    Debug.Assert(key != null);
                }
                else
                {
                    var enumerable = collectionToAddTo.Cast<GroupedObservableCollection<object, object>>();

                    var grouping = enumerable.SingleOrDefault(g => g.Key.Equals(key));
                    if (grouping == null)
                    {
                        GroupedObservableCollection<object, object> newGroup =
                            groupDescription.ChildFactory(key);

                        collectionToAddTo.Add(newGroup);
                        grouping = newGroup;
                    }

                    collectionToAddTo = grouping;
                }
            }

            try
            {
                collectionToAddTo.Add(item);
            }
            catch (Exception ex)
            {
                Debug.Assert(false);
            }
        }

        private bool RecursivelyRemove(ICollection<object> collection, object item)
        {
            bool result = false;
            if (collection.Contains(item))
            {
                collection.Remove(item);
                result = true;
            }
            else
            {

                foreach (var collectionItem in collection)
                {
                    var asCollection = collectionItem as ICollection<object>;
                    if (asCollection != null)
                    {
                        var recurseResult = RecursivelyRemove(asCollection, item);
                        if (recurseResult)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private void RemoveEmpties(IObservableCollection<object> observableCollection)
        {
            var enumerable = observableCollection.Where(x=> x is IObservableCollection<object>).Cast<IObservableCollection<object>>();
            foreach (var e in enumerable)
            {
                RemoveEmpties(e);
            }

            var empty = enumerable.Where(g => g.Count == 0).ToList();
            foreach (var e in empty)
            {
                observableCollection.Remove(e);
            }
        }

        private void ItemPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            
        }

        public void AddGroupingDescription(GroupingDescription<T,object> groupBySystem)
        {
            this.groupingDescriptions.Add(groupBySystem);
            this.Groupings.Clear();

            foreach (var item in this.SourceCollection)
            {
                AddItem(item);
            }
        }
    }
}