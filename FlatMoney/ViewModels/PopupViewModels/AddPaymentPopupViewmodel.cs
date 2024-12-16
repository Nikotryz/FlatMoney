using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;

namespace FlatMoney.ViewModels.PopupViewModels
{
    public partial class AddPaymentPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        public int reservationId;


        [ObservableProperty]
        public string? selectedPaymentName;

        [ObservableProperty]
        public string? selectedPaymentType;

        [ObservableProperty]
        public DateTime selectedPaymentDate; 

        [ObservableProperty]
        public float? selectedPaymentCost;



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

        private PaymentModel Create()
        {
            return new PaymentModel
            {
                ReservationId = this.ReservationId,
                Name = SelectedPaymentName,
                Type = SelectedPaymentType,
                Date = SelectedPaymentDate,
                Cost = SelectedPaymentCost
            };
        }

        private void SetDefault()
        {
            SelectedPaymentName = "Оплата";
            SelectedPaymentType = "Безнал";
            SelectedPaymentDate = DateTime.Today;
            SelectedPaymentCost = null;
        }



        private readonly LocalDBService _localDBService;
        private readonly IPopupService _popupService;
        public AddPaymentPopupViewModel(LocalDBService localDBService, IPopupService popupService)
        {
            _localDBService = localDBService;
            _popupService = popupService;
            SetDefault();
        }
    }
}
