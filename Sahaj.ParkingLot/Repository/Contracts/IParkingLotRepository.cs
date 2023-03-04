namespace Sahaj.ParkingLot.Repository.Contracts
{
    using Sahaj.ParkingLot.Entities;
    using Sahaj.ParkingLot.Enums;

    public interface IParkingLotRepository
    {
        string GetNextTicketNumber();
        string GetNextReceiptNumber();
        bool IsParkingAllowed(ParkingLotType parkingLotType, VehicleType vehicleType);
        ParkingSpot? GetAvailableParkingSpot(VehicleType vehicleType);
        ParkingSpot? GetParkingSpot(ParkingTicket parkingTicket);
        ParkingFee GetParkingFee(ParkingLotType parkingLotType, VehicleType vehicleType, double hours);
    }
}
