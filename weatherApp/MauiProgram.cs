using Microsoft.Extensions.Logging;
using weatherApp.Pages;
using weatherApp.Services;
using CommunityToolkit.Maui;
using weatherApp.ViewModels;
 

namespace weatherApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).UseMauiCommunityToolkitMediaElement();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<APIService>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<AirPollutionPage>();
            builder.Services.AddTransient<AstronomyPage>();
            builder.Services.AddTransient<WeatherPage>();
            builder.Services.AddTransient<CuriositiesPage>();

            return builder.Build();
        }
    }
}
