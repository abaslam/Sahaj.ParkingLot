namespace Sahaj.ParkingLot.Domain.Implementations
{
    using Sahaj.ParkingLot.Domain.Entities;
    using Sahaj.ParkingLot.Entities;
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Contracts;
    using Sahaj.ParkingLot.Utilities;

    public abstract class BaseParkingLot
    {
        private readonly IParkingLotRepository parkingLotRepository;

        public BaseParkingLot(IParkingLotRepository parkingLotRepository, ParkingLotType parkingLotType)
        {
            this.parkingLotRepository = parkingLotRepository;
            this.ParkingLotType = parkingLotType;
        }

        internal ParkingLotType ParkingLotType { get; }

        public Response<ParkingTicket> Park(VehicleType vehicleType)
        {
            if (!this.parkingLotRepository.IsParkingAllowed(this.ParkingLotType, vehicleType))
            {
                return new Response<ParkingTicket>($"The Vehicle Type-{vehicleType} is not allowed in this parking lot.");
            }

            var availableSpot = this.parkingLotRepository.GetAvailableParkingSpot(vehicleType);

            if (availableSpot is null)
            {
                return new Response<ParkingTicket>($"No empty parking spot is available for Vehicle Type-{vehicleType}.");
            }

            var parkingTicket = new ParkingTicket(this.parkingLotRepository.GetNextTicketNumber(), availableSpot.SpotNumber, SystemTime.Now());
            availableSpot.Occupy(parkingTicket);

            return new Response<ParkingTicket>(parkingTicket);
        }

        public Response<ParkingReceipt> Unpark(ParkingTicket parkingTicket)
        {
            var parkingSpot = this.parkingLotRepository.GetParkingSpot(parkingTicket);

            if (parkingSpot is null)
            {
                return new Response<ParkingReceipt>($"Invalid parking ticket-{parkingTicket.TicketNumber}");
            }

            parkingSpot.Vacate();

            var exitDateTime = SystemTime.Now();
            var totalHours = exitDateTime.Subtract(parkingTicket.EntryDateTime).TotalHours;
            var parkingFee = this.parkingLotRepository.GetParkingFee(this.ParkingLotType, parkingSpot.VehicleType, totalHours);

            var receipt = new ParkingReceipt(this.parkingLotRepository.GetNextReceiptNumber(), parkingTicket.EntryDateTime, exitDateTime, parkingFee.CalculateFee(totalHours));
            return new Response<ParkingReceipt>(receipt);
        }
    }
}
