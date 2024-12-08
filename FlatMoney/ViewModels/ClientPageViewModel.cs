using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Views.Details;

namespace FlatMoney.ViewModels
{
    public partial class ClientPageViewModel : ObservableObject
    {
        private readonly LocalDBService _localDBService;
        private AddReservationPage _addReservationPage;
        public ClientPageViewModel(LocalDBService localDBService, AddReservationPage addReservationPage)
        {
            _localDBService = localDBService;

            _addReservationPage = addReservationPage;

            SetPageModal();
        }

        private void SetPageModal()
        {
            Shell.Current.Navigation.PushModalAsync(_addReservationPage);
            Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        public async Task AddShortReservation()
        {
            await Shell.Current.GoToAsync(nameof(AddReservationPage), true);
        }

        [RelayCommand]
        public async Task AddLongReservation()
        {
            await Shell.Current.GoToAsync(nameof(AddReservationPage), true);
        }

        public async Task Load()
        {
            await Task.Delay(1);
        }

    }
}
