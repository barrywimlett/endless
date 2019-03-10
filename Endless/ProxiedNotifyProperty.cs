using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Endless
{
    /// <summary>
    /// Allows one viewmodel to present a calculation based on contents of another viewmodel
    /// use primaryily to expose members of a nested viewmodel
    /// </summary>
    /// <typeparam name="T">type of the field</typeparam>
    /// <typeparam name="R">type of the remote viewmodel containing the properties we want to expose/base calculation upon</typeparam>
    public class ProxiedNotifyProperty<T, R> : NotifyProperty<T> where R : class 
    {
        Func<R, T> getter;
        Action<T,R> setter;
        private IViewModelBase remoteViewModel;

        public ProxiedNotifyProperty(IViewModelBase localViewModel, string name, IViewModelBase remoteViewModel, Func<R, T> get,Action<T,R> set )
            : base(localViewModel, name)
        {
            Contract.Assert(remoteViewModel!= null);
            Contract.Assert(get != null);
            Contract.Assert(set!= null);

            this.remoteViewModel = remoteViewModel;
            remoteViewModel.PropertyChanged += RemoteViewModel_PropertyChanged;
            this.getter = get;
            this.setter = set;

        }

        
        private void RemoteViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            R vm = this.remoteViewModel as R;
            Contract.Assume(vm!= null);
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