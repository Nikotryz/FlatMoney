using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    public partial class MoneyPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<ServiceModel> myServices = [];

        [ObservableProperty]
        public ObservableCollection<ExpenseTypeModel> myExpenseTypes = [];



        [ObservableProperty]
        private bool isRefreshing = false;



        private readonly LocalDBService _localDBService;

        public MoneyPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            Task.Run(async () => await Load());
        }

        [RelayCommand]
        private async Task Refresh()
        {
            await Task.Delay(500);
            await Load();
            IsRefreshing = false;
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
        }
    }
}
