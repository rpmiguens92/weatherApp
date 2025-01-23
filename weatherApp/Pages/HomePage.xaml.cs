using weatherApp.Services;
using weatherApp.ViewModels;

namespace weatherApp.Pages;

public partial class HomePage : ContentPage
{
	public HomePage(APIService apiService)
	{
		InitializeComponent();
        BindingContext = new HomeViewModel(apiService, this.Navigation);
    }
    private async void OnLinkedInTapped(object sender, EventArgs e)
    {
        var linkedInUrl = "https://www.linkedin.com/in/rpmiguens";
        try
        {
            await Browser.Default.OpenAsync(linkedInUrl, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to open LinkedIn: {ex.Message}", "OK");
        }
    }
    private async void OnLinkTapped(object sender, EventArgs e)
    {
        var linkedInUrl = "https://www.cinel.pt/appv2";
        try
        {
            await Browser.Default.OpenAsync(linkedInUrl, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to open CINEL: {ex.Message}", "OK");
        }
    }
}