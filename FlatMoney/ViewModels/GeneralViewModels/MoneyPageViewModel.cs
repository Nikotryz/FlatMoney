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
        // PAGE 1
        [ObservableProperty]
        public float? profitForYear;
        [ObservableProperty]
        public float? profitForMonth;
        [ObservableProperty]
        public float? profitForDay;


        [ObservableProperty]
        public ObservableCollection<ServiceModel> myServices = [];
        [ObservableProperty]
        public ServiceModel? selectedService;

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
                {"info", SelectedService! },
                {"name", SelectedService!.Name },
                {"cost", SelectedService!.Cost }
            };

            await Shell.Current.GoToAsync(nameof(AddServicePage), true, parameters);

            SelectedService = null;
        }


        [ObservableProperty]
        public ObservableCollection<ExpenseTypeModel> myExpenseTypes = [];
        [ObservableProperty]
        public ExpenseTypeModel? selectedExpenseType;

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
                {"info", SelectedExpenseType! },
                {"name", SelectedExpenseType!.Name }
            };

            await Shell.Current.GoToAsync(nameof(AddExpenseTypePage), true, parameters);

            SelectedExpenseType = null;
        }


        [ObservableProperty]
        public ObservableCollection<ClientModel> myClientsModel = [];
        [ObservableProperty]
        public ObservableCollection<ClientModel> myClientsView = [];
        [ObservableProperty]
        public ClientModel? selectedClient;

        [ObservableProperty]
        public ObservableCollection<string> clientNames = [];
        [ObservableProperty]
        public string? searchText;

        [RelayCommand]
        public async Task AddClient()
        {
            await Shell.Current.GoToAsync(nameof(AddClientPage), true);
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

            await Shell.Current.GoToAsync(nameof(AddClientPage), true, parameters);

            SelectedClient = null;
        }



        // PAGE 2
        [ObservableProperty]
        public float? incomesForYear;
        [ObservableProperty]
        public float? incomesForMonth;
        [ObservableProperty]
        public float? incomesForDay;

        [ObservableProperty]
        public DateTime currentIncomesDate;
        [ObservableProperty]
        public string currentIncomesMonth;
        [ObservableProperty]
        public ObservableCollection<PaymentModel> myIncomes = [];

        [RelayCommand]
        public async Task IncomesPreviousMonth()
        {
            CurrentIncomesDate = CurrentIncomesDate.AddMonths(-1);

            CurrentIncomesMonth = GetFormattedDate(CurrentIncomesDate);

            await LoadIncomeHistory();
        }

        [RelayCommand]
        public async Task IncomesNextMonth()
        {
            CurrentIncomesDate = CurrentIncomesDate.AddMonths(1);

            CurrentIncomesMonth = GetFormattedDate(CurrentIncomesDate);

            await LoadIncomeHistory();
        }



        // PAGE 3
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



        // DATA LOADING
        public async Task Load()
        {
            List<Task> tasks =
            [
                LoadMyServices(),
                LoadMyExpenseTypes(),
                LoadMyClientsModel(),
                LoadExpensesForYear(),
                LoadExpensesForMonth(),
                LoadExpensesForDay(),
                LoadExpenseHistory(),
                LoadIncomesForYear(),
                LoadIncomesForMonth(),
                LoadIncomesForDay(),
                LoadIncomeHistory()
            ];

            await Task.WhenAll(tasks);

            LoadProfit();
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

        private async Task LoadMyClientsModel()
        {
            var clients = await _localDBService.GetItems<ClientModel>();
            MyClientsModel.Clear();
            ClientNames.Clear();
            foreach (var item in clients)
            {
                MyClientsModel.Add(item);
                ClientNames.Add(item.Name!);
            }
            LoadMyClientsView();
        }

        private void LoadMyClientsView()
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
                var searchClients = MyClientsModel.Where(x => x.Name!.ToLower().Contains(SearchText.ToLower()));
                MyClientsView.Clear();
                foreach (var item in searchClients)
                {
                    MyClientsView.Add(item);
                }
            }
            
        }

        partial void OnSearchTextChanged(string? value)
        {
            LoadMyClientsView();
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

        private async Task LoadIncomesForYear()
        {
            var incomes = await _localDBService.GetItems<PaymentModel>();

            var incomesForYear = incomes.Where(x => x.Date >= DateTime.Today.AddYears(-1));

            float? result = 0;

            foreach (var item in incomesForYear)
            {
                result += item.Cost;
            }

            IncomesForYear = result;
        }

        private async Task LoadIncomesForMonth()
        {
            var incomes = await _localDBService.GetItems<PaymentModel>();

            var incomesForMonth = incomes.Where(x => x.Date >= DateTime.Today.AddMonths(-1));

            float? result = 0;

            foreach (var item in incomesForMonth)
            {
                result += item.Cost;
            }

            IncomesForMonth = result;
        }

        private async Task LoadIncomesForDay()
        {
            var incomes = await _localDBService.GetItems<PaymentModel>();

            var incomesForDay = incomes.Where(x => x.Date >= DateTime.Today.AddDays(-1));

            float? result = 0;

            foreach (var item in incomesForDay)
            {
                result += item.Cost;
            }

            IncomesForDay = result;
        }

        private async Task LoadIncomeHistory()
        {
            var incomes = await _localDBService.GetItems<PaymentModel>();
            var monthIncomes = incomes.Where(x => x.Date.Month == CurrentIncomesDate.Month && x.Date.Year == CurrentIncomesDate.Year);
            var result = monthIncomes.OrderByDescending(x => x.Date);
            MyIncomes.Clear();
            foreach (var item in result)
            {
                MyIncomes.Add(item);
            }
        }

        private void LoadProfit()
        {
            ProfitForYear = (float?)(IncomesForYear - ExpensesForYear);
            ProfitForMonth = (float?)(IncomesForMonth - ExpensesForMonth);
            ProfitForDay = (float?)(IncomesForDay - ExpensesForDay);
        }



        private readonly LocalDBService _localDBService;
        public MoneyPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
            SetDefault();
            Task.Run(async () => await Load());
        }

        private void SetDefault()
        {
            CurrentExpensesDate = DateTime.Today;
            CurrentExpensesMonth = GetFormattedDate(CurrentExpensesDate);
            CurrentIncomesDate = DateTime.Today;
            CurrentIncomesMonth = GetFormattedDate(CurrentExpensesDate);
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
    }
}
