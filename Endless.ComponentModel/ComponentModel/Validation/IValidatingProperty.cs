using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Endless.ComponentModel.Validation
{
    public interface IValidatingProperty : INotifyProperty
    {
        IList<IRule> Rules { get; }
        ObservableCollection<object> Errors { get; }

        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        bool Validate();
    
    }
}