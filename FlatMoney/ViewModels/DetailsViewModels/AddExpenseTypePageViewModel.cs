using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(ExpenseTypeInfo), "info")]
    [QueryProperty(nameof(ExpenseTypeName), "name")]
    public partial class AddExpenseTypePageViewModel : ObservableObject
    {
        [ObservableProperty] public ExpenseTypeModel expenseTypeInfo;
        [ObservableProperty] public string expenseTypeName;

        [RelayCommand]
        public async Task Delete()
        {
            if (ExpenseTypeInfo is null)
            {
                await Shell.Current.GoToAsync("..", animate: true);
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Вы точно хотите удалить категорию?", null, "Да", "Нет");

            if (confirm)
            {
                await _localDBService.DeleteItem(ExpenseTypeInfo);
                await Shell.Current.GoToAsync("..", animate: true);
                await Toast.Make("Категория удалена").Show();
            }
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..", animate: true);
        }

        [RelayCommand]
        public async Task Save()
        {
            if (ExpenseTypeInfo is null)
            {
                await Create();
            }
            else
            {
                await Update();
            }

            await Shell.Current.GoToAsync("..", animate: true);
        }

        private async Task Create()
        {
            await _localDBService.InsertItem<ExpenseTypeModel>(new ExpenseTypeModel()
            {
                Name = ExpenseTypeName,
            });
        }

        private async Task Update()
        {
            ExpenseTypeInfo.Name = ExpenseTypeName;

            await _localDBService.UpdateItem<ExpenseTypeModel>(ExpenseTypeInfo);
        }



        private readonly LocalDBService _localDBService;
        public AddExpenseTypePageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
        }
    }
}
