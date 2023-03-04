namespace Sahaj.ParkingLot.Entities
{
    using Sahaj.ParkingLot.Enums;
    public class ParkingSpot
    {
        public ParkingSpot(int spotNumber, VehicleType vehicleType)
        {
            SpotNumber = spotNumber;
            VehicleType = vehicleType;
        }

        public int SpotNumber { get; }
        public VehicleType VehicleType { get; }
        public ParkingTicket? ParkingTicket { get; private set; }
        public bool IsEmpty => ParkingTicket == null;

        public void Occupy(ParkingTicket parkingTicket)
        {
            this.ParkingTicket = parkingTicket;
        }

        public void Vacate()
        {
            this.ParkingTicket = null;
        }
    }
}
