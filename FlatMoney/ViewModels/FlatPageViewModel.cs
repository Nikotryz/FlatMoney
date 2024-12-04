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
        public ObservableCollection<FlatModel> myFlats = [];

        [ObservableProperty]
        public string myFlatsCount;

        [ObservableProperty]
        private FlatModel selectedItem;

        [ObservableProperty]
        private bool isRefreshing = false;



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



        //[RelayCommand]
        //private async Task NavigatedTo()
        //{
        //    await Load();
        //}

        [RelayCommand]
        private async Task Refresh()
        {
            await Task.Delay(300);
            await Load();
            IsRefreshing = false;
        }

        [RelayCommand]
        private async Task Add()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"info", SelectedItem },
                {"name", string.Empty },
                {"type", "Арендная" },
                {"rentCost", 0 },
                {"rentStartDate", DateTime.Today },
                {"rentInterval", 30 },
                {"rentAutopay", false },
                {"internetCost", 0 },
                {"internetStartDate", DateTime.Today },
                {"internetInterval", 30 },
                {"internetAutopay", false },
                {"address", string.Empty }
            };

            await Shell.Current.GoToAsync(nameof(AddFlatPage), parameters);
        }

        [RelayCommand]
        private async Task SelectionChanged()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
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

            await Shell.Current.GoToAsync(nameof(AddFlatPage), parameters);

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
    }
}
