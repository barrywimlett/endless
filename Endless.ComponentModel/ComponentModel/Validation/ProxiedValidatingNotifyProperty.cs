using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace Endless.ComponentModel.Validation
{
    public class ProxiedValidatingNotifyProperty<T, R> : ValidatingNotifyProperty<T> where R : class
    {
        Func<R, T> getter;
        Action<T, R> setter;
        private IViewModelBase remoteViewModel;

        public ProxiedValidatingNotifyProperty(ValidatingViewModelBase localViewModel, string name, IViewModelBase remoteViewModel, Func<R, T> get, Action<T, R> set)
            : base(localViewModel, name)
        {
            Contract.Assert(remoteViewModel != null);
            Contract.Assert(get != null);
            Contract.Assert(set != null);

            this.remoteViewModel = remoteViewModel;
            remoteViewModel.PropertyChanged += RemoteViewModel_PropertyChanged;
            this.getter = get;
            this.setter = set;

        }

        private void RemoteViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            R vm = this.remoteViewModel as R;
            Contract.Assume(vm != null);
            T newValue = getter(vm);
            this.Value = newValue;

        }

        protected override void OnPropertyChanged(T oldValue, T newValue)
        {
            base.OnPropertyChanged(oldValue, newValue);

            try
            {
                R remote = this.remoteViewModel as R;
                Contract.Assume(remote != null);
                this.setter(newValue, remote);
            }
            catch (Exception )
            {
                //todo what the hell to do here?
            }

        }

    }
}