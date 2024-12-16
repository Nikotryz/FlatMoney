using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using FlatMoney.Views.Details;
using Microsoft.Maui.Platform;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels.PopupViewModels
{
    public partial class AddClientPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        public int reservationId;


        [ObservableProperty]
        public ObservableCollection<ClientModel> clientsModel = [];

        [ObservableProperty]
        public ObservableCollection<ClientModel> clientsView = [];

        [ObservableProperty]
        public ObservableCollection<string>? clientNames = [];

        [ObservableProperty]
        public ObservableCollection<object>? selectedClients = [];

        [ObservableProperty]
        public string? searchText;



        [RelayCommand]
        public async Task CreateClient()
        {
            await _popupService.ClosePopupAsync();

            await Shell.Current.GoToAsync(nameof(AddClientPage), true);
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await _popupService.ClosePopupAsync();
        }

        [RelayCommand]
        public async Task Save()
        {
            var model = AddClients();

            await _popupService.ClosePopupAsync(model);
        }

        private ObservableCollection<ReservationClientModel?>? AddClients()
        {
            var result = new ObservableCollection<ReservationClientModel?>();

            if (SelectedClients is null)
            {
                return null;
            }

            foreach (var item in SelectedClients)
            {
                var clientModel = item as ClientModel;
                result.Add(new ReservationClientModel
                {
                    ReservationId = this.ReservationId,
                    ClientId = clientModel?.Id,
                    Name = clientModel?.Name
                });
            }

            return result;
        }

        private void SetDefault()
        {
            SearchText = null;
        }

        public async Task LoadModel()
        {
            var clients = await _localDBService.GetItems<ClientModel>();
            ClientsModel.Clear();
            ClientNames?.Clear();
            foreach (var item in clients)
            {
                ClientsModel.Add(item);
                ClientNames?.Add(item.Name!);
            }
            LoadView();
        }

        private void LoadView()
        {
            if (SearchText is null)
            {
                ClientsView.Clear();
                foreach (var item in ClientsModel)
                {
                    ClientsView.Add(item);
                }
            }
            else
            {
                var searchClients = ClientsModel.Where(x => x.Name!.ToLower().Contains(SearchText.ToLower()));
                ClientsView.Clear();
                foreach (var item in searchClients)
                {
                    ClientsView.Add(item);
                }
            }

        }

        partial void OnSearchTextChanged(string? value)
        {
            LoadView();
        }



        private readonly LocalDBService _localDBService;
        private readonly IPopupService _popupService;
        public AddClientPopupViewModel(LocalDBService localDBService, IPopupService popupService)
        {
            _localDBService = localDBService;
            _popupService = popupService;
            SetDefault();
            Task.Run(async () => await LoadModel());
        }
    }
}
