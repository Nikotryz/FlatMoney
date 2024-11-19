using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(FlatInfo), "info")]

    [QueryProperty(nameof(NameText), "name")]
    [QueryProperty(nameof(IsOwnText), "isOwn")]
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
        public List<string> types = new List<string>() {"Арендная", "Своя"};

        [ObservableProperty]
        public string selectedType;



        [ObservableProperty]
        public FlatModel flatInfo;

        [ObservableProperty]
        public string nameText;

        [ObservableProperty]
        public bool isOwnText;

        [ObservableProperty]
        public float rentCostText;

        [ObservableProperty]
        public DateTime rentStartDateText;

        [ObservableProperty]
        public int rentIntervalText;

        [ObservableProperty]
        public bool rentAutopayText;

        [ObservableProperty]
        public float internetCostText;

        [ObservableProperty]
        public DateTime internetStartDateText;

        [ObservableProperty]
        public int internetIntervalText;

        [ObservableProperty]
        public bool internetAutopayText;

        [ObservableProperty]
        public string addressText;


        private readonly LocalDBService _localDBService;

        public AddFlatPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
        }

        [RelayCommand]
        public async Task Delete()
        {
            if (FlatInfo is not null) await _localDBService.DeleteItem(FlatInfo);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await SetDefault();

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Save()
        {
            if (FlatInfo is not null) await Update();
            else await Create();

            await Shell.Current.GoToAsync("..");
        }

        private async Task SetDefault()
        {
            NameText = string.Empty;
            IsOwnText = false;
            RentCostText = 0;
            RentStartDateText = DateTime.Today;
            RentIntervalText = 30;
            RentAutopayText = false;
            InternetCostText = 0;
            InternetStartDateText = DateTime.Today;
            InternetIntervalText = 30;
            InternetAutopayText = false;
            AddressText = string.Empty;
        }

        private async Task UpdateInfo()
        {
            FlatInfo.Name = NameText;
            FlatInfo.RentCost = RentCostText;
            FlatInfo.RentStartDate = RentStartDateText;
            FlatInfo.RentInterval = RentIntervalText;
            FlatInfo.RentAutopay = RentAutopayText;
            FlatInfo.InternetCost = InternetCostText;
            FlatInfo.InternetStartDate = InternetStartDateText;
            FlatInfo.InternetInterval = InternetIntervalText;
            FlatInfo.InternetAutopay = InternetAutopayText;
            FlatInfo.Address = AddressText;
        }

        private async Task Update()
        {
            await UpdateInfo();
            await _localDBService.UpdateItem<FlatModel>(FlatInfo);
        }

        private async Task Create()
        {
            await _localDBService.InsertItem<FlatModel>(new FlatModel()
            {
                Name = NameText,
                IsOwn = IsOwnText,
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
    }
}
