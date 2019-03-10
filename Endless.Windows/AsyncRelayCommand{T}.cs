// -----------------------------------------------------------------------
// <copyright file="AsyncRelayCommand{T}.cs" company="Solarvista">
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
    /// <typeparam name="T">The parameter type</typeparam>
    public class AsyncRelayCommand<T> : ICommand where T : class
    {
        /// <summary>
        /// The true predicate instance.
        /// </summary>
        private static readonly Predicate<T> True = _ => true;

        /// <summary>
        /// The can execute predicate
        /// </summary>
        private readonly Predicate<T> canExecutePredicate;

        /// <summary>
        /// The execute action
        /// </summary>
        private readonly Func<T, Task> executeAction;

        /// <summary>
        /// The allow a null parameter.
        /// </summary>
        private bool allowNull;

        /// <summary>
        /// The exception handler
        /// </summary>
        private Action<Exception> exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="allowNull">if set to <c>true</c> allow null parameters to be passed to action.</param>
        [DebuggerStepThrough]
        public AsyncRelayCommand(Func<T, Task> execute, bool allowNull = false)
            : this(execute, True, null, allowNull)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="allowNull">if set to <c>true</c> allow null parameters to be passed to action.</param>
        [DebuggerStepThrough]
        public AsyncRelayCommand(Func<T, Task> execute, Predicate<T> canExecute, bool allowNull = false)
            : this(execute, canExecute, null, allowNull)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <param name="allowNull">if set to <c>true</c> allow null parameters to be passed to action.</param>
        [DebuggerStepThrough]
        public AsyncRelayCommand(Func<T, Task> execute, Predicate<T> canExecute, Action<Exception> exceptionHandler, bool allowNull = false)
        {
            Contract.Requires(execute != null);
            Contract.Requires(canExecute != null);

            this.executeAction = execute;
            this.canExecutePredicate = canExecute;
            this.exceptionHandler = exceptionHandler;
            this.allowNull = allowNull;
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
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public bool CanExecute(T parameter)
        {
            return parameter != null ?
                this.canExecutePredicate(parameter) :
                this.allowNull && this.canExecutePredicate(null);
        }

        /// <summary>
        /// Executes the method to be called when the command is invoked.
        /// Returns a task, useful for executing the command outside of WPF.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>A task</returns>
        [DebuggerStepThrough]
        public async Task ExecuteAsync(T parameter)
        {
            if (this.CanExecute(parameter))
            {
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
            return this.CanExecute(parameter as T);
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
        /// <param name="parameter">The parameter.</param>
        /// <remarks>
        /// There issues with explicit interfaces being async void.
        /// Just make the interface method wrap an internal async void method.
        /// </remarks>
        [DebuggerStepThrough]
        private async void ExecuteAsyncVoidInternal(object parameter)
        {
            await this.ExecuteAsync(parameter as T);
        }
    }
}