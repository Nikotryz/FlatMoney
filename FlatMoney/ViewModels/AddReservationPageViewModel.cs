using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using System.Collections.ObjectModel;

namespace FlatMoney.ViewModels
{
    public partial class AddReservationPageViewModel : ObservableObject
    {
        //Page 1
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
        public TimeSpan inTime;
        [ObservableProperty]
        public TimeSpan outTime;

        [ObservableProperty]
        public int daysCount;
        [ObservableProperty]
        public double sumPerDay;
        [ObservableProperty]
        public double totalSum;

        [ObservableProperty]
        public ObservableCollection<string> flatsList = [];
        [ObservableProperty]
        public FlatModel? selectedFlat;

        [ObservableProperty]
        public string comment;


        //Page 2
        [ObservableProperty]
        public int peopleAmount;
        [ObservableProperty]
        public int childAmount;

        [ObservableProperty]
        public ObservableCollection<string> clientsList = [];


        //Page 3
        [ObservableProperty]
        public ObservableCollection<string> servicesList = [];


        //Page 4
        [ObservableProperty]
        public ObservableCollection<string> paymentsList = [];



        [RelayCommand]
        public async Task Delete()
        {
            //if (FlatInfo is not null) await _localDBService.DeleteItem(FlatInfo);

            await Shell.Current.GoToAsync("..");

            //await SetDefault();
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");

            SetDefault();
        }

        [RelayCommand]
        public async Task Save()
        {
            //if (FlatInfo is not null) await Update();
            //else await Create();

            await Shell.Current.GoToAsync("..");

            //await SetDefault();
        }



        private async Task Load()
        {
            var items = await _localDBService.GetItems<FlatModel>();
            FlatsList.Clear();
            foreach (var item in items)
            {
                FlatsList.Add(item.Name);
            }
        }

        private void SetDefault()
        {
            SelectedSegmentIndex = 0;
            ClientName = string.Empty;
            ClientPhone = string.Empty;
            InDate = DateTime.Today;
            OutDate = DateTime.Today.AddDays(1);
            InTime = TimeSpan.Parse("13:00");
            OutTime = TimeSpan.Parse("12:00");
            DaysCount = 1;
            SumPerDay = 2000.00;
            TotalSum = DaysCount * SumPerDay;
            Comment = string.Empty;

            PeopleAmount = 1;
            ChildAmount = 0;
        }

        partial void OnInDateChanged(DateTime value)
        {
            if (InDate >= OutDate) OutDate = InDate.AddDays(1);

            DaysCount = (OutDate.Subtract(InDate)).Days;
        }

        partial void OnOutDateChanged(DateTime value)
        {
            if (InDate >= OutDate) InDate = OutDate.AddDays(-1);

            DaysCount = (OutDate.Subtract(InDate)).Days;
        }

        partial void OnDaysCountChanged(int value)
        {
            if (value == 0) DaysCount = 1;

            OutDate = InDate.AddDays(DaysCount);

            TotalSum = DaysCount * SumPerDay;
        }

        partial void OnSumPerDayChanged(double value)
        {
            TotalSum = DaysCount * SumPerDay;
        }



        private readonly LocalDBService _localDBService;
        public AddReservationPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
            SetDefault();
            Load();
        }
    }
}
