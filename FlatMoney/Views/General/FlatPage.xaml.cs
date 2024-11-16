using FlatMoney.LocalDataBase;
using System.Collections.ObjectModel;
using FlatMoney.ViewModels;
using FlatMoney.Models;
using System.Runtime.CompilerServices;

namespace FlatMoney.Views.General;

public partial class FlatPage : ContentPage
{
    public ObservableCollection<FlatModel> Items { get; set; } = [];
    public FlatModel SelectedItem { get; set; }

    private LocalDBService _localDBService;

    public FlatPage(LocalDBService localDBService, FlatPageViewModel viewModel)
    {
        InitializeComponent();

        _localDBService = localDBService;

        BindingContext = this;

        Load();
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

    private async void AddButton(object sender, EventArgs e)
    {
        //var items = await _localDBService.GetItems<FlatModel>();

        //foreach (var item in items)
        //{
            
        //}

        var items = await _localDBService.GetItems<FlatModel>();

        int maxID = 0;

        if (items.Count != 0) maxID = items.Max(v => v.Id);

        FlatModel item = new FlatModel
        {
            Name = $" вартира {maxID + 1}"
        };

        await _localDBService.InsertItem(item);

        await Load();
    }

    private async void Create(FlatModel item)
    {
        await _localDBService.InsertItem(item);
        await Load();
    }

    private async void DeleteButton(object sender, EventArgs e)
    {
        await _localDBService.DeleteItem(SelectedItem);
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