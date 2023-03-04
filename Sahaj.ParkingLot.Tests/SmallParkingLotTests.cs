namespace Sahaj.ParkingLot.Tests
{
    using NUnit.Framework;
    using Sahaj.ParkingLot.Domain.Entities;
    using Sahaj.ParkingLot.Domain.Implementations;
    using Sahaj.ParkingLot.Entities;
    using Sahaj.ParkingLot.Enums;
    using Sahaj.ParkingLot.Repository.Implementations;
    using Sahaj.ParkingLot.Utilities;

    [TestFixture]
    public class SmallParkingLotTests
    {
        private MallParkingLot smallParkingLot;
        private Response<ParkingTicket> motorCycleParkingTicket;
        private Response<ParkingTicket> scooterParkingTicket;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.smallParkingLot = new MallParkingLot(new InMemoryParkingLotRepository(2, 0, 0));
        }

        [Test, Order(0)]
        public void Scenario_1_Motor_Cycle_Parking_Should_Succeed()
        {
            SystemTime.Now = () => DateTime.Parse("29-May-2022 14:04:07");

            this.motorCycleParkingTicket = smallParkingLot.Park(VehicleType.Motorcyle);

            Assert.IsTrue(motorCycleParkingTicket.IsSuccess);
            Assert.IsNotNull(motorCycleParkingTicket.Data);
            Assert.AreEqual(1, motorCycleParkingTicket.Data.SpotNumber);
            Assert.AreEqual("001", motorCycleParkingTicket.Data.TicketNumber);
        }

        [Test, Order(1)]
        public void Scenario_2_Scooter_Parking_Should_Succeed()
        {
            SystemTime.Now = () => DateTime.Parse("29-May-2022 14:44:07");

            this.scooterParkingTicket = smallParkingLot.Park(VehicleType.Motorcyle);

            Assert.IsTrue(scooterParkingTicket.IsSuccess);
            Assert.IsNotNull(scooterParkingTicket.Data);
            Assert.AreEqual(2, scooterParkingTicket.Data.SpotNumber);
            Assert.AreEqual("002", scooterParkingTicket.Data.TicketNumber);
        }

        [Test, Order(3)]
        public void Scenario_3_Scooter_Parking_Should_Fail()
        {
            var vehicleType = VehicleType.Motorcyle;
            var anotherScooterParkingTicket = smallParkingLot.Park(vehicleType);

            Assert.IsFalse(anotherScooterParkingTicket.IsSuccess);
            Assert.That(anotherScooterParkingTicket.ErrorMessage, Is.EqualTo($"No empty parking spot is available for Vehicle Type-{vehicleType}."));
        }

        [Test, Order(4)]
        public void Scenario_4_Unparking_Scooter_From_Scenario_2_Should_Succeed()
        {
            SystemTime.Now = () => DateTime.Parse("29-May-2022 15:40:07");

            var scooterParkingReceipt = smallParkingLot.Unpark(scooterParkingTicket.Data);

            Assert.IsTrue(scooterParkingReceipt.IsSuccess);
            Assert.IsNotNull(scooterParkingReceipt.Data);
            Assert.AreEqual(10, scooterParkingReceipt.Data.Fees);
        }

        [Test, Order(5)]
        public void Scenario_5_Parking_Motor_Cycle_Should_Succeed()
        {
            SystemTime.Now = () => DateTime.Parse("29-May-2022 15:59:07");

            var anotherMotorCycleParking = smallParkingLot.Park(VehicleType.Motorcyle);

            Assert.IsTrue(anotherMotorCycleParking.IsSuccess);
            Assert.IsNotNull(anotherMotorCycleParking.Data);
            Assert.AreEqual(2, anotherMotorCycleParking.Data.SpotNumber);
            Assert.AreEqual("003", anotherMotorCycleParking.Data.TicketNumber);
        }

        [Test, Order(6)]
        public void Scenario_6_Unparking_Motor_Cycle_From_Scenario_1_Should_Succeed()
        {
            SystemTime.Now = () => DateTime.Parse("29-May-2022 17:44:07");

            var motorCycleParkingReceipt = smallParkingLot.Unpark(motorCycleParkingTicket.Data);

            Assert.IsTrue(motorCycleParkingReceipt.IsSuccess);
            Assert.IsNotNull(motorCycleParkingReceipt.Data);
            Assert.AreEqual(40, motorCycleParkingReceipt.Data.Fees);
        }

        [Test, Order(7)]
        public void Attempting_To_Park_Invalid_Vehicle_Should_Fail()
        {
            var vehicleType = VehicleType.CarSUV;
            var anotherScooterParkingTicket = smallParkingLot.Park(vehicleType);

            Assert.IsFalse(anotherScooterParkingTicket.IsSuccess);
            Assert.That(anotherScooterParkingTicket.ErrorMessage, Is.EqualTo($"The Vehicle Type-{vehicleType} is not allowed in this parking lot."));
        }

        [Test, Order(7)]
        public void Attempting_To_Unpark_With_Invalid_Parking_Ticket_Should_Fail()
        {
            var dummyParkingTicket = new ParkingTicket("1001", 100, SystemTime.Now());

            var parkingReceipt = smallParkingLot.Unpark(dummyParkingTicket);

            Assert.IsFalse(parkingReceipt.IsSuccess);
            Assert.That(parkingReceipt.ErrorMessage, Is.EqualTo($"Invalid parking ticket-{dummyParkingTicket.TicketNumber}"));
        }
    }
}
