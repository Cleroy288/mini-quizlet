using Microsoft.Extensions.Logging;
using Intello.Services;
using Intello.ViewModels;
using Intello.Pages;

namespace Intello;

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
            });

        // Register services
        builder.Services.AddSingleton<IJsonDataService, JsonDataService>();
        
        // Register ViewModels
        builder.Services.AddSingleton<QcmViewModel>();
        
        // Register Pages
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<QcmListPage>();
        builder.Services.AddTransient<QcmQuizPage>();
        builder.Services.AddTransient<QcmResultsPage>();
        builder.Services.AddTransient<FlashcardPage>();
        builder.Services.AddTransient<CodeGamePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
