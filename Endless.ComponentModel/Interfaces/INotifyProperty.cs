using System;

namespace Endless
{
    public interface INotifyProperty
    {
        string Name { get; }
        IModelBase ViewModel { get; }
        event EventHandler<ExtendedPropertyChangedEventArgs> NotifyPropertyChanged;
        bool HasChanged { get; }
    }
}