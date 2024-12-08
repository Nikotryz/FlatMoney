using FlatMoney.LocalDataBase;
using FlatMoney.ViewModels;
using FlatMoney.Views.Details;
using FlatMoney.Views.General;
using Microsoft.Extensions.Logging;
using DevExpress.Maui;
using UraniumUI;
using Plugin.SegmentedControl.Maui;

namespace FlatMoney
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseDevExpress()
                .UseDevExpressControls()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .UseSegmentedControl()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("SeroPro.ttf", "Regular");
                    fonts.AddFont("SeroPro-Light.ttf", "Light");
                    fonts.AddFont("SeroPro-Extralight.ttf", "ExtraLight");
                });

            builder.Services.AddSingleton<LocalDBService>();

            builder.Services.AddTransient<FlatPageViewModel>();
            builder.Services.AddSingleton<FlatPage>(serviceProvider => new FlatPage()
            {
                BindingContext = serviceProvider.GetRequiredService<FlatPageViewModel>()
            });

            builder.Services.AddTransient<AddFlatPageViewModel>();
            builder.Services.AddSingleton<AddFlatPage>(serviceProvider => new AddFlatPage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddFlatPageViewModel>()
            });

            builder.Services.AddTransient<MoneyPageViewModel>();
            builder.Services.AddSingleton<MoneyPage>(serviceProvider => new MoneyPage()
            {
                BindingContext = serviceProvider.GetRequiredService<MoneyPageViewModel>()
            });

            builder.Services.AddTransient<AddServicePageViewModel>();
            builder.Services.AddSingleton<AddServicePage>(serviceProvider => new AddServicePage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddServicePageViewModel>()
            });

            builder.Services.AddTransient<AddExpenseTypePageViewModel>();
            builder.Services.AddSingleton<AddExpenseTypePage>(serviceProvider => new AddExpenseTypePage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddExpenseTypePageViewModel>()
            });

            builder.Services.AddTransient<AddExpensePageViewModel>();
            builder.Services.AddSingleton<AddExpensePage>(serviceProvider => new AddExpensePage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddExpensePageViewModel>()
            });

            builder.Services.AddTransient<ClientPageViewModel>();
            builder.Services.AddSingleton<ClientPage>(serviceProvider => new ClientPage()
            {
                BindingContext = serviceProvider.GetRequiredService<ClientPageViewModel>()
            });

            builder.Services.AddTransient<AddReservationPageViewModel>();
            builder.Services.AddSingleton<AddReservationPage>(serviceProvider => new AddReservationPage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddReservationPageViewModel>()
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
