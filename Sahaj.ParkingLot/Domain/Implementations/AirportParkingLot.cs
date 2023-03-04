namespace Sahaj.ParkingLot.Domain.Implementations
{
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Contracts;

    public class AirportParkingLot : BaseParkingLot
    {
        public AirportParkingLot(IParkingLotRepository parkingLotRepository) : base(parkingLotRepository, ParkingLotType.Airport)
        {
        }
    }
}
