using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Endless.Windows.Annotations;

namespace Endless.Windows.Collections
{
    public abstract class GroupingDescription<T,K>
    {
        public abstract K GenerateKey(T obj);
        
        public Func<object, GroupedObservableCollection<object, object>> ChildFactory { get; protected set; }
    }

    public class FunctionBasedGroupingDescription<T,K> : GroupingDescription<T, K>
    {
        protected Func<T,K> Function { get; set; }

        public FunctionBasedGroupingDescription(Func<T, K> function, Func<object, GroupedObservableCollection<object, object>> childFactory)
        {
            Function = function;
            ChildFactory = childFactory;

        }

        public override K GenerateKey(T obj)
        {
            return Function(obj);
        }

    }


    public class GroupedObservableCollection<T,K> : ObservableCollectionWithReadOnly<T>
    {
        public K Key { get; protected set; }

        public Func<object, GroupedObservableCollection<object, object>> GroupingFactory;
        public GroupedObservableCollection(K key)
        {
            this.Key = key;
            GroupingFactory = DefaultGroupingFactory;
        }

        public GroupedObservableCollection(K key, Func<object, GroupedObservableCollection<object, object>> func)
        {
            this.Key = key;
            GroupingFactory = func;
        }

        protected GroupedObservableCollection<object, object> DefaultGroupingFactory(object key)
        {
            return new GroupedObservableCollection<object, object>(key);
        }
    }

    public class KeyedObservableCollection<T,K> where T:class
    {
        private readonly Func<T, K> keyFunction;
        
        private readonly Dictionary<K, ObservableCollectionWithReadOnly<T>> dictionary= new Dictionary<K, ObservableCollectionWithReadOnly<T>>();
        private readonly IEnumerable<string> propertyNames =null;


        public KeyedObservableCollection(INotifyCollectionChanged sourceCollection, Func<T, K> keyFunction)
        {

            Contract.Assume(sourceCollection != null, "Does not impliment INotifyCollectionChanged");
            sourceCollection.CollectionChanged += SourceCollectionOnCollectionChanged;
            this.keyFunction = keyFunction;


        }

        public KeyedObservableCollection(INotifyCollectionChanged sourceCollection, Func<T, K> keyFunction,
            string propertyName) : this(sourceCollection, keyFunction, new string[] {propertyName})
        {
        }

        public KeyedObservableCollection(INotifyCollectionChanged sourceCollection, Func<T, K> keyFunction,IEnumerable<string> propertyNames) : this(sourceCollection,keyFunction)
        {
            this.propertyNames = this.propertyNames;
            foreach (T item in sourceCollection as ICollection<T>)
            {
                var collection = GetCollection(item);
                var model = item as ModelBase;
                if (model != null)
                {
                    model.PropertyChanged += ItemNotifyPropertyChanged;
                }
                collection.Add(item);
            }
        }

        
        private void ItemNotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var notify = e as ExtendedPropertyChangedEventArgs;
            if (propertyNames!=null && propertyNames.Contains(e.PropertyName))
            {
                if (notify != null)
                {
                    GetCollection((K) notify.OldValue).Remove(sender as T);
                    GetCollection((K) notify.NewValue).Add(sender as T);
                }
            }
        }

        public ReadOnlyObservableCollection<T> this[K key]
        {
            get { return GetReadOnlyCollection(key); }
        }
        private ObservableCollectionWithReadOnly<T> GetCollection(T item)
        {
            K key = keyFunction(item);
            return GetCollection(key);
        }

        private ObservableCollectionWithReadOnly<T> GetCollection(K key)
        {
            ObservableCollectionWithReadOnly<T> collection = null;
            if (this.dictionary.ContainsKey(key))
            {
                collection = dictionary[key];
            }
            else
            {
                collection = new ObservableCollectionWithReadOnly<T>();
                dictionary.Add(key, collection);
            }
            return collection;
        }

        public ReadOnlyObservableCollection<T> GetReadOnlyCollection(K key)
        {
            return GetCollection(key).ReadOnly;
        }

        private void SourceCollectionOnCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    
                case NotifyCollectionChangedAction.Remove:
                    
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Move:

                    if (notifyCollectionChangedEventArgs.NewItems != null)
                    {
                        foreach (T item in notifyCollectionChangedEventArgs.NewItems)
                        {
                            var collection = GetCollection(item);
                            var notify = item as ModelBase;
                            if (notify != null)
                            {
                                notify.PropertyChanged += ItemNotifyPropertyChanged;
                            }
                
                            collection.Add(item);
                        }
                    }

                    if (notifyCollectionChangedEventArgs.OldItems != null)
                    {

                        foreach (T item in notifyCollectionChangedEventArgs.OldItems)
                        {
                            var collection = GetCollection(item);
                            collection.Remove(item);
                        }
                    }
                    break;
                default:
                    Debugger.Break();
                    break;
            }
        }

    }
}