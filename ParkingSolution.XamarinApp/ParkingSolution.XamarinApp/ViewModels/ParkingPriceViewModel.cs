using ParkingSolution.XamarinApp.Models.Helpers;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.ObjectModel;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class ParkingPriceViewModel : BaseViewModel
    {
        private ObservableCollection<PriceHelper> prices;
        private SerializedParking parking;

        public ParkingPriceViewModel(SerializedParking parking)
        {
            Prices = new ObservableCollection<PriceHelper>();
            Parking = parking;
        }

        public ObservableCollection<PriceHelper> Prices
        {
            get => prices;
            set => SetProperty(ref prices, value);
        }
        public SerializedParking Parking
        {
            get => parking;
            set => SetProperty(ref parking, value);
        }

        internal void OnAppearing()
        {
            LoadPrices();
        }

        private void LoadPrices()
        {
            for (TimeSpan i = TimeSpan.Zero;
                i < TimeSpan.FromHours(24);
                i += TimeSpan.FromHours(1))
            {
                if (i < Parking.BeforePaidTime)
                {
                    Prices.Add(new PriceHelper
                    {
                        Color = Xamarin.Forms.Color.Green,
                        Time = i,
                        PriceInRubles = 0
                    });
                }
                else if (i < Parking.BeforeFreeTime)
                {
                    Prices.Add(new PriceHelper
                    {
                        Color = Xamarin.Forms.Color.Red,
                        Time = i,
                        PriceInRubles = Parking.CostInRubles
                    });
                }
                else
                {
                    Prices.Add(new PriceHelper
                    {
                        Color = Xamarin.Forms.Color.Green,
                        Time = i,
                        PriceInRubles = 0
                    });
                }
            }
        }
    }
}