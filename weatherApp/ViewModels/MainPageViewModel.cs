using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherApp.Pages;
using weatherApp.Services;
using System.Windows.Input;

namespace weatherApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
   
        private readonly INavigation _navigation;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel( INavigation navigation)
        {
         
            _navigation = navigation;

            NavigateToLoginCommand = new Command<APIService>(async (apiService) => await NavigateToLogin(apiService));

        }
        public ICommand NavigateToLoginCommand { get; }

        private async Task NavigateToLogin(APIService apiService)
        {
            await _navigation.PushAsync(new LoginPage(apiService));
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
