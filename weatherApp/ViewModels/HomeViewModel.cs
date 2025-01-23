using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherApp.Services;
using weatherApp.Pages;
using System.ComponentModel;
using System.Windows.Input;

namespace weatherApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly APIService _apiService;
        private readonly INavigation _navigation;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand NavigateToWeatherCommand { get; }
        public ICommand NavigateToAstronomyCommand { get; }
        public ICommand NavigateToCuriositiesCommand { get; }
 

       
 
        public HomeViewModel(APIService apiService, INavigation navigation)
        {
            _apiService = apiService;
            _navigation = navigation;
 
            NavigateToWeatherCommand = new Command(async () => await NavigateToWeather());
            NavigateToAstronomyCommand = new Command(async () => await NavigateToAstronomy());
            NavigateToCuriositiesCommand = new Command(async () => await NavigateToCuriosities());
 
        }


        private async Task NavigateToWeather()
        {
            await _navigation.PushAsync(new WeatherPage());
        }

        private async Task NavigateToAstronomy()
        {
            await _navigation.PushAsync(new AstronomyPage());
        }

        private async Task NavigateToCuriosities()
        {
            await _navigation.PushAsync(new CuriositiesPage());
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
