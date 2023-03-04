namespace Sahaj.ParkingLot.Entities
{
    using Sahaj.ParkingLot.Enums;
    public record ParkingFee(double MinHours, double MaxHours, double Fee, FeeCalculationType FeeCalculationType, ParkingFee? PreviousIntervalFee)
    {
        public ParkingFee(double MinHours, double MaxHours, double Fee, FeeCalculationType FeeCalculationType): this(MinHours,MaxHours, Fee, FeeCalculationType, null)
        {
        }

        public double CalculateFee(double totalHours)
        {
            var roundedHours = Math.Ceiling(totalHours);

            double sumOfPreviousIntervalFee = 0;
            var previousIntervalFee = this.PreviousIntervalFee;

            if (previousIntervalFee != null)
            {
                roundedHours -= previousIntervalFee.MaxHours;
            }

            while (previousIntervalFee != null)
            {
                sumOfPreviousIntervalFee += previousIntervalFee.Fee;
                previousIntervalFee = previousIntervalFee.PreviousIntervalFee;
            }

            var amount = this.FeeCalculationType switch
            {
                FeeCalculationType.PerHour => (this.Fee * roundedHours),
                FeeCalculationType.PerDay => this.Fee * Math.Ceiling(totalHours / 24),
                _ => this.Fee,
            };

            amount += sumOfPreviousIntervalFee;
            return amount;
        }
    }
}
