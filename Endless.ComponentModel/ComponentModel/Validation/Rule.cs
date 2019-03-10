using System.Diagnostics.Contracts;

namespace Endless.ComponentModel.Validation
{
    /// <summary>
    /// A named rule containing an error to be used if the rule fails.
    /// </summary>
    /// <typeparam name="T">The type of the object the rule applies to.</typeparam>
    public abstract class Rule<T> : IRule
    {
        //private string propertyName;
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule<T>"/> class.
        /// </summary>
        /// <param name="error">The error message if the rules fails.</param>
        protected Rule(object error)
        {
            Contract.Assume(error!=null);
            
            this.Error = error;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error message if the rules fails.
        /// </summary>
        /// <value>The error message if the rules fails.</value>
        public object Error { get; protected set; }

        #endregion

        #region Apply

        /// <summary>
        /// Applies the rule to the specified object.
        /// </summary>
        /// <param name="obj">The object to apply the rule to.</param>
        /// <returns>
        /// <c>true</c> if the object satisfies the rule, otherwise <c>false</c>.
        /// </returns>

        public abstract bool IsValid(T obj);

        #endregion
    }
}