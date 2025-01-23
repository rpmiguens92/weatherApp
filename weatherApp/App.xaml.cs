using weatherApp.Services;
using weatherApp.Pages;
using CommunityToolkit.Maui;

namespace weatherApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            SetMainPage();

        }

        private void SetMainPage()
        {
            
            var accessToken = Preferences.Get("AuthToken", string.Empty);

            if (string.IsNullOrEmpty(accessToken))
            {
                
                MainPage = new NavigationPage(_serviceProvider.GetService<MainPage>());
            }
            else
            {
                
                MainPage = _serviceProvider.GetService<AppShell>();
            }

            
        }
        public void NavigateToHomePage()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Shell.Current.GoToAsync("//HomePage");
            });
        }

    }
}
