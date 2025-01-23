using weatherApp.ViewModels;

namespace weatherApp.Pages;

public partial class AstronomyPage : ContentPage
{
	public AstronomyPage()
	{
		InitializeComponent();
		BindingContext = new AstronomyViewModel();
	}
}