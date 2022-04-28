using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        bool isRefreshing;
        bool isBusy = false;
        public IDataStore<SerializedUserCar> CarDataStore =>
            DependencyService.Get<IDataStore<SerializedUserCar>>();
        public IDataStore<SerializedParking> ParkingDataStore =>
            DependencyService.Get<IDataStore<SerializedParking>>();
        public IDataStore<SerializedParkingPlaceReservation> ReservationDataStore =>
            DependencyService.Get<IDataStore<SerializedParkingPlaceReservation>>();
        public IDataStore<SerializedPaymentHistory> PaymentHistoryDataStore =>
            DependencyService.Get<IDataStore<SerializedPaymentHistory>>();
        public IDataStore<SerializedUser> UserDataStore =>
            DependencyService.Get<IDataStore<SerializedUser>>();
        public IFeedbackService FeedbackService =>
            DependencyService.Get<IFeedbackService>();
        public IDataStore<SerializedLoginUser> LoginDataStore =>
            DependencyService.Get<IDataStore<SerializedLoginUser>>();
        public IRegistrationService<SerializedUser> RegistrationService =>
           DependencyService.Get<IRegistrationService<SerializedUser>>();
        public string Role => AppIdentity.Role;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler changed = PropertyChanged;

            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
