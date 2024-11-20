using FlatMoney.LocalDataBase;
using FlatMoney.Validations;
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

            builder.Services.AddSingleton<FlatPageViewModel>();
            builder.Services.AddSingleton<FlatPage>(serviceProvider => new FlatPage()
            {
                BindingContext = serviceProvider.GetRequiredService<FlatPageViewModel>()
            });

            builder.Services.AddSingleton<AddFlatPageViewModel>();
            builder.Services.AddSingleton<AddFlatPage>(serviceProvider => new AddFlatPage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddFlatPageViewModel>()
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
