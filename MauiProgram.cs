using ElorMAUI.Services;
using ElorMAUI.ViewModels;
using ElorMAUI.Services;
using Microsoft.Extensions.Logging;


namespace ElorMAUI;

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
            });

        // 1. Configuración de Blazor WebView
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // 2. Registro de Clientes HTTP y Servicios
        // Añadimos AddHttpClient para que el WeatherService pueda funcionar
        builder.Services.AddSingleton(new HttpClient());

        // Registro de tus servicios
        builder.Services.AddSingleton<CentroService>();
        builder.Services.AddSingleton<WeatherService>(); // Registro del servicio del clima

        // 3. Registro de ViewModels
        builder.Services.AddTransient<MainViewModel>();

        return builder.Build();
    }
}