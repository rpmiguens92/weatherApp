 
using System.Windows.Input;
using System.Net.Http.Json;
using System.Text.Json;
using weatherApp.Services;
using weatherApp.Pages;
using weatherApp.Models;
 
using Microsoft.Maui.Controls;
using System.ComponentModel;
using weatherApp.ViewModels;


namespace weatherApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly APIService _apiService;
     

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }
       
        public ICommand NavigateToHomeCommand { get; }
 
        public LoginViewModel(APIService apiService )
        {
            _apiService = apiService;
            

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://liganos.azurewebsites.net/")
            };

            LoginCommand = new Command(async () => await ExecuteLoginCommand());
         
            NavigateToHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));


        }
        private async Task ExecuteLoginCommand()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your email and password", "OK");
                return;
            }

            var loginDto = new Login
            {
                Username = Email,
                Password = Password
            };

            var response = await _httpClient.PostAsJsonAsync("Api/ApiAccounts/CreateToken", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);

                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        Preferences.Set("AuthToken", loginResponse.Token);
                        Preferences.Set("TokenExpiration", loginResponse.Expiration);
                        Preferences.Set("UserId", loginResponse.UserId);
                        Preferences.Set("UserName", loginResponse.UserName);

                        await Application.Current.MainPage.DisplayAlert("Success", "Login successful! Welcome.", "OK");

                        await Shell.Current.GoToAsync("//HomePage");


                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to retrieve token from response.", "OK");
                    }
                }
                catch (JsonException jsonEx)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"JSON Deserialization Error: {jsonEx.Message}", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "OK");
            }
        }
    }
}
