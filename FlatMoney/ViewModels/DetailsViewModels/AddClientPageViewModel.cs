using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(ClientInfo), "info")]
    [QueryProperty(nameof(SelectedClientName), "name")]
    [QueryProperty(nameof(SelectedClientPhone), "phone")]
    [QueryProperty(nameof(SelectedClientEmail), "email")]
    [QueryProperty(nameof(SelectedClientPassport), "passport")]
    [QueryProperty(nameof(SelectedClientRegistration), "registration")]
    public partial class AddClientPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ClientModel clientInfo;

        [ObservableProperty]
        public string? selectedClientName;

        [ObservableProperty]
        public string? selectedClientPhone;

        [ObservableProperty]
        public string? selectedClientEmail;

        [ObservableProperty]
        public string? selectedClientPassport;

        [ObservableProperty]
        public string? selectedClientRegistration;



        [RelayCommand]
        public async Task Delete()
        {
            if (ClientInfo is null)
            {
                await Shell.Current.GoToAsync("..", animate: true);
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Вы точно хотите удалить клиента?", null, "Да", "Нет");

            if (confirm)
            {
                await _localDBService.DeleteItem(ClientInfo);
                await Shell.Current.GoToAsync("..", animate: true);
                await Toast.Make("Клиент удален").Show();
            }
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..", animate: true);
        }

        [RelayCommand]
        public async Task Save()
        {
            string message = "";
            if (Validate(ref message))
            {
                if (ClientInfo is null)
                {
                    await Create();
                }
                else
                {
                    await Update();
                }
                await Shell.Current.GoToAsync("..", animate: true);
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка", message, "ОК");
            }
        }



        [ObservableProperty]
        private ObservableCollection<ClientModel> clients = [];
        private bool Validate(ref string message)
        {
            foreach (var client in Clients)
            {
                if (SelectedClientPhone == client.Phone && SelectedClientPhone is not null && ClientInfo?.Id != client.Id)
                {
                    message = "Клиент с таким телефоном уже существует";
                    return false;
                }
                if (SelectedClientEmail == client.Email && SelectedClientEmail is not null && ClientInfo?.Id != client.Id) 
                {
                    message = "Клиент с такой электронной почтой уже существует";
                    return false;
                }
                if (SelectedClientPassport == client.Passport && SelectedClientPassport is not null && ClientInfo?.Id != client.Id)
                {
                    message = "Клиент с такими паспортными данными уже существует";
                    return false;
                }
            }
            return true;
        }

        private async Task Update()
        {
            ClientInfo!.Name = SelectedClientName;
            ClientInfo!.Phone = SelectedClientPhone;
            ClientInfo!.Email = SelectedClientEmail;
            ClientInfo!.Passport = SelectedClientPassport;
            ClientInfo!.Registration = SelectedClientRegistration;

            await _localDBService.UpdateItem<ClientModel>(ClientInfo);
        }

        private async Task Create()
        {
            await _localDBService.InsertItem<ClientModel>(new ClientModel()
            {
                Name = SelectedClientName,
                Phone = SelectedClientPhone,
                Email = SelectedClientEmail,
                Passport = SelectedClientPassport,
                Registration = SelectedClientRegistration
            });
        }



        private readonly LocalDBService _localDBService;
        public AddClientPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            LoadClients();
        }

        private async Task LoadClients()
        {
            var items = await _localDBService.GetItems<ClientModel>();
            Clients?.Clear();
            foreach (var item in items)
            {
                Clients?.Add(item);
            }
        }
    }
}
