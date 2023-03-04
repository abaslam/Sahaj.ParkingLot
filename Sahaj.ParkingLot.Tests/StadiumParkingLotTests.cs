namespace Sahaj.ParkingLot.Tests
{
    using NUnit.Framework;
    using Sahaj.ParkingLot.Domain.Implementations;
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Implementations;
    using Sahaj.ParkingLot.Utilities;

    [TestFixture]
    public class StadiumParkingLotTests
    {
        private StadiumParkingLot stadiumParkingLot;
        private DateTime timeNow;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.stadiumParkingLot = new StadiumParkingLot(new InMemoryParkingLotRepository(1000, 1500, 0));
            this.timeNow = SystemTime.Now();
        }

        [TestCaseSource(nameof(StadiumParkingLotTestScenarios))]
        public void Parking_Various_Vehicles_For_Various_Durations_Should_Succeed_With_Expected_Fees(VehicleType vehicle, TimeSpan duration, double expectedFees)
        {
            SystemTime.Now = () => timeNow;

            var parkingTicket = this.stadiumParkingLot.Park(vehicle);

            Assert.That(parkingTicket.IsSuccess, Is.True);
            Assert.That(parkingTicket.Data, Is.Not.Null);

            SystemTime.Now = () => timeNow.Add(duration);

            var parkingReceipt = this.stadiumParkingLot.Unpark(parkingTicket.Data);

            Assert.That(parkingReceipt.IsSuccess, Is.True);
            Assert.That(parkingReceipt.Data, Is.Not.Null);
            Assert.That(parkingReceipt.Data?.Fees, Is.EqualTo(expectedFees));
        }

        public static object[] StadiumParkingLotTestScenarios =
        {
            new object[]{ VehicleType.Motorcyle, new TimeSpan(0, 3, 40, 0), 30 },
            new object[]{ VehicleType.Motorcyle, new TimeSpan(0, 14, 59, 0), 390 },
            new object[]{ VehicleType.CarSUV, new TimeSpan(0, 11, 30, 0), 180 },
            new object[]{ VehicleType.CarSUV, new TimeSpan(0, 13, 5, 0), 580 },
        };
    }
}
