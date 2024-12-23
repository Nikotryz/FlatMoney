using CommunityToolkit.Maui.Alerts;
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
        [ObservableProperty] public ServiceModel serviceInfo;
        [ObservableProperty] public string serviceName;
        [ObservableProperty] public float serviceCost;

        [RelayCommand]
        private async Task Delete()
        {
            if (ServiceInfo is null)
            {
                await Shell.Current.GoToAsync("..", animate: true);
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Вы точно хотите удалить услугу?", null, "Да", "Нет");

            if (confirm)
            {
                await _localDBService.DeleteItem(ServiceInfo);
                await Shell.Current.GoToAsync("..", animate: true);
                await Toast.Make("Услуга удалена").Show();
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
            if (ServiceInfo is null)
            {
                await Create();
            }
            else
            {
                await Update();
            }

            await Shell.Current.GoToAsync("..", animate: true);
        }

        private async Task Create()
        {
            await _localDBService.InsertItem<ServiceModel>(new ServiceModel()
            {
                Name = ServiceName,
                Cost = ServiceCost
            });
        }

        private async Task Update()
        {
            ServiceInfo.Name = ServiceName;
            ServiceInfo.Cost = ServiceCost;

            await _localDBService.UpdateItem<ServiceModel>(ServiceInfo);
        }



        private readonly LocalDBService _localDBService;
        public AddServicePageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
        }
    }
}
