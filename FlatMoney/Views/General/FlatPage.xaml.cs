using FlatMoney.ViewModels;

namespace FlatMoney.Views.General;

public partial class FlatPage : ContentPage
{
    public FlatPage()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var vm = BindingContext as FlatPageViewModel;
        await vm!.Load();
    }
}