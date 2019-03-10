// -----------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="Solarvista">
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
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The no-op disabled instance
        /// </summary>
        private static readonly ICommand NoopDisabledInstance = new RelayCommand(() => { }, () => false);

        /// <summary>
        /// The no-op instance
        /// </summary>
        private static readonly ICommand NoopInstance = new RelayCommand(() => { }, () => true);

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
        private readonly Action executeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        [DebuggerStepThrough]
        public RelayCommand(Action execute)
            : this(execute, True)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        [DebuggerStepThrough]
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            Contract.Requires(execute != null);
            Contract.Requires(canExecute != null);

            this.executeAction = execute;
            this.canExecutePredicate = canExecute;
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
        /// Gets the no-op command.
        /// Does nothing on invoke, always enabled.
        /// </summary>
        /// <value>
        /// The no-op command.
        /// </value>
        public static ICommand Noop
        {
            get { return NoopInstance; }
        }

        /// <summary>
        /// Gets the disabled no-op command.
        /// Does nothing on invoke, always disabled.
        /// </summary>
        /// <value>
        /// The no-op disabled.
        /// </value>
        public static ICommand NoopDisabled
        {
            get { return NoopDisabledInstance; }
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
            return this.canExecutePredicate();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        [DebuggerStepThrough]
        public void Execute()
        {
            if (this.CanExecute())
            {
                this.executeAction();
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
            this.Execute();
        }
    }
}