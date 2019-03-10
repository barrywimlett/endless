using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Endless.Windows
{
    /// <summary>
    /// A command that takes a action to execute.
    /// </summary>
    public class AsyncParamRelayCommand : ICommand
    {
        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            set
            {
                _isExecuting = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// The true predicate instance.
        /// </summary>
        protected static readonly Func<object, bool> ReturnsTrue = (object o) => true;

        /// <summary>
        /// The can execute predicate
        /// </summary>
        protected readonly Func<object,bool> canExecutePredicate;

        /// <summary>
        /// The execute action
        /// </summary>
        protected readonly Func<object, Task> executeAction;

        /// <summary>
        /// The exception handler
        /// </summary>
        protected Action<Exception> exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        [DebuggerStepThrough]
        public AsyncParamRelayCommand(Func<object,Task> execute)
            : this(execute, ReturnsTrue, null)
        {
        }

      


        [DebuggerStepThrough]
        protected AsyncParamRelayCommand ()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        [DebuggerStepThrough]
        public AsyncParamRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute)
            : this(execute, canExecute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        [DebuggerStepThrough]
        public AsyncParamRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute, Action<Exception> exceptionHandler)
        {
            Contract.Requires(execute != null);
            Contract.Requires(canExecute != null);

            this.executeAction = execute;
            this.canExecutePredicate = canExecute;
            this.exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return IsExecuting == false && this.canExecutePredicate(parameter);
        }

        /// <summary>
        /// Executes asynchronous command.
        /// </summary>
        /// <returns>
        /// A task.
        /// </returns>
        [DebuggerStepThrough]
        public async Task ExecuteAsync(object parameter)
        {
            if (this.CanExecute(parameter) == false)
            {
                Debugger.Break();
            }
            else
            {


                IsExecuting = true;

                try
                {
                    await this.executeAction(parameter);
                }
                catch (Exception e)
                {
                    if (this.exceptionHandler != null)
                    {
                        this.exceptionHandler(e);
                    }
                }
                IsExecuting = false;
            }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        [DebuggerStepThrough]
        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsyncVoidInternal(parameter);
        }

        /// <summary>
        /// Executes the asynchronous void internal.
        /// </summary>
        /// <remarks>
        /// There issues with explicit interfaces being async void.
        /// Just make the interface method wrap an internal async void method.
        /// </remarks>
        [DebuggerStepThrough]
        protected async void ExecuteAsyncVoidInternal(object parameter)
        {

            await this.ExecuteAsync(parameter);

        }
    }
}