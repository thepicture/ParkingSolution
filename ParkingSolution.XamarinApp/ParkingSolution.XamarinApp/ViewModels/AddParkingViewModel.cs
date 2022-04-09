using ParkingSolution.XamarinApp.Models.Helpers;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        internal void OnAppearing()
        {
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
            StringBuilder validationErrors = new StringBuilder();
            if (ParkingType == null)
            {
                _ = validationErrors.AppendLine("Выберите тип " +
                    "парковки");
            }
            if (string.IsNullOrWhiteSpace(City))
            {
                _ = validationErrors.AppendLine("Укажите город");
            }
            if (string.IsNullOrWhiteSpace(Street))
            {
                _ = validationErrors.AppendLine("Укажите улицу");
            }
            if (string.IsNullOrWhiteSpace(CostInRubles)
                || !decimal.TryParse(CostInRubles, out decimal price)
                || price <= 0)
            {
                _ = validationErrors.AppendLine("Стоимость должна " +
                    "быть положительной и в рублях");
            }
            if (BeforePaidTime >= BeforeFreeTime)
            {
                _ = validationErrors.AppendLine("Дата начала " +
                    "платной парковки " +
                    "должна быть раньше " +
                    "даты окончания платной парковки");
            }
            if (ParkingPlaces.Count == 0)
            {
                _ = validationErrors.AppendLine("Необходимо " +
                    "хотя бы одно парковочное место");
            }

            if (validationErrors.Length > 0)
            {
                await FeedbackService.InformError(
                    validationErrors.ToString());
                IsBusy = false;
                return;
            }

            IsBusy = true;
            SerializedParking parking = null;
            if (EditingParking != null)
            {
                EditingParking.City = City;
                EditingParking.Street = Street;
                EditingParking.ParkingTypeId = ParkingType == "Придорожная" ? 1 : 2;
                EditingParking.BeforePaidTime = BeforePaidTime;
                EditingParking.BeforeFreeTime = BeforeFreeTime;
                EditingParking.CostInRubles = decimal.Parse(CostInRubles);
                EditingParking.ParkingPlacesCarTypes = ParkingPlaces.Select(pp => pp.Name);
                parking = EditingParking;
            }
            else
            {
                parking = new SerializedParking
                {
                    City = City,
                    Street = Street,
                    ParkingTypeId = ParkingType == "Придорожная" ? 1 : 2,
                    BeforePaidTime = BeforePaidTime,
                    BeforeFreeTime = BeforeFreeTime,
                    CostInRubles = decimal.Parse(CostInRubles),
                    ParkingPlacesCarTypes = ParkingPlaces.Select(pp => pp.Name)
                };
            }

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
            if (await FeedbackService.Ask("Удалить парковку?"))
            {
                string id = EditingParking.Id.ToString();
                if (await ParkingDataStore.DeleteItemAsync(id))
                {
                    await Shell.Current.GoToAsync("..");
                }
            }
        }

        public bool IsEditing => EditingParking != null;
    }
}