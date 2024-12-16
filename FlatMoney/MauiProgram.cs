using FlatMoney.LocalDataBase;
using FlatMoney.ViewModels;
using FlatMoney.Views.Details;
using FlatMoney.Views.General;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using DevExpress.Maui;
using UraniumUI;
using Plugin.SegmentedControl.Maui;
using FlatMoney.Views.Popups;
using FlatMoney.ViewModels.PopupViewModels;

namespace FlatMoney
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
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
            builder.Services.AddTransient<FlatPage>(serviceProvider => new FlatPage()
            {
                BindingContext = serviceProvider.GetRequiredService<FlatPageViewModel>()
            });

            builder.Services.AddTransient<AddFlatPageViewModel>();
            builder.Services.AddTransient<AddFlatPage>(serviceProvider => new AddFlatPage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddFlatPageViewModel>()
            });

            builder.Services.AddTransient<MoneyPageViewModel>();
            builder.Services.AddTransient<MoneyPage>(serviceProvider => new MoneyPage()
            {
                BindingContext = serviceProvider.GetRequiredService<MoneyPageViewModel>()
            });

            builder.Services.AddTransient<AddServicePageViewModel>();
            builder.Services.AddTransient<AddServicePage>(serviceProvider => new AddServicePage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddServicePageViewModel>()
            });

            builder.Services.AddTransient<AddExpenseTypePageViewModel>();
            builder.Services.AddTransient<AddExpenseTypePage>(serviceProvider => new AddExpenseTypePage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddExpenseTypePageViewModel>()
            });

            builder.Services.AddTransient<AddClientPageViewModel>();
            builder.Services.AddTransient<AddClientPage>(serviceProvider => new AddClientPage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddClientPageViewModel>()
            });

            builder.Services.AddTransient<AddExpensePageViewModel>();
            builder.Services.AddTransient<AddExpensePage>(serviceProvider => new AddExpensePage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddExpensePageViewModel>()
            });

            builder.Services.AddTransient<ClientPageViewModel>();
            builder.Services.AddTransient<ClientPage>(serviceProvider => new ClientPage()
            {
                BindingContext = serviceProvider.GetRequiredService<ClientPageViewModel>()
            });

            builder.Services.AddTransient<AddReservationPageViewModel>();
            builder.Services.AddTransient<AddReservationPage>(serviceProvider => new AddReservationPage()
            {
                BindingContext = serviceProvider.GetRequiredService<AddReservationPageViewModel>()
            });



            builder.Services.AddTransientPopup<AddServicePopup, AddServicePopupViewModel>();
            builder.Services.AddTransient<AddServicePopup>(serviceProvider => new AddServicePopup()
            {
                BindingContext = serviceProvider.GetRequiredService<AddServicePopupViewModel>()
            });

            builder.Services.AddTransientPopup<AddPaymentPopup, AddPaymentPopupViewModel>();
            builder.Services.AddTransient<AddPaymentPopup>(serviceProvider => new AddPaymentPopup()
            {
                BindingContext = serviceProvider.GetRequiredService<AddPaymentPopupViewModel>()
            });

            builder.Services.AddTransientPopup<AddClientPopup, AddClientPopupViewModel>();
            builder.Services.AddTransient<AddClientPopup>(serviceProvider => new AddClientPopup()
            {
                BindingContext = serviceProvider.GetRequiredService<AddClientPopupViewModel>()
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
