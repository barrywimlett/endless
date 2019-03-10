using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Endless.Collections
{
    public class CollectionUnion<T>
    {
        ICollection<ICollection<T>> sources;
        ObservableCollection<T> union;

        public CollectionUnion(ICollection<ICollection<T>> sources): this(sources, new ObservableCollection<T>())
        { 
        }

        
        public CollectionUnion(ICollection<ICollection<T>> sources, ObservableCollection<T> union)
        {
            this.sources = sources;
            this.union = union;
            foreach (var collection in sources)
            {
                var collectionAsNotifyCollectionChanged = collection as INotifyCollectionChanged;
                if (collectionAsNotifyCollectionChanged == null)
                {

                }
                else
                {
                    collectionAsNotifyCollectionChanged.CollectionChanged += SourceCollectionChanged;
                }
            }
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // if no source collections contain the item then remove it from the union
            foreach (T item in e.OldItems)
            {
                if (!sources.Any(s => s.Contains(item)) && union.Contains(item))
                {
                    union.Remove(item);
                }
                
            }

            // if the union does already not contain a newItem them add it
            foreach (T item in e.NewItems)
            {
                if (!union.Contains(item))
                {
                    union.Add(item);
                }
            }

        }
    }
}