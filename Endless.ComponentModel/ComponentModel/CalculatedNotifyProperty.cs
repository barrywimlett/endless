using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Endless
{
    /// <summary>
    /// Allows a field to be calculated based on contents of the viewModel
    /// </summary>
    /// <typeparam name="T">type of the field</typeparam>
    /// <typeparam name="V">type of the viewmodel, which should allow access to the peoperties used to execute the calculation</typeparam>
    public class CalculatedNotifyProperty<T, V> : NotifyProperty<T> where V : class
    {
        Func<V, T> calculation;
        private Collection<string> triggerProperties;
        private bool needsCalc = true;

        public CalculatedNotifyProperty(IModelBase localViewModel, string name, Func<V, T> calculation,string[] triggerProperties)
            : this(localViewModel, name,calculation)
        {
            this.triggerProperties = new Collection<string>(triggerProperties);
        }

        public CalculatedNotifyProperty(IModelBase localViewModel, string name, Func<V, T> calculation) :base(localViewModel,name)
        {
            Contract.Assert(calculation!= null);

            //register for view model changes so we can re-run the calculation
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            this.calculation = calculation;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (triggerProperties == null)
            {
                Recalculate();
            }
            else if (triggerProperties.Contains(e.PropertyName))
            {
                Recalculate();
            }
        }

        public void Recalculate()
        {
            V vm = this.ViewModel as V;
            Contract.Assume(vm != null);
            T newValue = calculation(vm);
            this.Value = newValue;
        }

        public override T Value {
            get
            {
                if (needsCalc)
                {
                    needsCalc = false;
                    this.Recalculate();
                }
                return base.Value;
            }
            set { base.Value = value; }
        }
    }
}