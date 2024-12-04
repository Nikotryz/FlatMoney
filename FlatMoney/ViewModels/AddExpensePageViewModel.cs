using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(ExpenseInfo), "info")]
    [QueryProperty(nameof(SelectedType), "type")]
    [QueryProperty(nameof(ExpenseDate), "date")]
    [QueryProperty(nameof(ExpenseCost), "cost")]
    public partial class AddExpensePageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<string> expenseTypes = [];



        [ObservableProperty]
        public ExpenseModel expenseInfo;

        [ObservableProperty]
        public string selectedType;

        [ObservableProperty]
        public DateTime expenseDate;

        [ObservableProperty]
        public float expenseCost;



        [ObservableProperty]
        private bool isRefreshing = false;

        

        private readonly LocalDBService _localDBService;
        public AddExpensePageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            SetDefault();

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
            ExpenseDate = DateTime.Today;
            ExpenseCost = 0;
        }

        private void UpdateInfo()
        {
            ExpenseInfo.TypeName = SelectedType;
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
                TypeName = SelectedType,
                Date = ExpenseDate,
                Cost = ExpenseCost,
            });
        }
    }
}
