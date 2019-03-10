using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Endless.ComponentModel.Validation
{
    [Obsolete("Use a standard NotifyProperty, the validation code is reponsiblity of the view model")]
    public class ValidatingNotifyProperty<T> : NotifyProperty<T>
    {
        

        public ValidatingNotifyProperty(ValidatingViewModelBase localViewModel, string name)
            : base(localViewModel, name)
        {
            localViewModel.AddProperty(this);
        }

        public ValidatingNotifyProperty(ValidatingViewModelBase localViewModel, string name, IEnumerable<Rule<T>> rules)
            : base(localViewModel, name)
        {
            foreach (var rule in rules)
            {
                AddRule(rule);
            }
            
        }

        [Obsolete("Add a binding and mapping via the viewmodel")]
        public void AddRule(Rule<T> rule)
        {
            ValidatingViewModelBase localViewModel = this.ViewModel as ValidatingViewModelBase;
            Contract.Assume(localViewModel !=null);
            PropertyMapping mapping = localViewModel.Mappings[this.Name];
            mapping.AddBinding(new NotifyPropertyBindingBase<T>(this, rule));
            mapping.Validate();
        }

        
        
    }
}