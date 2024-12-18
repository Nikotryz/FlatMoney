using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Utils.About;
using FlatMoney.LocalDataBase;
using FlatMoney.Models;
using FlatMoney.Views.Details;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Linq;
using UraniumUI.Material.Controls;

namespace FlatMoney.ViewModels
{
    public partial class ClientPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<ReservationGroup> shortReservations;

        [ObservableProperty]
        public ObservableCollection<ReservationGroup> longReservations;

        [ObservableProperty]
        public ReservationModel? selectedShortReservation;

        [ObservableProperty]
        public ReservationModel? selectedLongReservation;



        [RelayCommand]
        public async Task AddShortReservation()
        {
            var parameters = new Dictionary<string, object?>()
            {
                {"type", "Краткосрочное" }
            };

            await Shell.Current.GoToAsync(nameof(AddReservationPage), true, parameters);
        }

        [RelayCommand]
        public async Task AddLongReservation()
        {
            var parameters = new Dictionary<string, object?>()
            {
                {"type", "Долгосрочное" }
            };

            await Shell.Current.GoToAsync(nameof(AddReservationPage), true, parameters);
        }

        [RelayCommand]
        public async Task ShortTapped()
        {
            var flats = await _localDBService.GetItems<FlatModel>();

            int flatId;

            if (SelectedShortReservation?.FlatId is null)
            {
                flatId = 0;
            }
            else
            {
                flatId = flats.Where(x => x.Id == (SelectedShortReservation.FlatId)).First().Id;
            }

            TimeSpan? inTime = TimeSpan.Parse($"{TimeOnly.FromDateTime(SelectedShortReservation!.CheckInDate)}");
            TimeSpan? outTime = TimeSpan.Parse($"{TimeOnly.FromDateTime(SelectedShortReservation!.CheckOutDate)}");

            var parameters = new Dictionary<string, object?>()
            {
                {"info", SelectedShortReservation},
                {"id", SelectedShortReservation.Id},
                {"type", "Краткосрочное" },
                {"flatid", flatId},
                {"indate", SelectedShortReservation?.CheckInDate},
                {"outdate", SelectedShortReservation?.CheckOutDate},
                {"people", SelectedShortReservation?.PeopleAmount},
                {"child", SelectedShortReservation?.ChildAmount},
                {"days", SelectedShortReservation?.DaysOrMonthsAmount},
                {"sum", SelectedShortReservation?.CostPerAmount},
                {"status", FormatStatus(SelectedShortReservation?.ReservationStatus!)},
                {"comment", SelectedShortReservation?.Comment}
            };

            await Shell.Current.GoToAsync(nameof(AddReservationPage), true, parameters);

            SelectedShortReservation = null;
        }

        [RelayCommand]
        public async Task LongTapped()
        {
            var flats = await _localDBService.GetItems<FlatModel>();

            var parameters = new Dictionary<string, object?>()
            {
                {"info", SelectedLongReservation},
                {"id", SelectedLongReservation!.Id},
                {"type", "Долгосрочное" },
                {"flatid", flats.Where(x => x.Id == SelectedLongReservation?.FlatId).First().Id},
                {"indate", SelectedLongReservation?.CheckInDate},
                {"outdate", SelectedLongReservation?.CheckOutDate},
                {"people", SelectedLongReservation?.PeopleAmount},
                {"child", SelectedLongReservation?.ChildAmount},
                {"days", SelectedLongReservation?.DaysOrMonthsAmount},
                {"sum", SelectedLongReservation?.CostPerAmount},
                {"status", FormatStatus(SelectedLongReservation?.ReservationStatus!)},
                {"comment", SelectedLongReservation?.Comment}
            };

            await Shell.Current.GoToAsync(nameof(AddReservationPage), true, parameters);

            SelectedLongReservation = null;
        }

        private int FormatStatus(string status)
        {
            switch (status) 
            {
                case "Бронь": return 0;
                case "Заселен": return 1;
                case "Выехал": return 2;
            }
            return 0;
        }



        public async Task Load()
        {
            var items = await _localDBService.GetItems<ReservationModel>();

            var shortItems = items.Where(x => x.Type == "Краткосрочное");

            var longItems = items.Where(x => x.Type == "Долгосрочное");

            ShortReservations = new ObservableCollection<ReservationGroup>(
                shortItems
                    .OrderBy(x => x.ReservationStatus)
                    .GroupBy(x => x.ReservationStatus)
                    .Select(group => new ReservationGroup(group.Key!, group.OrderBy(x => x.FlatName)))
            );

            LongReservations = new ObservableCollection<ReservationGroup>(
                longItems
                    .OrderBy(x => x.ReservationStatus)
                    .GroupBy(x => x.ReservationStatus)
                    .Select(group => new ReservationGroup(group.Key!, group.OrderBy(x => x.FlatName)))
            );
        }



        private readonly LocalDBService _localDBService;
        public ClientPageViewModel(LocalDBService localDBService)
        {
            _localDBService = localDBService;

            Task.Run(async () => await Load());
        }
    }
}
