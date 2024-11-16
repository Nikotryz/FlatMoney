using FlatMoney.LocalDataBase;
using FlatMoney.ViewModels;
using FlatMoney.Views.Details;
using FlatMoney.Views.General;
using Microsoft.Extensions.Logging;
using UraniumUI;

namespace FlatMoney
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("SeroPro.ttf", "Regular");
                    fonts.AddFont("SeroPro-Light.ttf", "Light");
                    fonts.AddFont("SeroPro-Extralight.ttf", "ExtraLight");
                });

            builder.Services.AddSingleton<LocalDBService>();

            builder.Services.AddTransient<FlatPageViewModel>();
            builder.Services.AddTransient<FlatPage>();
            builder.Services.AddTransient<AddFlatPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
