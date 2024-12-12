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
        //Page 1
        [ObservableProperty]
        public ObservableCollection<ServiceModel> myServices = [];
        [ObservableProperty]
        public ServiceModel selectedService;

        [ObservableProperty]
        public ObservableCollection<ExpenseTypeModel> myExpenseTypes = [];
        [ObservableProperty]
        public ExpenseTypeModel selectedExpenseType;

        //Page 3
        [ObservableProperty]
        public double expensesForYear;
        [ObservableProperty]
        public double expensesForMonth;
        [ObservableProperty]
        public double expensesForDay;

        [ObservableProperty]
        public DateTime currentExpensesDate;
        [ObservableProperty]
        public string currentExpensesMonth;
        [ObservableProperty]
        public ObservableCollection<ExpenseModel> myExpenses = [];
        [ObservableProperty]
        public ExpenseModel selectedExpense;



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
            List<Task> tasks =
            [
                LoadMyServices(),
                LoadMyExpenseTypes(),
                LoadExpensesForYear(),
                LoadExpensesForMonth(),
                LoadExpensesForDay(),
                LoadExpenseHistory()
            ];

            await Task.WhenAll(tasks);
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


        private async Task LoadExpensesForYear()
        {
            var expenses = await _localDBService.GetItems<ExpenseModel>();

            var expensesForYear = expenses.Where(x => x.Date >= DateTime.Today.AddYears(-1));

            double result = 0;

            foreach (var item in expensesForYear)
            {
                result += item.Cost;
            }

            ExpensesForYear = result;
        }

        private async Task LoadExpensesForMonth()
        {
            var expenses = await _localDBService.GetItems<ExpenseModel>();

            var expensesForMonth = expenses.Where(x => x.Date >= DateTime.Today.AddMonths(-1));

            double result = 0;

            foreach (var item in expensesForMonth)
            {
                result += item.Cost;
            }

            ExpensesForMonth = result;
        }

        private async Task LoadExpensesForDay()
        {
            var expenses = await _localDBService.GetItems<ExpenseModel>();

            var expensesForDay = expenses.Where(x => x.Date >= DateTime.Today.AddDays(-1));

            double result = 0;

            foreach (var item in expensesForDay)
            {
                result += item.Cost;
            }

            ExpensesForDay = result;
        }

        private async Task LoadExpenseHistory()
        {
            var expenses = await _localDBService.GetItems<ExpenseModel>();
            var monthExpenses = expenses.Where(x => x.Date.Month == CurrentExpensesDate.Month && x.Date.Year == CurrentExpensesDate.Year);
            var result = monthExpenses.OrderByDescending(x => x.Date);
            MyExpenses.Clear();
            foreach (var item in result)
            {
                MyExpenses.Add(item);
            }
        }

        [RelayCommand]
        public async Task PreviousMonth()
        {
            CurrentExpensesDate = CurrentExpensesDate.AddMonths(-1);

            CurrentExpensesMonth = GetFormattedDate(CurrentExpensesDate);

            await LoadExpenseHistory();
        }

        [RelayCommand]
        public async Task NextMonth()
        {
            CurrentExpensesDate = CurrentExpensesDate.AddMonths(1);

            CurrentExpensesMonth = GetFormattedDate(CurrentExpensesDate);

            await LoadExpenseHistory();
        }

        private string GetFormattedDate(DateTime date)
        {
            return $"{Enum.GetName(typeof(Months), date.Month)} {date.Year} г.";
        }

        enum Months
        {
            Январь = 1,
            Февраль = 2,
            Март = 3,
            Апрель = 4,
            Май = 5,
            Июнь = 6,
            Июль = 7,
            Август = 8,
            Сентябрь = 9,
            Октябрь = 10,
            Ноябрь = 11,
            Декабрь = 12
        }



        private readonly LocalDBService _localDBService;
        public MoneyPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            CurrentExpensesDate = DateTime.Today;

            CurrentExpensesMonth = GetFormattedDate(CurrentExpensesDate);

            Task.Run(async () => await Load());
        }
    }
}
