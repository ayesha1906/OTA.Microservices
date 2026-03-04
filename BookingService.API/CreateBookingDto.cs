namespace BookingService.API
{
    public class CreateBookingDto
    {
        public int HotelId { get; set; }

        public string UserEmail { get; set; } = null!;

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }
    }
    public class HotelDto
    {
        public int Id { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
