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
        public ObservableCollection<ExpenseWithTypeNameModel> myExpenses = [];

        [ObservableProperty]
        public ExpenseWithTypeNameModel selectedExpense;



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
                {"type", SelectedExpense.TypeId.ToString() },
                {"date", SelectedExpense.Date },
                {"cost", SelectedExpense.Cost }
            };

            await Shell.Current.GoToAsync(nameof(AddExpenseTypePage), parameters);

            SelectedExpense = null;
        }

        private async Task Load()
        {
            var services = await _localDBService.GetItems<ServiceModel>();
            MyServices.Clear();
            foreach (var item in services)
            {
                MyServices.Add(item);
            }

            var expenseTypes = await _localDBService.GetItems<ExpenseTypeModel>();
            MyExpenseTypes.Clear();
            foreach (var item in expenseTypes)
            {
                MyExpenseTypes.Add(item);
            }

            MyExpenses.Add(new ExpenseWithTypeNameModel
            {
                TypeName = "Прачка",
                Date = DateTime.Now,
                Cost = 340
            });
        }
    }
}
