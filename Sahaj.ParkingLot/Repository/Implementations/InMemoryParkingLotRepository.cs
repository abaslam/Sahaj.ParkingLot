namespace Sahaj.ParkingLot.Repository.Implementations
{
    using Sahaj.ParkingLot.Entities;
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Contracts;

    public class InMemoryParkingLotRepository : IParkingLotRepository
    {
        private int currentReceiptNumber;
        private int currentTicketNumber;

        private bool canParkMotorCycles;
        private bool canParkCarSUVs;
        private bool canParkBusTrucks;

        private HashSet<ParkingSpot> parkingSpots = new();
        private Dictionary<ParkingLotType, Dictionary<VehicleType, HashSet<ParkingFee>>> parkingFees = new();

        public InMemoryParkingLotRepository(int numberOfMotorCycleParkings, int numberOfCarSUVParkings, int numberOfBusTruckParkings)
        {
            this.Initialize(numberOfMotorCycleParkings, numberOfCarSUVParkings, numberOfBusTruckParkings);
        }

        public ParkingSpot? GetAvailableParkingSpot(VehicleType vehicleType)
        {
            return this.parkingSpots.FirstOrDefault(x => x.VehicleType == vehicleType && x.IsEmpty);
        }

        public string GetNextReceiptNumber()
        {
            return $"R-{this.currentReceiptNumber++:D3}";
        }

        public string GetNextTicketNumber()
        {
            return $"{this.currentTicketNumber++:D3}";
        }

        public ParkingFee GetParkingFee(ParkingLotType parkingLotType, VehicleType vehicleType, double hours)
        {
            return this.parkingFees[parkingLotType][vehicleType].First(x => x.MinHours <= hours && hours < x.MaxHours);
        }

        public ParkingSpot? GetParkingSpot(ParkingTicket parkingTicket)
        {
            return this.parkingSpots.FirstOrDefault(x => x.ParkingTicket == parkingTicket);
        }

        public bool IsParkingAllowed(ParkingLotType parkingLotType, VehicleType vehicleType)
        {
            return vehicleType == VehicleType.Motorcyle && this.canParkMotorCycles ||
                   vehicleType == VehicleType.CarSUV && this.canParkCarSUVs ||
                   vehicleType == VehicleType.BusTruck && this.canParkBusTrucks;
        }

        private void Initialize(int numberOfMotorCycleParkings, int numberOfCarSUVParkings, int numberOfBusTruckParkings)
        {
            this.currentReceiptNumber = 1;
            this.currentTicketNumber = 1;

            this.canParkMotorCycles = numberOfMotorCycleParkings > 0;
            this.canParkCarSUVs = numberOfCarSUVParkings > 0;
            this.canParkBusTrucks = numberOfBusTruckParkings > 0;

            this.InitializeParkingSpots(numberOfMotorCycleParkings, numberOfCarSUVParkings, numberOfBusTruckParkings);
            this.InitializeParkingFees();
        }

        private void InitializeParkingFees()
        {
            var mallParkingFees = new Dictionary<VehicleType, HashSet<ParkingFee>>();

            mallParkingFees.Add(
                VehicleType.Motorcyle,
                new() { new(0, double.MaxValue, 10, FeeCalculationType.PerHour) });

            mallParkingFees.Add(
                VehicleType.CarSUV,
                new() { new(0, double.MaxValue, 20, FeeCalculationType.PerHour) });

            mallParkingFees.Add(
                VehicleType.BusTruck,
                new() { new(0, double.MaxValue, 50, FeeCalculationType.PerHour) });

            this.parkingFees.Add(ParkingLotType.Mall, mallParkingFees);

            var stadiumParkingFees = new Dictionary<VehicleType, HashSet<ParkingFee>>();

            var motorCycleFee1 = new ParkingFee(0, 4, 30, FeeCalculationType.FlatRate);
            var motorCycleFee2 = new ParkingFee(4, 12, 60, FeeCalculationType.FlatRate, motorCycleFee1);
            var motorCycleFee3 = new ParkingFee(12, double.MaxValue, 100, FeeCalculationType.PerHour, motorCycleFee2);

            stadiumParkingFees.Add(
                VehicleType.Motorcyle,
                new()
                {
                    motorCycleFee1,
                    motorCycleFee2,
                    motorCycleFee3
                });

            var carSUVFee1= new ParkingFee(0, 4, 60, FeeCalculationType.FlatRate);
            var carSUVFee2 = new ParkingFee(4, 12, 120, FeeCalculationType.FlatRate, carSUVFee1);
            var carSUVFee3 = new ParkingFee(12, double.MaxValue, 200, FeeCalculationType.PerHour, carSUVFee2);

            stadiumParkingFees.Add(
                VehicleType.CarSUV,
                new()
                {
                    carSUVFee1,
                    carSUVFee2,
                    carSUVFee3
                });

            this.parkingFees.Add(ParkingLotType.Stadium, stadiumParkingFees);

            var airportParkingFees = new Dictionary<VehicleType, HashSet<ParkingFee>>();
            airportParkingFees.Add(
                VehicleType.Motorcyle,
                new()
                {
                    new(0, 1, 0, FeeCalculationType.FlatRate),
                    new(1, 8, 40, FeeCalculationType.FlatRate),
                    new(8, 24, 60, FeeCalculationType.FlatRate),
                    new(24, double.MaxValue, 80, FeeCalculationType.PerDay)
                });

            airportParkingFees.Add(
                VehicleType.CarSUV,
                new()
                {
                    new(0,12,60, FeeCalculationType.FlatRate),
                    new(12, 24, 80, FeeCalculationType.FlatRate),
                    new(24, double.MaxValue, 100, FeeCalculationType.PerDay)
                }); ;

            this.parkingFees.Add(ParkingLotType.Airport, airportParkingFees);
        }

        private void InitializeParkingSpots(int numberOfMotorCycleParkings, int numberOfCarSUVParkings, int numberOfBusTruckParkings)
        {
            var parkingSpotNumber = 1;

            if (numberOfMotorCycleParkings > 0)
            {
                parkingSpotNumber = this.AddParkingSpots(numberOfMotorCycleParkings, parkingSpotNumber, VehicleType.Motorcyle);
            }

            if (numberOfCarSUVParkings > 0)
            {
                parkingSpotNumber = this.AddParkingSpots(numberOfCarSUVParkings, parkingSpotNumber, VehicleType.CarSUV);
            }

            if (numberOfBusTruckParkings > 0)
            {
                this.AddParkingSpots(numberOfBusTruckParkings, parkingSpotNumber, VehicleType.BusTruck);
            }
        }

        private int AddParkingSpots(int numberOfParkings, int parkingSpotNumber, VehicleType vehicleType)
        {
            for (int i = 0; i < numberOfParkings; i++)
            {
                this.parkingSpots.Add(new ParkingSpot(parkingSpotNumber++, vehicleType));
            }

            return parkingSpotNumber;
        }
    }
}
