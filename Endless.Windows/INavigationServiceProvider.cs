namespace Endless.Windows
{
    public interface INavigationServiceProvider
    {
        void Configure(System.Windows.Navigation.NavigationService service);
        System.Windows.Navigation.NavigationService NavigationService { get; }
    }
}
