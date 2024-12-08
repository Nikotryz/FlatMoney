using FlatMoney.ViewModels;

namespace FlatMoney.Views.Details;

public partial class AddExpensePage : ContentPage
{
	public AddExpensePage()
	{
		InitializeComponent();
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var vm = BindingContext as AddExpensePageViewModel;
        await vm!.Load();
    }
}