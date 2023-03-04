namespace Sahaj.ParkingLot.Domain.Entities
{
    public record Response<T>
    {
        public Response(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public Response(T data)
        {
            this.Data = data;
        }

        public string? ErrorMessage { get;}
        public bool IsSuccess { get => string.IsNullOrWhiteSpace(this.ErrorMessage); }
        public T? Data { get; }
    }
}
