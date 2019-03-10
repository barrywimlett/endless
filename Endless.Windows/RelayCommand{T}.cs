// -----------------------------------------------------------------------
// <copyright file="RelayCommand{T}.cs" company="Solarvista">
//     Copyright (c) Solarvista. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows.Input;

namespace Endless.Windows
{
    /// <summary>
    /// A command that takes a action to execute.
    /// </summary>
    /// <typeparam name="T">The parameter type</typeparam>
    public class RelayCommand<T> : ICommand where T : class
    {
        /// <summary>
        /// The true predicate instance.
        /// </summary>
        private static readonly Predicate<T> True = _ => true;

        /// <summary>
        /// The allow null
        /// </summary>
        private readonly bool allowNull;

        /// <summary>
        /// The can execute predicate
        /// </summary>
        private readonly Predicate<T> canExecutePredicate;

        /// <summary>
        /// The execute action
        /// </summary>
        private readonly Action<T> executeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="allowNull">if set to <c>true</c> allow null parameters to be passed to action.</param>
        public RelayCommand(Action<T> execute, bool allowNull = false)
            : this(execute, True, allowNull)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <param name="allowNull">if set to <c>true</c> allow null parameters to be passed to action.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute, bool allowNull = false)
        {
            Contract.Requires(execute != null);
            Contract.Requires(canExecute != null);

            this.executeAction = execute;
            this.canExecutePredicate = canExecute;
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
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(T parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.executeAction(parameter);
            }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute(parameter as T);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        void ICommand.Execute(object parameter)
        {
            this.Execute(parameter as T);
        }
    }
}