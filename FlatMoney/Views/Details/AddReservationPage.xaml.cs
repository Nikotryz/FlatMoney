using FlatMoney.ViewModels;

namespace FlatMoney.Views.Details;

public partial class AddReservationPage : ContentPage
{
	public AddReservationPage()
	{
		InitializeComponent();
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var vm = BindingContext as AddReservationPageViewModel;
        await vm!.Load();
    }
}