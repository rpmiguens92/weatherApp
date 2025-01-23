using System.Text.Json;
using weatherApp.Models;
using weatherApp.ViewModels;

namespace weatherApp.Pages;

public partial class WeatherPage : ContentPage
{
    private WeatherViewModel viewModel;

    public WeatherPage()
    {
        InitializeComponent();
        viewModel = new WeatherViewModel();
        BindingContext = viewModel;
    }

 
}