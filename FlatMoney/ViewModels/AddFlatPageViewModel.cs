using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;

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
        private List<string> types = new List<string>() {"Арендная", "Своя"};



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


        
        private readonly LocalDBService _localDBService;

        public AddFlatPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            SetDefault();
        }



        [RelayCommand]
        private async Task Delete()
        {
            if (FlatInfo is null)
            {
                await Shell.Current.GoToAsync("..", true);
                SetDefault();
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Вы точно хотите удалить квартиру?", null, "Да", "Нет");

            if (confirm)
            {
                await _localDBService.DeleteItem(FlatInfo);
                await Shell.Current.GoToAsync("..", true);
                SetDefault();
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");

            SetDefault();
        }

        [RelayCommand]
        private async Task Save()
        {
            if (FlatInfo is not null) await Update();
            else await Create();

            await Shell.Current.GoToAsync("..");

            SetDefault();
        }



        private void SetDefault()
        {
            NameText = string.Empty;
            TypeText = "Арендная";
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

        private void UpdateInfo()
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
        }

        private async Task Update()
        {
            UpdateInfo();
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
    }
}
