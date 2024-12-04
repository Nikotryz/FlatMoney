using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.DisplayHelper;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using FlatMoney.Views.Details;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    public partial class MoneyPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public double columnWidth = MauiDisplayHelper.UnitWidth / 4;

        [ObservableProperty]
        public ObservableCollection<ServiceModel> myServices = [];

        [ObservableProperty]
        public ServiceModel selectedService;


        [ObservableProperty]
        public ObservableCollection<ExpenseTypeModel> myExpenseTypes = [];

        [ObservableProperty]
        public ExpenseTypeModel selectedExpenseType;


        [ObservableProperty]
        public ObservableCollection<ExpenseModel> myExpenses = [];

        [ObservableProperty]
        public ExpenseModel selectedExpense;



        [ObservableProperty]
        private bool isRefreshing = false;



        private readonly LocalDBService _localDBService;

        public MoneyPageViewModel(LocalDBService localDBService, AddServicePage addServicePage, AddExpenseTypePage addExpenseTypePage)
        {
            _localDBService = localDBService;

            Task.Run(async () => await Load());
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await Task.Delay(300);
            await Load();
            IsRefreshing = false;
        }

        [RelayCommand]
        private async Task AddService()
        {
            await Shell.Current.GoToAsync(nameof(AddServicePage));
        }

        [RelayCommand]
        private async Task ServiceTapped()
        {
            Dictionary<string, object> parameters = new()
            {
                {"info", SelectedService },
                {"name", SelectedService.Name },
                {"cost", SelectedService.Cost }
            };

            await Shell.Current.GoToAsync(nameof(AddServicePage), parameters);

            SelectedService = null;
        }

        [RelayCommand]
        private async Task AddExpenseType()
        {

            await Shell.Current.GoToAsync(nameof(AddExpenseTypePage));
        }

        [RelayCommand]
        private async Task ExpenseTypeTapped()
        {
            Dictionary<string, object> parameters = new()
            {
                {"info", SelectedExpenseType },
                {"name", SelectedExpenseType.Name }
            };

            await Shell.Current.GoToAsync(nameof(AddExpenseTypePage), parameters);

            SelectedExpenseType = null;
        }

        [RelayCommand]
        private async Task AddExpense()
        {
            await Shell.Current.GoToAsync(nameof(AddExpensePage));
        }

        [RelayCommand]
        private async Task ExpenseTapped()
        {
            Dictionary<string, object> parameters = new()
            {
                {"info", SelectedExpense },
                {"type", SelectedExpense.TypeName },
                {"date", SelectedExpense.Date },
                {"cost", SelectedExpense.Cost }
            };

            await Shell.Current.GoToAsync(nameof(AddExpensePage), parameters);

            SelectedExpense = null;
        }

        private async Task Load()
        {
            await LoadMyServices();
            await LoadMyExpenseTypes();
            await LoadExpenseHistory();
        }

        private async Task LoadMyServices()
        {
            var services = await _localDBService.GetItems<ServiceModel>();
            MyServices.Clear();
            foreach (var item in services)
            {
                MyServices.Add(item);
            }
        }

        private async Task LoadMyExpenseTypes()
        {
            var expenseTypes = await _localDBService.GetItems<ExpenseTypeModel>();
            MyExpenseTypes.Clear();
            foreach (var item in expenseTypes)
            {
                MyExpenseTypes.Add(item);
            }
        }

        private async Task LoadExpenseHistory()
        {
            var expenses = await _localDBService.GetItems<ExpenseModel>();
            MyExpenses.Clear();
            foreach (var item in expenses)
            {
                MyExpenses.Add(item);
            }
        }
    }
}
