namespace Endless
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// ViewModel Provider
    /// </summary>
    public class ViewModelProvider : IViewModelProvider
    {
        /// <summary>
        /// The dependency resolver
        /// </summary>
        private readonly IDependencyResolver dependencyResolver = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelProvider"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public ViewModelProvider(IDependencyResolver dependencyResolver)
        {
            Contract.Requires(dependencyResolver != null);

            this.dependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ViewModelProvider"/> class from being created.
        /// </summary>
        private ViewModelProvider()
        {
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <typeparam name="T">The view model type</typeparam>
        /// <returns>
        /// The view model instance
        /// </returns>
        public T GetViewModel<T>()
        {
            return this.dependencyResolver.Export<T>();
        }
    }
}
