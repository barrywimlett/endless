// -----------------------------------------------------------------------
// <copyright file="AsyncRelayCommand.cs" company="Solarvista">
//     Copyright (c) Solarvista. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
    public class AsyncRelayCommand : ICommand
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
        private static readonly Func<bool> True = () => true;

        /// <summary>
        /// The can execute predicate
        /// </summary>
        private readonly Func<bool> canExecutePredicate;

        /// <summary>
        /// The execute action
        /// </summary>
        private readonly Func<Task> executeAction;

        /// <summary>
        /// The exception handler
        /// </summary>
        private Action<Exception> exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        [DebuggerStepThrough]
        public AsyncRelayCommand(Func<Task> execute)
            : this(execute, True, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        [DebuggerStepThrough]
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute)
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
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute, Action<Exception> exceptionHandler)
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
        public bool CanExecute()
        {
            return IsExecuting==false && this.canExecutePredicate();
        }

        /// <summary>
        /// Executes asynchronous command.
        /// </summary>
        /// <returns>
        /// A task.
        /// </returns>
        [DebuggerStepThrough]
        public async Task ExecuteAsync()
        {
            if (this.CanExecute()==false)
            { 
                Debugger.Break();
            }
            else {

            
                IsExecuting = true;

                WithExceptionHandling.Do("ICommand.ExecuteAsync", this.executeAction,exceptionHandler);

                /*
                try
                {
                    await this.executeAction();
                }
                catch (AggregateException e)
                {
                }
                catch (Exception e)
                {
                    if (this.exceptionHandler != null)
                    {
                        this.exceptionHandler(e);
                    }
                }
                 */
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
            return this.CanExecute();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        [DebuggerStepThrough]
        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsyncVoidInternal();
        }

        /// <summary>
        /// Executes the asynchronous void internal.
        /// </summary>
        /// <remarks>
        /// There issues with explicit interfaces being async void.
        /// Just make the interface method wrap an internal async void method.
        /// </remarks>
        [DebuggerStepThrough]
        private async void ExecuteAsyncVoidInternal()
        {
            
            await this.ExecuteAsync();
            
        }
    }
}