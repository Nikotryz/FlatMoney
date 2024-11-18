using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(FlatInfo), "info")]
    public partial class AddFlatPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public List<string> types = new List<string>() {"Арендная", "Своя"};

        [ObservableProperty]
        public FlatModel flatInfo;

        private readonly LocalDBService _localDBService;

        public AddFlatPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
        }

        public AddFlatPageViewModel() { }

        [RelayCommand]
        public async Task Delete()
        {
            if (FlatInfo is not null) await _localDBService.DeleteItem(FlatInfo);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Save()
        {
            await _localDBService.InsertOrUpdateItem<FlatModel>(FlatInfo);
            await Shell.Current.GoToAsync("..");
        }
    }
}
