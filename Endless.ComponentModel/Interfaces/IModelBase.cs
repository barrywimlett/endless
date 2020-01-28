using System;
using System.ComponentModel;

namespace Endless
{
    /// <summary>
    /// Interface for ViewModelBase
    /// </summary>
    /// <remarks>
    /// Needed to give a well known and accessible way of calling "NotifyPropertyChanges" as the event cannot be invoked externally
    /// </remarks>

    public interface IModelBase
    {
        event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanges(string propertyName);
        void NotifyPropertyChanges(string propertyName, Type type, object oldValue, object newValue);

        void AddProperty(string name, INotifyProperty property);
    }
}