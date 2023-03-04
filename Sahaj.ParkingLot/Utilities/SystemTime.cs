﻿namespace Sahaj.ParkingLot.Utilities
{
    public class SystemTime
    {
        private static Func<DateTime> now = () => DateTime.Now;
        public static Func<DateTime> Now
        {
            get
            {
                return now;
            }

            set
            {
                now = value;
            }
        }
    }
}
