namespace Sahaj.ParkingLot.Tests
{
    using NUnit.Framework;
    using Sahaj.ParkingLot.Domain.Implementations;
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Implementations;
    using Sahaj.ParkingLot.Utilities;
    using System;

    [TestFixture]
    public class AirportParkingLotTests
    {
        private AirportParkingLot airportParkingLot;
        private DateTime timeNow;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.airportParkingLot = new AirportParkingLot(new InMemoryParkingLotRepository(200, 500, 100));
            this.timeNow = SystemTime.Now();
        }

        [TestCaseSource(nameof(AirportParkingLotTestScenarios))]
        public void Parking_Various_Vehicles_For_Various_Durations_Should_Succeed_With_Expected_Fees(VehicleType vehicle, TimeSpan duration, double expectedFees)
        {
            SystemTime.Now = () => timeNow;

            var parkingTicket = this.airportParkingLot.Park(vehicle);

            Assert.That(parkingTicket.IsSuccess, Is.True);
            Assert.That(parkingTicket.Data, Is.Not.Null);

            SystemTime.Now = () => timeNow.Add(duration);

            var parkingReceipt = this.airportParkingLot.Unpark(parkingTicket.Data);

            Assert.That(parkingReceipt.IsSuccess, Is.True);
            Assert.That(parkingReceipt.Data, Is.Not.Null);
            Assert.That(parkingReceipt.Data?.Fees, Is.EqualTo(expectedFees));
        }

        public static object[] AirportParkingLotTestScenarios =
        {
            new object[]{ VehicleType.Motorcyle, new TimeSpan(0, 0, 55, 0), 0 },
            new object[]{ VehicleType.Motorcyle, new TimeSpan(0, 14, 57, 0), 60 },
            new object[]{ VehicleType.Motorcyle, new TimeSpan(1, 12, 0, 0), 160 },
            new object[]{ VehicleType.CarSUV, new TimeSpan(0, 0, 50, 0), 60 },
            new object[]{ VehicleType.CarSUV, new TimeSpan(0, 23, 59, 0), 80 },
            new object[]{ VehicleType.CarSUV, new TimeSpan(3, 1, 0, 0), 400 },
        };
    }
}
