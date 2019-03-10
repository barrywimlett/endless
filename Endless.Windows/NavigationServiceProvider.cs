using System.Composition;
using System.Diagnostics.Contracts;
using System.Windows.Navigation;

namespace Endless.Windows
{
    /// <summary>
    /// NavigationServiceProvider
    /// </summary>
    [Shared]
    [Export(typeof(INavigationServiceProvider))]
    public sealed class NavigationServiceProvider : INavigationServiceProvider
    {
        /// <summary>
        /// The Navigation Service
        /// </summary>
        private NavigationService navigationService = null;

        /// <summary>
        /// Configures the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        public void Configure(NavigationService service)
        {
            Contract.Requires(service != null);

            this.navigationService = service;
        }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>
        /// The navigation service.
        /// </value>
        public NavigationService NavigationService 
        { 
            get
            {
                return this.navigationService;
            }
        }
    }
}
