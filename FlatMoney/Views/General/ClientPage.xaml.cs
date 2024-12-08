using FlatMoney.ViewModels;

namespace FlatMoney.Views.General;

public partial class ClientPage : ContentPage
{
	public ClientPage()
	{
		InitializeComponent();
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var vm = BindingContext as ClientPageViewModel;
        await vm!.Load();
    }
}