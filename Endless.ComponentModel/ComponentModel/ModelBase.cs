using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Endless
{
    public abstract class ModelBase : IModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        readonly Dictionary<string, INotifyProperty> properties = new Dictionary<string, INotifyProperty>();
        
        public void AddProperty(string name,INotifyProperty property)
        {
            this.properties.Add(name, property);
        }
        virtual protected internal void NotifyPropertyChanges(string propertyName)
        {

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }


            //PropertyChangedEventHandler handler = this.PropertyChanged;
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        event PropertyChangedEventHandler IModelBase.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        void IModelBase.NotifyPropertyChanges(string propertyName)
        {
            this.NotifyPropertyChanges(propertyName);
        }

        void IModelBase.NotifyPropertyChanges(string propertyName,Type type,object oldValue,object newValue)
        {
            NotifyPropertyChanges(propertyName, type, oldValue, newValue);
        }

        virtual protected internal void NotifyPropertyChanges(string propertyName,Type type,object oldValue,object newValue)
        {

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }


            //PropertyChangedEventHandler handler = this.PropertyChanged;
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new ExtendedPropertyChangedEventArgs(propertyName,type,oldValue,newValue));
            }
        }
    }

    
}