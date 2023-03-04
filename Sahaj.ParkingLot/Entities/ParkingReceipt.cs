namespace Sahaj.ParkingLot.Entities
{
    public record ParkingReceipt(string ReceiptNumber, DateTime EntryDateTime, DateTime ExitDateTime, double Fees);
}
