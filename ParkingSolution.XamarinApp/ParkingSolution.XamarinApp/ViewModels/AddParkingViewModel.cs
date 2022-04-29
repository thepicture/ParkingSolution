using ParkingSolution.XamarinApp.Models.Helpers;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class AddParkingViewModel : BaseViewModel
    {
        private ObservableCollection<ParkingTypeHelper> parkingPlaces;

        public AddParkingViewModel()
        {
            ParkingPlaces = new ObservableCollection<ParkingTypeHelper>();
        }

        public AddParkingViewModel(SerializedParking serializedParking)
        {
            ParkingPlaces = new ObservableCollection<ParkingTypeHelper>();
            EditingParking = serializedParking;
            if (EditingParking.ParkingPlacesCarTypes != null)
            {
                foreach (string parkingPlaceCarType in EditingParking.ParkingPlacesCarTypes)
                {
                    ParkingTypeHelper parkingTypeHelper = new ParkingTypeHelper
                    {
                        Name = parkingPlaceCarType
                    };
                    if (parkingPlaceCarType == "A") parkingTypeHelper.Id = 1;
                    if (parkingPlaceCarType == "B") parkingTypeHelper.Id = 2;
                    if (parkingPlaceCarType == "C") parkingTypeHelper.Id = 3;
                    if (parkingPlaceCarType == "D") parkingTypeHelper.Id = 4;
                    if (parkingPlaceCarType == "E") parkingTypeHelper.Id = 5;
                    ParkingPlaces.Add(parkingTypeHelper);
                }
            }

            City = EditingParking.City;
            Street = EditingParking.Street;
            ParkingType = EditingParking.ParkingType;
            BeforePaidTime = EditingParking.BeforePaidTime;
            BeforeFreeTime = EditingParking.BeforeFreeTime;
            CostInRubles = EditingParking.CostInRubles.ToString("F0");
        }

        public ObservableCollection<ParkingTypeHelper> ParkingPlaces
        {
            get => parkingPlaces;
            set => SetProperty(ref parkingPlaces, value);
        }

        private string parkingType;

        public string ParkingType
        {
            get => parkingType;
            set => SetProperty(ref parkingType, value);
        }

        private string city;

        public string City
        {
            get => city;
            set => SetProperty(ref city, value);
        }

        private string street;

        public string Street
        {
            get => street;
            set => SetProperty(ref street, value);
        }

        private Command saveChangesCommand;

        public ICommand SaveChangesCommand
        {
            get
            {
                if (saveChangesCommand == null)
                {
                    saveChangesCommand = new Command(SaveChangesAsync);
                }

                return saveChangesCommand;
            }
        }

        private async void SaveChangesAsync()
        {
            IsBusy = true;
            SerializedParking parking = new SerializedParking();
            if (EditingParking != null)
            {
                parking = EditingParking;
            }
            parking.City = City;
            parking.Street = Street;
            if (ParkingType != null)
            {
                parking.ParkingTypeId = ParkingType == "Придорожная" ? 1 : 2;
            }
            parking.BeforePaidTime = BeforePaidTime;
            parking.BeforeFreeTime = BeforeFreeTime;
            parking.CostInRublesAsString = CostInRubles;
            parking.ParkingPlacesCarTypes = ParkingPlaces?.Select(pp => pp.Name);
            if (await ParkingDataStore.AddItemAsync(parking))
            {
                await Shell.Current.GoToAsync("..");
            }
            IsBusy = false;
        }

        private TimeSpan beforePaidTime = TimeSpan.FromHours(12);

        public TimeSpan BeforePaidTime
        {
            get => beforePaidTime;
            set => SetProperty(ref beforePaidTime, value);
        }

        private TimeSpan beforeFreeTime = TimeSpan.FromHours(20);

        public TimeSpan BeforeFreeTime
        {
            get => beforeFreeTime;
            set => SetProperty(ref beforeFreeTime, value);
        }

        private ParkingTypeHelper parkingPlaceType;

        public ParkingTypeHelper ParkingPlaceType
        {
            get => parkingPlaceType;
            set => SetProperty(ref parkingPlaceType, value);
        }

        private Command addParkingPlaceCommand;

        public ICommand AddParkingPlaceCommand
        {
            get
            {
                if (addParkingPlaceCommand == null)
                {
                    addParkingPlaceCommand = new Command(AddParkingPlaceAsync);
                }

                return addParkingPlaceCommand;
            }
        }

        private async void AddParkingPlaceAsync()
        {
            if (ParkingPlaceType == null)
            {
                await FeedbackService.InformError("Выберите " +
                    "тип парковочного места");
                return;
            }
            ParkingPlaces.Add(ParkingPlaceType);
            await FeedbackService.Inform("Парковочное место " +
                "добавлено локально. " +
                "Оно будет сохранено с парковкой");
        }

        private string costInRubles;
        private SerializedParking editingParking;

        public string CostInRubles
        {
            get => costInRubles;
            set => SetProperty(ref costInRubles, value);
        }
        public SerializedParking EditingParking
        {
            get => editingParking;
            set => SetProperty(ref editingParking, value);
        }

        private Command deleteParkingCommand;

        public ICommand DeleteParkingCommand
        {
            get
            {
                if (deleteParkingCommand == null)
                {
                    deleteParkingCommand = new Command(DeleteParkingAsync);
                }

                return deleteParkingCommand;
            }
        }

        private async void DeleteParkingAsync()
        {
            string id = EditingParking.Id.ToString();
            if (await ParkingDataStore.DeleteItemAsync(id))
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        public bool IsEditing => EditingParking != null;
    }
}