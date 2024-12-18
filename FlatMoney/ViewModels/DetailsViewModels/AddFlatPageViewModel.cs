using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(FlatInfo), "info")]

    [QueryProperty(nameof(NameText), "name")]
    [QueryProperty(nameof(TypeText), "type")]
    [QueryProperty(nameof(RentCostText), "rentCost")]
    [QueryProperty(nameof(RentStartDateText), "rentStartDate")]
    [QueryProperty(nameof(RentIntervalText), "rentInterval")]
    [QueryProperty(nameof(RentAutopayText), "rentAutopay")]
    [QueryProperty(nameof(InternetCostText), "internetCost")]
    [QueryProperty(nameof(InternetStartDateText), "internetStartDate")]
    [QueryProperty(nameof(InternetIntervalText), "internetInterval")]
    [QueryProperty(nameof(InternetAutopayText), "internetAutopay")]
    [QueryProperty(nameof(AddressText), "address")]
    public partial class AddFlatPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private FlatModel flatInfo;

        [ObservableProperty]
        private string nameText;
        [ObservableProperty]
        private string typeText;

        [ObservableProperty]
        private float rentCostText;
        [ObservableProperty]
        private DateTime rentStartDateText;
        [ObservableProperty]
        private int rentIntervalText;
        [ObservableProperty]
        private bool rentAutopayText;

        [ObservableProperty]
        private float internetCostText;
        [ObservableProperty]
        private DateTime internetStartDateText;
        [ObservableProperty]
        private int internetIntervalText;
        [ObservableProperty]
        private bool internetAutopayText;

        [ObservableProperty]
        private string addressText;



        [RelayCommand]
        private async Task Delete()
        {
            if (FlatInfo is null)
            {
                await Shell.Current.GoToAsync("..", animate: true);
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Вы точно хотите удалить квартиру?", null, "Да", "Нет");

            if (confirm)
            {
                await _localDBService.DeleteItem(FlatInfo);
                await Shell.Current.GoToAsync("..", animate: true);
                await Toast.Make("Квартира удалена").Show();
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..", animate: true);
        }

        [RelayCommand]
        private async Task Save()
        {
            if (Validate())
            {
                if (FlatInfo is null)
                {
                    await Create();
                }
                else
                {
                    await Update();
                }
                await Shell.Current.GoToAsync("..", animate: true);
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка", "Квартира с таким названием уже существует", "ОК");
            }
        }



        [ObservableProperty]
        private ObservableCollection<FlatModel> flats = [];
        private bool Validate()
        {
            foreach (var flat in Flats)
            {
                if (NameText == flat.Name && NameText is not null && FlatInfo?.Id != flat.Id)
                {
                    return false;
                }
            }
            return true;
        }
        
        private async Task LoadFlats()
        {
            var items = await _localDBService.GetItems<FlatModel>();
            Flats.Clear();
            foreach (var item in items)
            {
                Flats.Add(item);
            }
        }



        private async Task Update()
        {
            FlatInfo.Name = NameText;
            FlatInfo.Type = TypeText;
            FlatInfo.RentCost = RentCostText;
            FlatInfo.RentStartDate = RentStartDateText;
            FlatInfo.RentInterval = RentIntervalText;
            FlatInfo.RentAutopay = RentAutopayText;
            FlatInfo.InternetCost = InternetCostText;
            FlatInfo.InternetStartDate = InternetStartDateText;
            FlatInfo.InternetInterval = InternetIntervalText;
            FlatInfo.InternetAutopay = InternetAutopayText;
            FlatInfo.Address = AddressText;

            await _localDBService.UpdateItem<FlatModel>(FlatInfo);
        }

        private async Task Create()
        {
            await _localDBService.InsertItem<FlatModel>(new FlatModel()
            {
                Name = NameText,
                Type = TypeText,
                RentCost = RentCostText,
                RentStartDate = RentStartDateText,
                RentInterval = RentIntervalText,
                RentAutopay = RentAutopayText,
                InternetCost = InternetCostText,
                InternetStartDate = InternetStartDateText,
                InternetInterval = InternetIntervalText,
                InternetAutopay = InternetAutopayText,
                Address = AddressText
            });
        }



        private readonly LocalDBService _localDBService;
        public AddFlatPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            SetDefault();

            Task.Run(async () => await LoadFlats());
        }

        private void SetDefault()
        {
            TypeText = "Арендная";
            RentCostText = 20000.00f;
            RentStartDateText = DateTime.Today;
            RentIntervalText = 30;
            InternetCostText = 500.00f;
            InternetStartDateText = DateTime.Today;
            InternetIntervalText = 30;
        }
    }
}
