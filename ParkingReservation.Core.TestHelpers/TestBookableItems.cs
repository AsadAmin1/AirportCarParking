using ParkingReservation.Core.Interfaces;
using ParkingReservation.Core.Models;

namespace ParkingReservation.Core.TestHelpers
{
    public class TestBookableItems
    {
        public static readonly List<IBookable> Items = new (
            Enumerable.Range(1, 10).ToList().Select(i => new CarParkingSpot(i.ToString())).ToList()
           );
    }
}
