namespace ParkingReservation.Core.Interfaces
{
    public interface IBookable
    {
        string ItemReference { get; }
        string ItemType { get; }
    }
}
