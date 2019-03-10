namespace Endless
{
    /// <summary>
    /// Parses Planner or Business manager addin command lines and stores the arguments.
    /// </summary>
    public interface IArgumentProvider
    {
        /// <summary>
        /// Configures the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        void Parse(string[] arguments);

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        ArgumentContext Context { get; }
    }
}
