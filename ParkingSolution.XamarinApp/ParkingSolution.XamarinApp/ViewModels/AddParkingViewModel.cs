using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class AddParkingViewModel : BaseViewModel
    {
        private ObservableCollection<string> parkingPlaces;

        public AddParkingViewModel()
        {
            ParkingPlaces = new ObservableCollection<string>();
        }

        public ObservableCollection<string> ParkingPlaces
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
            if (BeforePaidTime >= beforeFreeTime)
            {
                _ = validationErrors.AppendLine("Дата начала " +
                    "платной парковки " +
                    "должна быть раньше " +
                    "даты окончания платной парковки");
            }

            if (validationErrors.Length > 0)
            {
                await FeedbackService.InformError(
                    validationErrors.ToString());
                IsBusy = false;
                return;
            }
            IsBusy = true;
            SerializedParking parking = new SerializedParking
            {
                City = City,
                Street = Street,
                ParkingTypeId = ParkingType == "Придорожная" ? 1 : 2,
                BeforePaidTime = BeforePaidTime,
                BeforeFreeTime = BeforeFreeTime,
                CostInRubles = decimal.Parse(CostInRubles),
                ParkingPlacesCarTypes = ParkingPlaces
            };
            if (await ParkingDataStore.AddItemAsync(parking))
            {
                await FeedbackService.Inform("Парковка добавлена");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await FeedbackService.Inform("Не удалось " +
                    "добавить парковку. " +
                    "Проверьте подключение к интернету");
            }
            IsBusy = false;
        }

        private TimeSpan beforePaidTime;

        public TimeSpan BeforePaidTime
        {
            get => beforePaidTime;
            set => SetProperty(ref beforePaidTime, value);
        }

        private System.TimeSpan beforeFreeTime;

        public TimeSpan BeforeFreeTime
        {
            get => beforeFreeTime;
            set => SetProperty(ref beforeFreeTime, value);
        }

        private string parkingPlaceType;

        public string ParkingPlaceType
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
                    addParkingPlaceCommand = new Command(AddParkingPlace);
                }

                return addParkingPlaceCommand;
            }
        }

        private void AddParkingPlace()
        {
            if (ParkingPlaceType == null)
            {
                FeedbackService.InformError("Вы не " +
                    "выбрали тип парковочного места");
                return;
            }
            ParkingPlaces.Add(ParkingPlaceType);
            FeedbackService.Inform("Парковочное место " +
                "добавлено локально. " +
                "Оно будет сохранено с парковкой");
        }

        private string costInRubles;

        public string CostInRubles
        {
            get => costInRubles;
            set => SetProperty(ref costInRubles, value);
        }
    }
}