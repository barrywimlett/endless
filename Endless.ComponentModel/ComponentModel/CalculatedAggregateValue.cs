using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Endless.Collections;

namespace Endless
{
    public class CalculatedAggregateValue<TCollection, TValue> : NotifyProperty<TValue> 
    {
        Func<ICollection<TCollection>,TValue> calculation;
        
        private Collection<string> triggerProperties;
        private bool needsCalc = true;
        protected readonly ICollection<TCollection> Collection;

        private ObservableCollectionWatcher<TCollection> watcher;

        public CalculatedAggregateValue(IModelBase localViewModel, string name, ICollection<TCollection> collection, Func<ICollection<TCollection>,TValue> calculation, string[] triggerProperties)
            : this(localViewModel, name, collection,calculation)
        {
            this.triggerProperties = new Collection<string>(triggerProperties);
        }

        public CalculatedAggregateValue(IModelBase localViewModel, string name, ICollection<TCollection> collection, Func<ICollection<TCollection>, TValue> calculation) : base(localViewModel, name)
        {
            Contract.Assert(calculation != null);
            this.Collection = collection;

            INotifyCollectionChanged collectionAsNotifyCollectionChanged = collection as INotifyCollectionChanged;
            if (collectionAsNotifyCollectionChanged != null)
            {
                watcher =new ObservableCollectionWatcher<TCollection>(collection as INotifyCollectionChanged);

                collectionAsNotifyCollectionChanged.CollectionChanged += CollectionChanged;
                watcher.PropertyChanged += CollectionItemPropertyChanged;
            }

            this.calculation = calculation;
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Recalculate();
        }


        private void CollectionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (triggerProperties == null || triggerProperties.Contains(e.PropertyName))
            {
                Recalculate();
            }
        }


        public void Recalculate()
        {
            TValue newValue = calculation(this.Collection);
            this.Value = newValue;
        }

        public override TValue Value
        {
            get
            {
                if (needsCalc)
                {
                    needsCalc = false;
                    this.Recalculate();
                }
                return base.Value;
            }
            set { base.Value = value; }
        }
    }
}