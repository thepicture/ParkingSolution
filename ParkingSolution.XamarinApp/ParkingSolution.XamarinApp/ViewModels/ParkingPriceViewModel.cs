using Microcharts;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.ObjectModel;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class ParkingPriceViewModel : BaseViewModel
    {
        private ObservableCollection<ChartEntry> prices;
        private SerializedParking parking;

        public ParkingPriceViewModel(SerializedParking parking)
        {
            Prices = new ObservableCollection<ChartEntry>();
            Parking = parking;
        }

        public ObservableCollection<ChartEntry> Prices
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

        public LineChart CurrentChart
        {
            get => currentChart;
            set => SetProperty(ref currentChart, value);
        }

        private LineChart currentChart = new LineChart();

        private void LoadPrices()
        {
            for (TimeSpan i = TimeSpan.Zero;
                i < TimeSpan.FromHours(24);
                i += TimeSpan.FromHours(1))
            {
                if (i < Parking.BeforePaidTime)
                {
                    Prices.Add(new ChartEntry(0)
                    {
                        Color = SkiaSharp.SKColor.Parse("#00FF00"),
                        Label = i.ToString(@"hh\:mm"),
                        ValueLabel = "0 руб."
                    });
                }
                else if (i < Parking.BeforeFreeTime)
                {
                    Prices.Add(new ChartEntry((int)Parking.CostInRubles)
                    {
                        Color = SkiaSharp.SKColor.Parse("#FF0000"),
                        Label = i.ToString(@"hh\:mm"),
                        ValueLabel = Parking.CostInRubles.ToString("F0") + "руб."
                    });
                }
                else
                {
                    Prices.Add(new ChartEntry(0)
                    {
                        Color = SkiaSharp.SKColor.Parse("#00FF00"),
                        Label = i.ToString(@"hh\:mm"),
                        ValueLabel = "0 руб."
                    });
                }
            }
            CurrentChart = new LineChart()
            {
                Entries = Prices,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Square,
                LabelTextSize = 40,
                PointSize = 18,
                IsAnimated = true
            };
        }
    }
}