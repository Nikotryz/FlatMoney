using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using FlatMoney.Views.Details;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    [ObservableObject]
    public partial class FlatPageViewModel
    {
        [ObservableProperty]
        public string flatCount = "нет квартир";

        [ObservableProperty]
        public ObservableCollection<FlatModel> items = [];

        private readonly LocalDBService _localDBService;
        public FlatPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            Load();
        }

        [RelayCommand]
        public async Task AddFlat()
        {
            await Shell.Current.GoToAsync(nameof(AddFlatPage));



            //var items = await _localDBService.GetItems<FlatModel>();

            //int maxID = 0;

            //if (items.Count != 0) maxID = items.Max(v => v.Id);

            //FlatModel item = new FlatModel
            //{
            //    Name = $"Квартира {maxID + 1}"
            //};

            //await _localDBService.InsertItem(item);

            //await Load();
        }

        //private async void AddButton(object sender, EventArgs e)
        //{
        //    bool NoException = true;

        //    var flats = await _localDBService.GetItems<FlatModel>();

        //    if (flatName.Text is null || flatName.Text == string.Empty)
        //        NoException = false;

        //    foreach (FlatModel flat in flats)
        //        if (flatName.Text == flat.Name)
        //            NoException = false;

        //    if (NoException)
        //    {
        //        FlatModel flat = new FlatModel
        //        {
        //            Name = flatName.Text,
        //            IsOwn = isOwn.IsToggled
        //        };
        //        Create(flat);
        //    }
        //    else
        //    {
        //        flatName.Text = null;
        //        isOwn.IsToggled = false;
        //    }
        //}

        private async Task Load()
        {
            var items = await _localDBService.GetItems<FlatModel>();
            Items.Clear();
            foreach (FlatModel item in items)
            {
                Items.Add(item);
            }

            FlatCount = $"{await _localDBService.GetCountOfItems<FlatModel>()} квартир";
        }
    }
}
