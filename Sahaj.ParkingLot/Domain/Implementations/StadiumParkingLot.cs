namespace Sahaj.ParkingLot.Domain.Implementations
{
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Contracts;

    public class StadiumParkingLot : BaseParkingLot
    {
        public StadiumParkingLot(IParkingLotRepository parkingLotRepository) : base(parkingLotRepository, ParkingLotType.Stadium)
        {
        }
    }
}
