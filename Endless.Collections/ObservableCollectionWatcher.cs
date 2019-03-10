using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Endless.Collections
{
    public class ObservableCollectionWatcher<T> : INotifyPropertyChanged
    {
        public INotifyCollectionChanged Collection { get; protected set; }

        
        public ObservableCollectionWatcher(INotifyCollectionChanged collection)
        {
            this.Collection = collection;
            collection.CollectionChanged += CollectionChanged;
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void CollectionItemPropertyChanged(object sender,
            PropertyChangedEventArgs notifyPropertyChangedEventArgs)
        {
            PropertyChanged?.Invoke(sender, notifyPropertyChangedEventArgs);
        }

        
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Move:

                    if (notifyCollectionChangedEventArgs.OldItems != null)
                    {

                        foreach (T item in notifyCollectionChangedEventArgs.OldItems)
                        {

                            var notify = item as INotifyPropertyChanged;
                            if (notify != null)
                            {
                                notify.PropertyChanged -= CollectionItemPropertyChanged;
                            }
                        }
                    }

                    if (notifyCollectionChangedEventArgs.NewItems != null)
                    {
                        foreach (T item in notifyCollectionChangedEventArgs.NewItems)
                        {
                            
                            var notify = item as INotifyPropertyChanged;
                            if (notify != null)
                            {
                                notify.PropertyChanged += CollectionItemPropertyChanged;
                            }
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