using System.Diagnostics;
using System.Diagnostics.Contracts;
using System;

namespace Endless
{
    /// <summary>
    /// Basic notify property takes a viewModel which impliments IViewModelBase 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotifyProperty<T> :INotifyProperty
    {
        private T value;

        //public event EventHandler<PropertyChangedEventArgs> PropertyChanged; 
        public event EventHandler<ExtendedPropertyChangedEventArgs> NotifyPropertyChanged;

        public bool HasChanged { get; protected set; }

        virtual public T Value
        {
            get { return this.value; }
            set {
                if (this.value == null)
                {
                    if (value != null)
                    {
                        T oldValue = this.value;
                        this.value = value;
                        OnPropertyChanged(oldValue, value);
                    }
                }
                else if (this.Value.Equals(value))
                {
                }
                else
                {
                    T oldValue = this.value;
                    this.value = value;
                    OnPropertyChanged(oldValue, value);
                }
            }
        }

        virtual protected void OnPropertyChanged(T oldValue, T newValue)
        {
            Debug.WriteLine("{0}:{1} changed from {2} to {3}",this.ViewModel.GetType(), this.Name,oldValue,newValue);
            var handler = this.NotifyPropertyChanged;
            HasChanged = true;

            this.ViewModel.NotifyPropertyChanges(this.Name);
            this.ViewModel.NotifyPropertyChanges(this.Name, typeof(T), oldValue, newValue);

            if (handler != null)
            {
                //handler.Invoke(this, new PropertyChangedEventArgs(this.Name));
                handler.Invoke(this, new ExtendedPropertyChangedEventArgs(this.Name,typeof(T),oldValue,newValue));
            }
            
        }

        public string Name { get; protected set; }
        public IModelBase ViewModel { get; protected set; }

        public NotifyProperty(IModelBase localViewModel, string name)
        {
            Contract.Assert(name != null);
            Contract.Assert(localViewModel != null);
            this.ViewModel = localViewModel;
            this.Name = name;
        }

        public NotifyProperty(IModelBase localViewModel, string name,T initialValue) : this(localViewModel, name)
        {
            this.Value = initialValue;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", this.Name, this.Value);
        }
    }
}