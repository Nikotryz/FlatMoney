using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatMoney.ViewModels.PopupViewModels
{
    public partial class AddServicePopupViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<string> services = [];

        [ObservableProperty]
        public ReservationServiceModel? reservationServiceInfo;

        [ObservableProperty]
        public ServiceModel? selectedService;

        [ObservableProperty]
        public float? serviceCost;



        [RelayCommand]
        private async Task Delete()
        {
            if (ReservationServiceInfo is null)
            {
                await _popupService.ClosePopupAsync();
                SetDefault();
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Вы точно хотите удалить услугу?", null, "Да", "Нет");

            if (confirm)
            {
                await _localDBService.DeleteItem(ReservationServiceInfo);
                await _popupService.ClosePopupAsync();
                SetDefault();
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _popupService.ClosePopupAsync();

            SetDefault();
        }

        [RelayCommand]
        public async Task Save()
        {
            if (ReservationServiceInfo is null) await Create();
            else await Update();

            await _popupService.ClosePopupAsync();

            SetDefault();
        }

        private void UpdateInfo()
        {
            ReservationServiceInfo.ReservationId = SelectedService.Id;
            ReservationServiceInfo.Name = SelectedService.Name;
            ReservationServiceInfo.Cost = ServiceCost;
        }

        private async Task Update()
        {
            UpdateInfo();
            await _localDBService.UpdateItem<ReservationServiceModel>(ReservationServiceInfo);
        }

        private async Task Create()
        {
            await _localDBService.InsertItem<ReservationServiceModel>(new ReservationServiceModel()
            {
                ReservationId = SelectedService.Id,
                Name = SelectedService.Name,
                Cost = ServiceCost
            });
        }

        public async Task Load()
        {
            var services = await _localDBService.GetItems<ServiceModel>();
            Services.Clear();
            foreach (var item in services)
            {
                Services.Add(item.Name);
            }
        }

        private void SetDefault()
        {
            ReservationServiceInfo = null;
            SelectedService = null;
            ServiceCost = null;
        }


        private readonly LocalDBService _localDBService;
        private readonly IPopupService _popupService;
        public AddServicePopupViewModel(LocalDBService localDBService, IPopupService popupService)
        {
            _localDBService = localDBService;
            _popupService = popupService;

            Task.Run(async () => await Load());
        }
    }
}
