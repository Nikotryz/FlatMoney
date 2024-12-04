using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using FlatMoney.DisplayHelper;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UraniumUI;
using UraniumUI.Material.Controls;

namespace FlatMoney.ViewModels
{
    public partial class AddReservationPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<string> flatsList = [];

        [ObservableProperty]
        public double columnWidth = MauiDisplayHelper.UnitWidth / 4;



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


        private LocalDBService _localDBService;
        public AddReservationPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;
            SetDefault();
            Load();
        }

        [ObservableProperty]
        private bool isRefreshing = false;

        [RelayCommand]
        private async Task Refresh()
        {
            await Task.Delay(300);
            await Load();
            IsRefreshing = false;
        }

        [RelayCommand]
        private async Task Delete()
        {
            //if (FlatInfo is not null) await _localDBService.DeleteItem(FlatInfo);

            await Shell.Current.GoToAsync("..");

            //await SetDefault();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");

            //await SetDefault();
        }

        [RelayCommand]
        private async Task Save()
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
            InDate = DateTime.Today;
            OutDate = DateTime.Today.AddDays(1);
            InTime = TimeSpan.Parse("13:00");
            OutTime = TimeSpan.Parse("12:00");
            DaysCount = 1;
            SumPerDay = 2000.00;
            TotalSum = DaysCount * SumPerDay;
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
            OutDate = InDate.AddDays(DaysCount);

            TotalSum = DaysCount * SumPerDay;
        }

        partial void OnSumPerDayChanged(double value)
        {
            TotalSum = DaysCount * SumPerDay;
        }
    }
}
