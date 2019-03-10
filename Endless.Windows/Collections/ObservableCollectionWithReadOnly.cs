using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;

namespace Endless.Windows
{
    public interface IDispatcherObject
    {
        Dispatcher Dispatcher { get; }
    }

    public interface IObservableCollection<T>: ICollection<T>, INotifyCollectionChanged
    {
        
    }

 
    public class ObservableCollectionWithReadOnly<T> : DispatcherObject, IObservableCollection<T>, IReadOnlyCollection<T>
    {
        private readonly ReadOnlyObservableCollection<T> readOnly;
        private System.Windows.Threading.Dispatcher _dispatcher;

        private ObservableCollection<T> innerCollection=new ObservableCollection<T>();
        public ObservableCollectionWithReadOnly()
        {
            _dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            readOnly = new ReadOnlyObservableCollection<T>(innerCollection);
            innerCollection.CollectionChanged += OnInnerCollectionChanged;

        }

        private void OnInnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
            OnCollectionChanged(e);
            //this.CollectionChanged?.Invoke(sender, e);
        }

        public void Add(T item)
        {
            ExecuteOnUI(() =>
            {
                try
                {
                    innerCollection.Add(item);
                }
                catch (AggregateException ex)
                {
                    Debug.Assert(false);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false);
                }
            });
            ;
        }

        protected void ExecuteOnUI(Action action)
        {
            lock (this)
            {
                if (_dispatcher.CheckAccess() == false)
                {

                    try
                    {
                        _dispatcher.Invoke(action);
                    }
                    catch (AggregateException ex)
                    {
                        Debug.Assert(false);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false);
                    }
                }
                else
                {
                    action();
                }
            }
        }

        protected T ExecuteOnUI<T>(Func<T> action)
        {
            T result;
            
            if (_dispatcher.CheckAccess() == false)
            {
                result = _dispatcher.Invoke(action);
            }
            else
            {
                result=action();
            }

            return result;
        }

        public void Add(IEnumerable<T> items)
        {
            lock (this)
            {
                ExecuteOnUI(() =>
                {
                    foreach (T item in items.ToList())
                    {
                        innerCollection.Add(item);
                    }
                });
                ;
            }
        }

        
        public void Clear()
        {
            lock (this)
            {
                ExecuteOnUI(() => { innerCollection.Clear(); });
            }
        }

        
        public ReadOnlyObservableCollection<T> ReadOnly
        {
            get { return readOnly; }
        }



        public bool Contains(T item)
        {
            return innerCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            innerCollection.CopyTo(array, arrayIndex);
        }

        
        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return ExecuteOnUI<bool>(() =>

                {
                    try
                    {
                        return innerCollection.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false);
                        return false;
                    }
                }
            );
        }

        public int Count => innerCollection.Count;


        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // Be nice - use BlockReentrancy like MSDN said
            lock (innerCollection)
            {
                var eventHandler = this.CollectionChanged;

                if (eventHandler != null)
                {
                    Delegate[] delegates = eventHandler.GetInvocationList();
                    // Walk thru invocation list
                    foreach (NotifyCollectionChangedEventHandler handler in delegates)
                    {
                        var dispatcherObject = handler.Target as DispatcherObject;

                        
                        // If the subscriber is a DispatcherObject and different thread
                        if (dispatcherObject != null && dispatcherObject.CheckAccess() == false)
                            
                        {
                            // Invoke handler in the target dispatcher's thread
                            dispatcherObject.Dispatcher.Invoke(DispatcherPriority.Normal,
                                handler, this, e);
                        }
                        else 
                        {
                            if (this._dispatcher.CheckAccess() == false)
                            {
                                this.Dispatcher.Invoke(handler, this, e);
                            }
                            // Execute handler as is
                            else
                            {
                                handler(this, e);
                            }
                        }
                    }
                }
            }
        }

        public Dispatcher Dispatcher => _dispatcher;
        public IEnumerator<T> GetEnumerator()
        {
            return innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerCollection.GetEnumerator();
        }
    }
}