namespace Endless
{
    /// <summary>
    /// View model provider interface
    /// </summary>
    public interface IViewModelProvider
    {
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <typeparam name="T">The view model type</typeparam>
        /// <returns>The view model instance</returns>
        T GetViewModel<T>();
    }
}
