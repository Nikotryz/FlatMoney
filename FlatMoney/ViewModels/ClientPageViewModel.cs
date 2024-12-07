using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Views.Details;

namespace FlatMoney.ViewModels
{
    public partial class ClientPageViewModel : ObservableObject
    {
        private readonly LocalDBService _localDBService;
        private AddReservationPage _addShort;
        public ClientPageViewModel(LocalDBService localDBService, AddReservationPage addShort)
        {
            _localDBService = localDBService;

            _addShort = addShort;

            SetShortPageModal();
        }

        private void SetShortPageModal()
        {
            Shell.Current.Navigation.PushModalAsync(_addShort);
            Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        private async Task AddShortReservation()
        {
            await Shell.Current.GoToAsync(nameof(AddReservationPage));
        }

        private async Task Load()
        {
            await Task.Delay(1);
        }

    }
}
