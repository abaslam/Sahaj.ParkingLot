namespace Sahaj.ParkingLot.Domain.Implementations
{
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Contracts;

    public class MallParkingLot : BaseParkingLot
    {
        public MallParkingLot(IParkingLotRepository parkingLotRepository) : base(parkingLotRepository, ParkingLotType.Mall)
        {
        }
    }
}
