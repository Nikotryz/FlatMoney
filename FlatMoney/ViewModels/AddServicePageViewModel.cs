using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(ServiceInfo), "info")]
    [QueryProperty(nameof(ServiceName), "name")]
    [QueryProperty(nameof(ServiceCost), "cost")]
    public partial class AddServicePageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ServiceModel serviceInfo;

        [ObservableProperty]
        public string serviceName;

        [ObservableProperty]
        public float serviceCost;



        private readonly LocalDBService _localDBService;

        public AddServicePageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            SetDefault();
        }



        [RelayCommand]
        private async Task Delete()
        {
            if (ServiceInfo is null)
            {
                await Shell.Current.GoToAsync("..", true);
                SetDefault();
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Вы точно хотите удалить услугу?", null, "Да", "Нет");

            if (confirm)
            {
                await _localDBService.DeleteItem(ServiceInfo);
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
            if (ServiceInfo != null) await Update();
            else await Create();

            await Shell.Current.GoToAsync("..");

            SetDefault();
        }



        private void SetDefault()
        {
            ServiceName = string.Empty;
            ServiceCost = 0;
        }

        private void UpdateInfo()
        {
            ServiceInfo.Name = ServiceName;
            ServiceInfo.Cost = ServiceCost;
        }

        private async Task Update()
        {
            UpdateInfo();
            await _localDBService.UpdateItem<ServiceModel>(ServiceInfo);
        }

        private async Task Create()
        {
            await _localDBService.InsertItem<ServiceModel>(new ServiceModel()
            {
                Name = ServiceName,
                Cost = ServiceCost
            });
        }
    }
}
