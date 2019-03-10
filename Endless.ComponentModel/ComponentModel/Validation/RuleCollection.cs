namespace Endless.ComponentModel.Validation
{
#if false
    public sealed class RuleCollection<T> : Collection<Rule<T>>
    {
        #region Public Methods

        /// <summary>
        /// Adds a new <see cref="Rule{T}"/> to this instance.
        /// </summary>
        /// <param name="propertyName">The name of the property the rules applies to.</param>
        /// <param name="error">The error if the object does not satisfy the rule.</param>
        /// <param name="rule">The rule to execute.</param>
        public void Add(T obj, object error, Func<T, bool> rule)
        {
            this.Add(new DelegateRule<T>(obj, error, rule));
        }
        /// <summary>
        /// Applies the <see cref="Rule{T}"/>'s contained in this instance to <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the rules to.</param>
        /// <param name="propertyName">Name of the property we want to apply rules for. <c>null</c>
        /// to apply all rules.</param>
        /// <returns>A collection of errors.</returns>
        public IEnumerable<object> Errors(T obj)
        {
            List<object> errors = new List<object>();

            foreach (IRule rule in this)
            {
                if (!rule.IsValid()==false)
                {
                    errors.Add(rule.Error);
                }
                    
            }

            return errors;
        }

        #endregion
    }
#endif

}