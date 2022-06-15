using ParkingSolution.XamarinApp.Models.Helpers;
using ParkingSolution.XamarinApp.Models.Serialized;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace ParkingSolution.XamarinApp.Services
{
    public class ParkingListToPositionConverter : IListConverter<SerializedParking, ParkingHelper>
    {
        public async Task<IEnumerable<ParkingHelper>> ConvertAllAsync(IEnumerable<SerializedParking> parkings)
        {
            Queue<SerializedParking> queue = new Queue<SerializedParking>(parkings);

            List<ParkingHelper> parkingHelpers = new List<ParkingHelper>();
            while (queue.Count > 0)
            {
                try
                {
                    ParkingHelper currentPositionedParking = await CreateParkingHelperAsync(queue.Peek());
                    parkingHelpers.Add(currentPositionedParking);
                    _ = queue.Dequeue();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Can't create a parking helper for id " + queue.Peek().Id + ", " + ex);
                }
            }
            return parkingHelpers;
        }

        private async Task<ParkingHelper> CreateParkingHelperAsync(SerializedParking serializedParking)
        {
            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations =
                await geoCoder
                .GetPositionsForAddressAsync(
                    string.Format(serializedParking.Address));
            Position position = approximateLocations.FirstOrDefault();
            ParkingHelper parkingHelper = new ParkingHelper
            {
                Address = serializedParking.Address,
                Description = serializedParking.ParkingType,
                Position = position,
                Parking = serializedParking
            };
            return parkingHelper;
        }
    }
}
