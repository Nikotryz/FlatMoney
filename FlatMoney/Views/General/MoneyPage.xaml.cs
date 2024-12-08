using FlatMoney.ViewModels;

namespace FlatMoney.Views.General;

public partial class MoneyPage : ContentPage
{
	public MoneyPage()
	{
		InitializeComponent();
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var vm = BindingContext as MoneyPageViewModel;
        await vm!.Load();
    }
}