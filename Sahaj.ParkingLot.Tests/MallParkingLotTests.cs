namespace Sahaj.ParkingLot.Tests
{
    using NUnit.Framework;
    using Sahaj.ParkingLot.Domain.Implementations;
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Implementations;
    using Sahaj.ParkingLot.Utilities;

    [TestFixture]
    public class MallParkingLotTests
    {
        private MallParkingLot mallParkingLot;
        private DateTime timeNow;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.mallParkingLot = new MallParkingLot(new InMemoryParkingLotRepository(100, 80, 10));
            this.timeNow = SystemTime.Now();
        }

        [TestCaseSource(nameof(MallParkingLotTestScenarios))]
        public void Parking_Various_Vehicles_For_Various_Durations_Should_Succeed_With_Expected_Fees(VehicleType vehicle, TimeSpan duration, double expectedFees)
        {
            SystemTime.Now = () => timeNow;

            var parkingTicket = this.mallParkingLot.Park(vehicle);

            Assert.That(parkingTicket.IsSuccess, Is.True);
            Assert.That(parkingTicket.Data, Is.Not.Null);

            SystemTime.Now = () => timeNow.Add(duration);

            var parkingReceipt = this.mallParkingLot.Unpark(parkingTicket.Data);

            Assert.That(parkingReceipt.IsSuccess, Is.True);
            Assert.That(parkingReceipt.Data, Is.Not.Null);
            Assert.That(parkingReceipt.Data.Fees, Is.EqualTo(expectedFees));
        }

        public static object[] MallParkingLotTestScenarios =
        {
            new object[]{ VehicleType.Motorcyle, new TimeSpan(0, 3, 30, 0), 40 },
            new object[]{ VehicleType.CarSUV, new TimeSpan(0, 6, 1, 0), 140 },
            new object[]{ VehicleType.BusTruck, new TimeSpan(0, 1, 59, 0), 100 }
        };
    }
}
