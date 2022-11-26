using ParkingReservation.Core.Interfaces;

namespace ParkingReservation.Core.Models
{
    public class CarParkingSpot : IBookable
    {
        #region Public Properties
        
        public string ItemReference { get; }

        public string ItemType { get => GetType().Name; }

        #endregion

        #region Constructors

        public CarParkingSpot(string itemReference)
        {
            ItemReference = itemReference;
        }

        #endregion

    }
}
