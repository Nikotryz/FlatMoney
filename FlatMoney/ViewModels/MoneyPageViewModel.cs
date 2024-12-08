using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using FlatMoney.Views.Details;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    public partial class MoneyPageViewModel : ObservableObject
    {
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



        private readonly LocalDBService _localDBService;
        public MoneyPageViewModel(LocalDBService localDBService, AddServicePage addServicePage, AddExpenseTypePage addExpenseTypePage, AddExpensePage addExpensePage)
        {
            _localDBService = localDBService;

            Task.Run(async () => await Load());
        }

        [RelayCommand]
        public async Task AddService()
        {
            await Shell.Current.GoToAsync(nameof(AddServicePage), true);
        }

        [RelayCommand]
        public async Task ServiceTapped()
        {
            Dictionary<string, object> parameters = new()
            {
                {"info", SelectedService },
                {"name", SelectedService.Name },
                {"cost", SelectedService.Cost }
            };

            await Shell.Current.GoToAsync(nameof(AddServicePage), true, parameters);

            SelectedService = null;
        }

        [RelayCommand]
        public async Task AddExpenseType()
        {

            await Shell.Current.GoToAsync(nameof(AddExpenseTypePage), true);
        }

        [RelayCommand]
        public async Task ExpenseTypeTapped()
        {
            Dictionary<string, object> parameters = new()
            {
                {"info", SelectedExpenseType },
                {"name", SelectedExpenseType.Name }
            };

            await Shell.Current.GoToAsync(nameof(AddExpenseTypePage), true, parameters);

            SelectedExpenseType = null;
        }

        [RelayCommand]
        public async Task AddExpense()
        {
            await Shell.Current.GoToAsync(nameof(AddExpensePage), true);
        }

        [RelayCommand]
        public async Task ExpenseTapped()
        {
            Dictionary<string, object> parameters = new()
            {
                {"info", SelectedExpense },
                {"type", SelectedExpense.TypeName },
                {"date", SelectedExpense.Date },
                {"cost", SelectedExpense.Cost }
            };

            await Shell.Current.GoToAsync(nameof(AddExpensePage), true, parameters);

            SelectedExpense = null;
        }

        public async Task Load()
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
