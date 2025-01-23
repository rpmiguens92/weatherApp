using weatherApp.Services;
using weatherApp.Pages;
using weatherApp.Models;

namespace weatherApp
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();

            Navigated += OnNavigated;


            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(AstronomyPage), typeof(AstronomyPage));
          

        }
        private void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            var currentPage = e.Current?.Location.OriginalString;

            if (string.IsNullOrEmpty(currentPage))
                return;
            if (currentPage.Contains("HomePage") || currentPage.Contains("AstronomyPage") || currentPage.Contains("CuriositiesPage") || currentPage.Contains("WeatherPage") )
            {
                FlyoutBehavior = FlyoutBehavior.Flyout;
            }
            else
            {
                FlyoutBehavior = FlyoutBehavior.Disabled;
            }

        }
    }
}
