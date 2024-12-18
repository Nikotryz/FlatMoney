using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using FlatMoney.Views.Details;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    public partial class FlatPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public string myFlatsCount;

        [ObservableProperty]
        public ObservableCollection<FlatModel> myFlats = [];

        [ObservableProperty]
        private FlatModel selectedItem;



        [RelayCommand]
        private async Task Add()
        {
            await Shell.Current.GoToAsync(nameof(AddFlatPage), animate: true);
        }

        [RelayCommand]
        private async Task SelectionChanged()
        {
            Dictionary<string, object?> parameters = new()
            {
                {"info", SelectedItem },
                {"name", SelectedItem.Name },
                {"type", SelectedItem.Type },
                {"rentCost", SelectedItem.RentCost },
                {"rentStartDate", SelectedItem.RentStartDate },
                {"rentInterval", SelectedItem.RentInterval },
                {"rentAutopay", SelectedItem.RentAutopay },
                {"internetCost", SelectedItem.InternetCost },
                {"internetStartDate", SelectedItem.InternetStartDate },
                {"internetInterval", SelectedItem.InternetInterval },
                {"internetAutopay", SelectedItem.InternetAutopay },
                {"address", SelectedItem.Address }
            };

            await Shell.Current.GoToAsync(nameof(AddFlatPage), animate: true, parameters);

            SelectedItem = null;
        }



        public async Task Load()
        {
            var flats = await _localDBService.GetItems<FlatModel>();
            MyFlats.Clear();
            foreach (FlatModel item in flats)
            {
                MyFlats.Add(item);
            }

            await CountLoad();
        }

        private async Task CountLoad()
        {
            int count = await _localDBService.GetCountOfItems<FlatModel>();
            int lastDigit = (int)(count % 10);
            string wordEnding;

            if (lastDigit == 1) wordEnding = "квартира";
            else if (lastDigit > 1 && lastDigit < 5) wordEnding = "квартиры";
            else wordEnding = "квартир";

            if (count == 0) MyFlatsCount = "нет квартир";
            else MyFlatsCount = $"{count} {wordEnding}";
        }



        private readonly LocalDBService _localDBService;
        public FlatPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            Task.Run(async () => await Load());
        }
    }
}