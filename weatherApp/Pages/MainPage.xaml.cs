using weatherApp.Services;
using weatherApp.ViewModels;

namespace weatherApp.Pages;

public partial class MainPage : ContentPage
{
    private readonly APIService _apiService;

    public MainPage(APIService apiService)
    {
        InitializeComponent();
 
    }
 
         private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_apiService));
    }
}
     

 
 