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
        public string flatCount;

        [ObservableProperty]
        public ObservableCollection<FlatModel> items = [];

        [ObservableProperty]
        public FlatModel selectedItem;

        private AddFlatPage _addFlatPage { get; set; }

        private readonly LocalDBService _localDBService;
        public FlatPageViewModel(LocalDBService localDBService, AddFlatPage addFlatPage)
        {
            _addFlatPage = addFlatPage;

            _localDBService = localDBService;

            Task.Run(async () => await Load());

            Shell.Current.Navigation.PushModalAsync(_addFlatPage);
            Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        public async Task AddFlat()
        {
            await Shell.Current.GoToAsync(nameof(AddFlatPage));
        }

        [RelayCommand]
        public async Task SelectionChanged()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"info", SelectedItem },
                {"name", SelectedItem.Name },
                {"isOwn", SelectedItem.IsOwn },
                {"rentCost", SelectedItem.RentCost },
                {"rentStartDate", SelectedItem.RentStartDate },
                {"rentInterval", SelectedItem.RentInterval },
                {"rentAutopay", SelectedItem.RentAutopay },
                {"internetCost", SelectedItem.InternetCost },
                {"internetStartDate", SelectedItem.InternetStartDate },
                {"internetInterval", SelectedItem.InternetInterval },
                {"internetAutopay", SelectedItem.InternetAutopay }
            };

            await Shell.Current.GoToAsync(nameof(AddFlatPage), parameters);

            SelectedItem = null;
        }

        private async Task Load()
        {
            var items = await _localDBService.GetItems<FlatModel>();
            Items.Clear();
            foreach (FlatModel item in items)
            {
                Items.Add(item);
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

            if (count == 0) FlatCount = "нет квартир";
            else FlatCount = $"{count} {wordEnding}";
        }
    }
}
