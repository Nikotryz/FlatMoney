using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using FlatMoney.ViewModels.PopupViewModels;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FlatMoney.ViewModels
{
    [QueryProperty(nameof(ReservationInfo), "info")]
    [QueryProperty(nameof(ReservationId), "id")]
    [QueryProperty(nameof(ReservationType), "type")]
    [QueryProperty(nameof(SelectedFlatId), "flatid")]
    [QueryProperty(nameof(InDate), "indate")]
    [QueryProperty(nameof(OutDate), "outdate")]
    [QueryProperty(nameof(InTime), "intime")]
    [QueryProperty(nameof(OutTime), "outtime")]
    [QueryProperty(nameof(PeopleAmount), "people")]
    [QueryProperty(nameof(ChildAmount), "child")]
    [QueryProperty(nameof(DaysCount), "days")]
    [QueryProperty(nameof(SumPerDay), "sum")]
    [QueryProperty(nameof(SelectedSegmentIndex), "status")]
    [QueryProperty(nameof(Comment), "comment")]
    public partial class AddReservationPageViewModel : ObservableObject
    {
        // GENERAL DATA
        [ObservableProperty]
        public ReservationModel? reservationInfo;

        [ObservableProperty]
        private int reservationId;

        [ObservableProperty]
        public string reservationType;



        // PAGE 1
        [ObservableProperty]
        public int selectedSegmentIndex;

        [ObservableProperty]
        public string? clientName;
        [ObservableProperty]
        public string? clientPhone;

        [ObservableProperty]
        public DateTime inDate;
        [ObservableProperty]
        public DateTime outDate;
        [ObservableProperty]
        public TimeSpan? inTime;
        [ObservableProperty]
        public TimeSpan? outTime;

        [ObservableProperty]
        public int daysCount;
        [ObservableProperty]
        public float sumPerDay;
        [ObservableProperty]
        public float totalSum;

        [ObservableProperty]
        public ObservableCollection<FlatModel> flatsList = [];
        [ObservableProperty]
        public FlatModel? selectedFlat;
        [ObservableProperty]
        public int selectedFlatId;

        [ObservableProperty]
        public string? comment;



        // PAGE 2
        [ObservableProperty]
        public int peopleAmount;

        [ObservableProperty]
        public int childAmount;

        [ObservableProperty]
        public ObservableCollection<ReservationClientModel> clientsList = [];

        [ObservableProperty]
        public ObservableCollection<object> selectedClients = [];

        [RelayCommand]
        public async Task AddClient()
        {
            int id = await GetReservationId();

            ObservableCollection<ReservationClientModel>? result = await _popupService.ShowPopupAsync<AddClientPopupViewModel>(onPresenting: vm => vm.ReservationId = id) as ObservableCollection<ReservationClientModel>;

            if (result is not null)
            {
                foreach (var item in result)
                {
                    if (!HasClient(item.ClientId))
                    {
                        ClientsList.Add(item);
                    }
                }
                await LoadMainClientData();
            }
        }

        [RelayCommand]
        public async Task DeleteClients()
        {
            foreach (ReservationClientModel item in SelectedClients!)
            {
                ClientsList.Remove(item);
            }
            SelectedClients.Clear();
            await LoadMainClientData();
        }

        private bool HasClient(int? clientId)
        {
            foreach (var item in ClientsList)
            {
                if (item.ClientId == clientId)
                {
                    return true;
                }
            }
            return false;
        }



        // PAGE 3
        [ObservableProperty]
        public ObservableCollection<ReservationServiceModel> servicesList = [];

        [ObservableProperty]
        public float? servicesSum;

        [ObservableProperty]
        public ObservableCollection<object> selectedServices = [];

        [RelayCommand]
        public async Task AddService()
        {
            int id = await GetReservationId();

            ReservationServiceModel? result = await _popupService.ShowPopupAsync<AddServicePopupViewModel>(onPresenting: vm => vm.ReservationId = id) as ReservationServiceModel;

            if (result is not null)
            { 
                ServicesList.Add(result);
            }

            LoadServicesSum();
        }

        [RelayCommand]
        public void DeleteServices()
        {
            foreach (ReservationServiceModel item in SelectedServices!)
            {
                ServicesList.Remove(item);
            }
            SelectedServices.Clear();
            LoadServicesSum();
        }

        private void LoadServicesSum()
        {
            ServicesSum = 0;
            foreach (var item in ServicesList)
            {
                ServicesSum += item.Cost;
            }
            if (ServicesSum == 0) ServicesSum = null;
        }

        

        // PAGE 4
        [ObservableProperty]
        public ObservableCollection<PaymentModel> paymentsList = [];

        [ObservableProperty]
        public float? paymentsSum;

        [ObservableProperty]
        public ObservableCollection<object> selectedPayments = [];

        [RelayCommand]
        public async Task AddPayment()
        {
            int id = await GetReservationId();

            PaymentModel? result = await _popupService.ShowPopupAsync<AddPaymentPopupViewModel>(onPresenting: vm => vm.ReservationId = id) as PaymentModel;

            if (result is not null)
            {
                PaymentsList.Add(result);
            }

            LoadPaymentsSum();
        }

        [RelayCommand]
        public void DeletePayments()
        {
            foreach (PaymentModel item in SelectedPayments!)
            {
                PaymentsList.Remove(item);
            }
            SelectedPayments.Clear();
            LoadPaymentsSum();
        }

        private void LoadPaymentsSum()
        {
            PaymentsSum = 0;
            foreach (var item in PaymentsList)
            {
                PaymentsSum += item.Cost;
            }
            if (PaymentsSum == 0) PaymentsSum = null;
        }



        // CONTROL BUTTONS
        [RelayCommand]
        public async Task Delete()
        {
            if (ReservationInfo is null)
            {
                await Shell.Current.GoToAsync("..", animate: true);
                return;
            }

            var confirm = await Shell.Current.DisplayAlert("Вы точно хотите удалить заселение?", null, "Да", "Нет");

            if (confirm)
            {
                await _localDBService.DeleteItem(ReservationInfo);
                await Shell.Current.GoToAsync("..", animate: true);
                await Toast.Make("Заселение удалено").Show();
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
            if (ReservationInfo is null)
            {
                await Create();
            }
            else
            {
                await Update();
            }

            await InsertReservationData();

            await Shell.Current.GoToAsync("..", animate: true);
        } 

        private async Task Create()
        {
            await _localDBService.InsertItem<ReservationModel>(new ReservationModel
            {
                FlatId = this.SelectedFlat?.Id,
                FlatName = this.SelectedFlat?.Name,
                Type = this.ReservationType,
                CheckInDate = GetDate(InDate, InTime),
                CheckOutDate = GetDate(OutDate, OutTime),
                PeopleAmount = this.PeopleAmount,
                ChildAmount = this.ChildAmount,
                DaysOrMonthsAmount = this.DaysCount,
                CostPerAmount = this.SumPerDay,
                DepositCost = null,
                DepositStatus = null,
                ReservationStatus = GetStatus(this.SelectedSegmentIndex),
                Comment = this.Comment
            });
        }

        private async Task Update()
        {
            ReservationInfo!.FlatId = this.SelectedFlat?.Id;
            ReservationInfo.FlatName = this.SelectedFlat?.Name;
            ReservationInfo.Type = this.ReservationType;
            ReservationInfo.CheckInDate = GetDate(InDate, InTime);
            ReservationInfo.CheckOutDate = GetDate(OutDate, OutTime);
            ReservationInfo.PeopleAmount = this.PeopleAmount;
            ReservationInfo.ChildAmount = this.ChildAmount;
            ReservationInfo.DaysOrMonthsAmount = this.DaysCount;
            ReservationInfo.CostPerAmount = this.SumPerDay;
            ReservationInfo.DepositCost = null;
            ReservationInfo.DepositStatus = null;
            ReservationInfo.ReservationStatus = GetStatus(this.SelectedSegmentIndex);
            ReservationInfo.Comment = this.Comment;

            await _localDBService.UpdateItem<ReservationModel>(ReservationInfo);
        }

        private async Task InsertReservationData()
        {
            // CLEAR OLD DATA
            var clients = await _localDBService.GetItems<ReservationClientModel>();
            var services = await _localDBService.GetItems<ReservationServiceModel>();
            var payments = await _localDBService.GetItems<PaymentModel>();

            var needClients = clients.Where(x => x.ReservationId == this.ReservationId);
            var needServices = services.Where(x => x.ReservationId == this.ReservationId);
            var needPayments = payments.Where(x => x.ReservationId == this.ReservationId);

            foreach (var item in needClients)
            {
                await _localDBService.DeleteItem<ReservationClientModel>(item);
            }
            foreach (var item in needServices)
            {
                await _localDBService.DeleteItem<ReservationServiceModel>(item);
            }
            foreach (var item in needPayments)
            {
                await _localDBService.DeleteItem<PaymentModel>(item);
            }



            // INSERT NEW DATA
            foreach (var item in ClientsList)
            {
                await _localDBService.InsertItem<ReservationClientModel>(item);
            }
            foreach (var item in ServicesList)
            {
                await _localDBService.InsertItem<ReservationServiceModel>(item);
            }
            foreach (var item in PaymentsList)
            {
                await _localDBService.InsertItem<PaymentModel>(item);
            }
        }

        private static DateTime GetDate(DateTime date, TimeSpan? time)
        {
            if (!time.HasValue)
            {
                return date;
            }
            return DateTime.Parse($"{DateOnly.FromDateTime(date)} {time}");
        }

        private string? GetStatus(int index)
        {
            switch (index)
            {
                case 0: return "Бронь";
                case 1: return "Заселен";
                case 2: return "Выехал";
            }
            return null;
        }



        // DATA LOADING
        public async Task Load()
        {
            List<Task> tasks =
            [
                LoadFlats(),
                LoadClients(),
                LoadServices(),
                LoadPayments()
            ];

            await Task.WhenAll(tasks);
        }

        private async Task LoadFlats()
        {
            var items = await _localDBService.GetItems<FlatModel>();
            FlatsList.Clear();
            foreach (var item in items)
            {
                FlatsList.Add(item);
            }
            if (SelectedFlatId != 0)
            {
                SelectedFlat = FlatsList.Where(x => x.Id == SelectedFlatId).First();
            }
        }

        private async Task LoadClients()
        {
            var items = await _localDBService.GetItems<ReservationClientModel>();
            var needItems = items.Where(x => x.ReservationId == this.ReservationId);
            ClientsList.Clear();
            foreach (var item in needItems)
            {
                ClientsList.Add(item);
            }
            await LoadMainClientData();
        }

        private async Task LoadMainClientData()
        {
            if (ClientsList is null || ClientsList.Count == 0)
            {
                ClientName = null;
                ClientPhone = null;
                return;
            }
            var client = await _localDBService.GetItem<ClientModel>(ClientsList.First().ClientId);
            ClientName = client.Name;
            ClientPhone = client.Phone;
        }

        private async Task LoadServices()
        {
            var items = await _localDBService.GetItems<ReservationServiceModel>();
            var needItems = items.Where(x => x.ReservationId == this.ReservationId);
            ServicesList.Clear();
            foreach (var item in needItems)
            {
                ServicesList.Add(item);
            }
        }

        private async Task LoadPayments()
        {
            var items = await _localDBService.GetItems<PaymentModel>();
            var needItems = items.Where(x => x.ReservationId == this.ReservationId);
            PaymentsList.Clear();
            foreach (var item in needItems)
            {
                PaymentsList.Add(item);
            }
        }

        private async Task<int> GetReservationId()
        {
            if (ReservationId == 0)
            {
                var items = await _localDBService.GetItems<ReservationModel>();
                if (items.Count == 0)
                {
                    return 1;
                }
                else
                {
                    return items.Max(x => x.Id) + 1;
                }
            }
            return ReservationId;
        }

        private void SetDefault()
        {
            SelectedSegmentIndex = 0;
            ClientName = null;
            ClientPhone = null;
            InDate = DateTime.Today;
            OutDate = DateTime.Today.AddDays(1);
            InTime = TimeSpan.Parse("13:00");
            OutTime = TimeSpan.Parse("12:00");
            DaysCount = 1;
            SumPerDay = 2000.00f;
            TotalSum = DaysCount * SumPerDay;
            Comment = null;

            PeopleAmount = 1;
            ChildAmount = 0;
        }



        // AUTO COMPLETE DATA
        partial void OnReservationTypeChanged(string value)
        {
            if (ReservationType == "Краткосрочное")
            {
                InDate = DateTime.Today;
                OutDate = DateTime.Today.AddDays(1);
                SumPerDay = 2000.00f;
            }
            if (ReservationType == "Долгосрочное")
            {
                InDate = DateTime.Today;
                OutDate = DateTime.Today.AddMonths(1);
                SumPerDay = 20000.00f;
            }
        }

        partial void OnInDateChanged(DateTime value)
        {
            if (ReservationType == "Краткосрочное")
            {
                if (InDate >= OutDate)
                {
                    OutDate = InDate.AddDays(1);
                }
                DaysCount = (OutDate.Subtract(InDate)).Days;
            }

            if (ReservationType == "Долгосрочное")
            {
                if (InDate >= OutDate)
                {
                    OutDate = InDate.AddMonths(1);
                }
                DaysCount = (OutDate.Subtract(InDate)).Days / 30;
            }
        }

        partial void OnOutDateChanged(DateTime value)
        {
            if (ReservationType == "Краткосрочное")
            {
                if (InDate >= OutDate)
                {
                    OutDate = InDate.AddDays(1);
                }
                DaysCount = (OutDate.Subtract(InDate)).Days;
            }

            if (ReservationType == "Долгосрочное")
            {
                if (InDate >= OutDate)
                {
                    OutDate = InDate.AddMonths(1);
                }
                DaysCount = (OutDate.Subtract(InDate)).Days / 30;
            }
        }

        partial void OnDaysCountChanged(int value)
        {
            if (value == 0) DaysCount = 1;

            TotalSum = DaysCount * SumPerDay;
        }

        partial void OnSumPerDayChanged(float value)
        {
            TotalSum = DaysCount * SumPerDay;
        }



        private readonly IPopupService _popupService;
        private readonly LocalDBService _localDBService;
        public AddReservationPageViewModel(IPopupService popupService, LocalDBService localDBService)
        {
            _popupService = popupService;
            _localDBService = localDBService;
            SetDefault();
            Task.Run(async () => await Load());
            LoadPaymentsSum();
            LoadServicesSum();
        }
    }
}