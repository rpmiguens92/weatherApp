using weatherApp.Services;
using weatherApp.ViewModels;
 

namespace weatherApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly APIService _apiService;

        public LoginPage(APIService apiService)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(apiService);
        }

    }
}
 