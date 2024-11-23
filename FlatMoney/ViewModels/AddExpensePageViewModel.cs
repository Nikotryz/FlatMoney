using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(ExpenseInfo), "info")]
    [QueryProperty(nameof(ExpenseType), "type")]
    [QueryProperty(nameof(ExpenseDate), "date")]
    [QueryProperty(nameof(ExpenseCost), "cost")]
    public partial class AddExpensePageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<string> expenseTypes = [];

        [ObservableProperty]
        public string selectedType;

        [ObservableProperty]
        public ExpenseModel expenseInfo;

        [ObservableProperty]
        public ExpenseTypeModel expenseType;

        [ObservableProperty]
        public DateTime expenseDate;

        [ObservableProperty]
        public float expenseCost;



        private readonly LocalDBService _localDBService;
        public AddExpensePageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            SetDefault();

            Load();
        }



        [RelayCommand]
        private async Task SelectionChanged()
        {
            await Load();
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (ExpenseInfo != null) await _localDBService.DeleteItem(ExpenseInfo);

            await Shell.Current.GoToAsync("..");

            SetDefault();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");

            SetDefault();
        }

        [RelayCommand]
        private async Task Save()
        {
            if (ExpenseInfo != null) await Update();
            else await Create();

            await Shell.Current.GoToAsync("..");

            SetDefault();
        }



        private async Task Load()
        {
            var types = await _localDBService.GetItems<ExpenseTypeModel>();
            ExpenseTypes.Clear();
            foreach (var item in types)
            {
                ExpenseTypes.Add(item.Name);
            }
        }

        private void SetDefault()
        {
            ExpenseType = null;
            ExpenseDate = DateTime.Today;
            ExpenseCost = 0;
        }

        private void UpdateInfo()
        {
            ExpenseInfo.TypeId = ExpenseType.Id;
            ExpenseInfo.Date = ExpenseDate;
            ExpenseInfo.Cost = ExpenseCost;
        }

        private async Task Update()
        {
            UpdateInfo();
            await _localDBService.UpdateItem<ExpenseModel>(ExpenseInfo);
        }

        private async Task Create()
        {
            await _localDBService.InsertItem<ExpenseModel>(new ExpenseModel()
            {
                TypeId = ExpenseType.Id,
                Date = ExpenseDate,
                Cost = ExpenseCost,
            });
        }
    }
}
