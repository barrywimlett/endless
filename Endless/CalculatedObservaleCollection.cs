using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Endless
{
    public class CalculatedObservaleCollection<T, V> : ObservableCollection<T> where V : class,IModelBase
    {
        Func<V, IList<T>> calculation;
        private IModelBase viewModel;
        
        public CalculatedObservaleCollection(IViewModelBase localViewModel, string name, Func<V, IList<T>> calculation)
            
        {
            this.viewModel = localViewModel;
            Contract.Assert(calculation != null);

            //register for view model changes so we can re-run the calculation
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
            this.calculation = calculation;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            V vm = this.viewModel as V;
            Contract.Assume(vm != null);

            IList<T> newValues = calculation(vm);
            foreach (var newValue in newValues)
            {
                if (this.Contains(newValue))
                {
                    this.Add(newValue);
                }
            }

            foreach (var item in this.Items)
            {
                if (newValues.Contains(item))
                {
                    this.Remove(item);
                }
            }
        }

        
    }
}