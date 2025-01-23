using weatherApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace weatherApp.ViewModels
{
    public class AstronomyViewModel: INotifyPropertyChanged
    {
        private const string ApiKey = "6f5f0720a13e47d98c9151945251401";
        private const string BaseUrl = "https://api.weatherapi.com/v1/astronomy.json";
        
        private Astronomy _astronomy;
        public Astronomy astronomy
        {
            get => _astronomy;
            set
            {
                _astronomy = value;
                OnPropertyChanged(nameof(astronomy));
            }
        }
        public string City { get; set; }
        public string Date { get; set; } = System.DateTime.Now.ToString("yyyy-MM-dd");

        private string _sunrise;
        public string Sunrise
        {
            get => _sunrise;
            set
            {
                _sunrise = value;
                OnPropertyChanged(nameof(Sunrise));
            }
        }

        private string _sunset;
        public string Sunset
        {
            get => _sunset;
            set
            {
                _sunset = value;
                OnPropertyChanged(nameof(Sunset));
            }
        }

        private string _moonrise;
        public string Moonrise
        {
            get => _moonrise;
            set
            {
                _moonrise = value;
                OnPropertyChanged(nameof(Moonrise));
            }
        }

        private string _moonset;
        public string Moonset
        {
            get => _moonset;
            set
            {
                _moonset = value;
                OnPropertyChanged(nameof(Moonset));
            }
        }

        private string _moonPhase;
        public string MoonPhase
        {
            get => _moonPhase;
            set
            {
                _moonPhase = value;
                OnPropertyChanged(nameof(MoonPhase));
            }
        }

        public ICommand GetAstronomyDataCommand { get; }

        public AstronomyViewModel()
        {
            GetAstronomyDataCommand = new Command(async () => await GetAstronomyDataAsync());
        }

        public async Task GetAstronomyDataAsync()
        {
            if (string.IsNullOrWhiteSpace(City))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please insert a city.", "OK");
                return;
            }

            try
            {
                using HttpClient client = new HttpClient();
                var url = $"{BaseUrl}?key={ApiKey}&q={City}&dt={Date}";
                Console.WriteLine($"URL: {url}");

                var response = await client.GetStringAsync(url);
                Console.WriteLine($"Resposta da API: {response}");

                var json = JsonDocument.Parse(response);

                // data
                var astro = json.RootElement
                    .GetProperty("astronomy")
                    .GetProperty("astro");

                Sunrise = astro.GetProperty("sunrise").GetString() ?? "N/A";
                Sunset = astro.GetProperty("sunset").GetString() ?? "N/A";
                Moonrise = astro.GetProperty("moonrise").GetString() ?? "N/A";
                Moonset = astro.GetProperty("moonset").GetString() ?? "N/A";
                MoonPhase = astro.GetProperty("moon_phase").GetString() ?? "N/A";

                Console.WriteLine($"Sunrise: {Sunrise}, Sunset: {Sunset}, Moonrise: {Moonrise}, Moonset: {Moonset}, MoonPhase: {MoonPhase}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Data not found on JSON: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Erro", "Alguma chave está ausente no JSON retornado.", "OK");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error HTTP: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error HTTP", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}