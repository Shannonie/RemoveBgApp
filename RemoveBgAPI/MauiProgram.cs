using Microsoft.Extensions.Logging;
using RemoveBgAPI.Services;
using RemoveBgAPI.Utils;
using RemoveBgAPI.ViewModels;
using RemoveBgAPI.Views;

namespace RemoveBgAPI
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
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<ImageService>();
            builder.Services.AddSingleton<RemoveBgService>();
#if WINDOWS || ANDROID
            builder.Services.AddSingleton<IFileSaver, FileSaver>();
#endif

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddSingleton<SavePageViewModel>();
            builder.Services.AddTransient<SavePage>();

            return builder.Build();
        }
    }
}
