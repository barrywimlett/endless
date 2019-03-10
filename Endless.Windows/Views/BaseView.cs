using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;

namespace Endless.Windows.Views
{
    public abstract class BaseView<T> : UserControl, IBaseView where T : class, IViewModelBase
    {
        public BaseView()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.Loaded += BaseView_Loaded;
                this.Initialized += BaseView_Initialisied;

                this.DataContextChanged += DataContextChangedHandler;
                
            }
        }

        protected virtual void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            // if someone changes viewmodel make sure we call the Asyncloader to get any static data
            this.ViewModel = e.NewValue as T;
            if (this.ViewModel != null)
            {
                var doNotWait = (this as IBaseView).ViewModel.AsyncLoad();
            }
        }

        /// <summary>
        /// Makes sure we have a viewmodel....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseView_Initialisied(object sender, System.EventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (this.DataContext == null)
                {

                    this.ViewModel = DependencyResolver.Instance.Export<T>();
                    Contract.Assert((this as IBaseView).ViewModel != null, string.Format("{0} does not impliment IBaseView", typeof(T)));
                    this.DataContext = this.ViewModel;
                    
                    var doNotWait = (this as IBaseView).ViewModel.AsyncLoad();

                }
                else if ((this.DataContext as T) == null)
                {
                    this.ViewModel = DependencyResolver.Instance.Export<T>();
                    this.DataContext = this.ViewModel;
                    
                    var doNotWait = (this as IBaseView).ViewModel.AsyncLoad();
                }
                else
                {
#if DEBUG
                                Debugger.Break();
#endif
                }

            }
        }

        public T ViewModel { get; protected set; }

        IViewModelBase IBaseView.ViewModel
        {
            get { return this.ViewModel as IViewModelBase; }
        }

        
        private void BaseView_Loaded(object sender, RoutedEventArgs e)
        {
        }


    }
}