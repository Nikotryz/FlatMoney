using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    public partial class FlatPageViewModel : ObservableObject
    {
        public ObservableCollection<FlatModel> Items { get; set; } = [];

        private readonly LocalDBService _localDBService;
        public FlatPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
        }

        [RelayCommand]
        public async Task AddFlatCommand()
        {
            var items = await _localDBService.GetItems<FlatModel>();

            

            int maxID = items.Max(v => v.Id);

            FlatModel item = new FlatModel {
                Name = $"Квартира {maxID+1}"
            };

            await _localDBService.InsertItem(item);

            await Load();
        }

        private async Task Load()
        {
            var items = await _localDBService.GetItems<FlatModel>();
            Items.Clear();
            foreach (FlatModel item in items)
            {
                Items.Add(item);
            }
        }
    }
}
