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
        // Observable Properties for Page 1
        [ObservableProperty] public float? profitForYear;
        [ObservableProperty] public float? profitForMonth;
        [ObservableProperty] public float? profitForDay;

        [ObservableProperty] public ObservableCollection<ServiceModel> myServices = new();
        [ObservableProperty] public ServiceModel? selectedService;

        [ObservableProperty] public ObservableCollection<ExpenseTypeModel> myExpenseTypes = new();
        [ObservableProperty] public ExpenseTypeModel? selectedExpenseType;

        [ObservableProperty] public ObservableCollection<ClientModel> myClientsModel = new();
        [ObservableProperty] public ObservableCollection<ClientModel> myClientsView = new();
        [ObservableProperty] public ClientModel? selectedClient;
        [ObservableProperty] public ObservableCollection<string> clientNames = new();
        [ObservableProperty] public string? searchText;

        // Observable Properties for Page 2
        [ObservableProperty] public float? incomesForYear;
        [ObservableProperty] public float? incomesForMonth;
        [ObservableProperty] public float? incomesForDay;

        [ObservableProperty] public DateTime currentIncomesDate;
        [ObservableProperty] public string currentIncomesMonth;
        [ObservableProperty] public ObservableCollection<PaymentModel> myIncomes = new();

        // Observable Properties for Page 3
        [ObservableProperty] public float? expensesForYear;
        [ObservableProperty] public float? expensesForMonth;
        [ObservableProperty] public float? expensesForDay;

        [ObservableProperty] public DateTime currentExpensesDate;
        [ObservableProperty] public string currentExpensesMonth;
        [ObservableProperty] public ObservableCollection<ExpenseModel> myExpenses = new();
        [ObservableProperty] public ExpenseModel selectedExpense;

        // Constructor
        private readonly LocalDBService _localDBService;
        public MoneyPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
            SetDefault();
            Task.Run(async () => await LoadData());
        }

        // Default settings for current date
        private void SetDefault()
        {
            CurrentExpensesDate = DateTime.Today;
            CurrentExpensesMonth = GetFormattedDate(CurrentExpensesDate);
            CurrentIncomesDate = DateTime.Today;
            CurrentIncomesMonth = GetFormattedDate(CurrentExpensesDate);
        }

        // Get formatted date
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

        // Load all data
        public async Task LoadData()
        {
            var tasks = new List<Task>
            {
                LoadItems<ServiceModel>(MyServices),
                LoadItems<ExpenseTypeModel>(MyExpenseTypes),
                LoadClients(),
                LoadFinancialData()
            };

            await Task.WhenAll(tasks);
        }

        // Load collection of items (generic method)
        private async Task LoadItems<T>(ObservableCollection<T> collection) where T : BaseTable, new()
        {
            var items = await _localDBService.GetItems<T>();
            collection.Clear();
            foreach (var item in items)
            {
                collection.Add((T)item);
            }
        }

        // Load clients and update both model and view
        private async Task LoadClients()
        {
            var clients = await _localDBService.GetItems<ClientModel>();
            MyClientsModel.Clear();
            ClientNames.Clear();
            foreach (var client in clients)
            {
                MyClientsModel.Add(client);
                ClientNames.Add(client.Name!);
            }
            FilterClients();
        }

        // Filter clients based on search text
        private void FilterClients()
        {
            if (SearchText is null)
            {
                MyClientsView.Clear();
                foreach (var item in MyClientsModel)
                {
                    MyClientsView.Add(item);
                }
            }
            else
            {
                var filteredClients = MyClientsModel.Where(x => x.Name!.ToLower().Contains(SearchText.ToLower()));
                MyClientsView.Clear();
                foreach (var item in filteredClients)
                {
                    MyClientsView.Add(item);
                }
            }
        }

        partial void OnSearchTextChanged(string? value)
        {
            FilterClients();
        }

        // Update financial data
        private async Task LoadFinancialData()
        {
            await Task.WhenAll
            (
                LoadHistory<PaymentModel>(CurrentIncomesDate, MyIncomes),
                LoadHistory<ExpenseModel>(CurrentExpensesDate, MyExpenses),
                LoadExpensesData(),
                LoadIncomesData()
            );
            LoadProfit();
        }

        // Load profit
        private void LoadProfit()
        {
            ProfitForYear = IncomesForYear - ExpensesForYear;
            ProfitForMonth = IncomesForMonth - ExpensesForMonth;
            ProfitForDay = IncomesForDay - ExpensesForDay;
        }

        // Load expenses for year, month, and day
        private async Task LoadExpensesData()
        {
            ExpensesForYear = await LoadFinancialSum<ExpenseModel>(x => x.Date >= DateTime.Today.AddYears(-1));
            ExpensesForMonth = await LoadFinancialSum<ExpenseModel>(x => x.Date >= DateTime.Today.AddMonths(-1));
            ExpensesForDay = await LoadFinancialSum<ExpenseModel>(x => x.Date >= DateTime.Today.AddDays(-1));
        }

        // Load incomes for year, month, and day
        private async Task LoadIncomesData()
        {
            IncomesForYear = await LoadFinancialSum<PaymentModel>(x => x.Date >= DateTime.Today.AddYears(-1));
            IncomesForMonth = await LoadFinancialSum<PaymentModel>(x => x.Date >= DateTime.Today.AddMonths(-1));
            IncomesForDay = await LoadFinancialSum<PaymentModel>(x => x.Date >= DateTime.Today.AddDays(-1));
        }

        // Generic method to load financial sums (expenses or incomes)
        private async Task<float?> LoadFinancialSum<T>(Func<T, bool> predicate) where T : BaseTable, new()
        {
            var items = await _localDBService.GetItems<T>();
            return items.Where(predicate).Sum(x => Convert.ToSingle(typeof(T).GetProperty("Cost")?.GetValue(x)));
        }

        // Load history for expenses or incomes
        private async Task LoadHistory<T>(DateTime date, ObservableCollection<T> collection) where T : BaseTable, new()
        {
            var items = await _localDBService.GetItems<T>();
            var monthItems = items.Where(x => ((DateTime)typeof(T)?.GetProperty("Date")?.GetValue(x)).Month == date.Month &&
                                              ((DateTime)typeof(T)?.GetProperty("Date")?.GetValue(x)).Year == date.Year);
            collection.Clear();
            foreach (var item in monthItems.OrderByDescending(x => typeof(T).GetProperty("Date")?.GetValue(x)))
            {
                collection.Add(item);
            }
        }

        // Navigate to Add Page
        [RelayCommand]
        public async Task AddItem(string pageName)
        {
            await Shell.Current.GoToAsync(pageName, animate: true);
        }

        // Navigate to detail page of the selected item
        [RelayCommand]
        public async Task ServiceTapped()
        {
            Dictionary<string, object> parameters = new()
            {
                {"info", SelectedService! },
                {"name", SelectedService!.Name },
                {"cost", SelectedService!.Cost }
            };

            await Shell.Current.GoToAsync(nameof(AddServicePage), animate: true, parameters);

            SelectedService = null;
        }

        [RelayCommand]
        public async Task ExpenseTypeTapped()
        {
            Dictionary<string, object> parameters = new()
            {
                {"info", SelectedExpenseType! },
                {"name", SelectedExpenseType!.Name }
            };

            await Shell.Current.GoToAsync(nameof(AddExpenseTypePage), animate: true, parameters);

            SelectedExpenseType = null;
        }

        [RelayCommand]
        public async Task ClientTapped()
        {
            Dictionary<string, object?> parameters = new()
            {
                {"info", SelectedClient},
                {"name", SelectedClient?.Name},
                {"phone", SelectedClient?.Phone},
                {"email", SelectedClient?.Email},
                {"passport", SelectedClient?.Passport},
                {"registration", SelectedClient?.Registration}
            };

            await Shell.Current.GoToAsync(nameof(AddClientPage), animate: true, parameters);

            SelectedClient = null;
        }

        // Commands for navigating between months
        [RelayCommand]
        public async Task NavigateIncomesMonth(bool isNext)
        {
            if (isNext)
            {
                CurrentIncomesDate = CurrentIncomesDate.AddMonths(1);
            }
            else
            {
                CurrentIncomesDate = CurrentIncomesDate.AddMonths(-1);
            }

            CurrentIncomesMonth = GetFormattedDate(CurrentIncomesDate);

            await LoadHistory<PaymentModel>(CurrentIncomesDate, MyIncomes);
        }

        [RelayCommand]
        public async Task NavigateExpensesMonth(bool isNext)
        {
            if (isNext)
            {
                CurrentExpensesDate = CurrentExpensesDate.AddMonths(1);
            }
            else
            {
                CurrentExpensesDate = CurrentExpensesDate.AddMonths(-1);
            }

            CurrentExpensesMonth = GetFormattedDate(CurrentExpensesDate);

            await LoadHistory<ExpenseModel>(CurrentExpensesDate, MyExpenses);
        }
    }
}