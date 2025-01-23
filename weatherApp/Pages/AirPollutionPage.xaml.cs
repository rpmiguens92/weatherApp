using weatherApp.ViewModels;

namespace weatherApp.Pages;

public partial class AirPollutionPage : ContentPage
{
    public AirPollutionPage(string airQuality)
    {
		InitializeComponent();
        BindingContext = new AirPollutionViewModel(airQuality);
    }
    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///HomePage"); 
    }
}