﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels.PopupViewModels
{
    public partial class AddServicePopupViewModel : ObservableObject
    {
        [ObservableProperty]
        public int reservationId;


        [ObservableProperty]
        public ObservableCollection<ServiceModel> services = [];

        [ObservableProperty]
        public ServiceModel? selectedService;

        [ObservableProperty]
        public float? serviceCost;



        [RelayCommand]
        public async Task Cancel()
        {
            await _popupService.ClosePopupAsync();

            SetDefault();
        }

        [RelayCommand]
        public async Task Save()
        {
            var model = Create();

            await _popupService.ClosePopupAsync(model);

            SetDefault();
        }

        [RelayCommand]
        public void SelectedServiceChanged()
        {
            ServiceCost = SelectedService?.Cost;
        }

        private ReservationServiceModel Create()
        {
            return new ReservationServiceModel
            {
                ReservationId = this.ReservationId,
                Name = SelectedService?.Name,
                Cost = ServiceCost
            };
        }

        public async Task Load()
        {
            var services = await _localDBService.GetItems<ServiceModel>();
            Services.Clear();
            foreach (var item in services)
            {
                Services.Add(item);
            }
        }

        private void SetDefault()
        {
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
