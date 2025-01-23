using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using weatherApp.Models;
using System.Windows.Input;
using weatherApp.Pages;
 

namespace weatherApp.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private WeatherData _weather;
        private double _latitude;
        private double _longitude;
        private string _airQuality;

        public WeatherData Weather
        {
            get => _weather;
            set
            {
                _weather = value;
                OnPropertyChanged(nameof(Weather));
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }
        public string AirQuality
        {
            get => _airQuality;
            set
            {
                _airQuality = value;
                OnPropertyChanged(nameof(AirQuality));
            }
        }

  
        public ICommand GetWeatherCommand { get; }
        public ICommand GetAirPollutionCommand { get; }
        public WeatherViewModel()
        {
            GetWeatherCommand = new Command(async () => await GetWeatherDataAsync());
            GetAirPollutionCommand = new Command(async () => await GetAirPollutionAsync());
        }

        public async Task GetWeatherDataAsync()
        {
            string apiKey = "8647d703a6fa1f83ca56ea71ef1c264d";//key
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={City}&appid={apiKey}&units=metric";

            using HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var json = JsonSerializer.Deserialize<JsonElement>(response);

            _latitude = json.GetProperty("coord").GetProperty("lat").GetDouble();
            _longitude = json.GetProperty("coord").GetProperty("lon").GetDouble();

            Weather = new WeatherData
            {
                City = json.GetProperty("name").GetString(),
                Temperature = json.GetProperty("main").GetProperty("temp").GetDouble(),
                Description = json.GetProperty("weather")[0].GetProperty("description").GetString(),
                Humidity = json.GetProperty("main").GetProperty("humidity").GetDouble(),
                WindSpeed = json.GetProperty("wind").GetProperty("speed").GetDouble()
            };
        }
        public async Task GetAirPollutionAsync()
        {
            if (_latitude == 0 || _longitude == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please insert a city!", "OK");
                return;
            }
            try
            {
                string apiKey = "8647d703a6fa1f83ca56ea71ef1c264d";
                string url = $"https://api.openweathermap.org/data/2.5/air_pollution?lat={_latitude}&lon={_longitude}&appid={apiKey}";

                using HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(url);
                var json = JsonSerializer.Deserialize<JsonElement>(response);

                int airQualityIndex = json.GetProperty("list")[0].GetProperty("main").GetProperty("aqi").GetInt32();
                string airQuality = airQualityIndex switch
                {
                    1 => "Good",
                    2 => "Fair",
                    3 => "Moderate",
                    4 => "Poor",
                    5 => "Very Poor",
                    _ => "Unknown"
                };

                //air quality
                await Application.Current.MainPage.Navigation.PushAsync(new AirPollutionPage(airQuality));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Can´t find a city:{ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}