namespace SearchService.API.Models
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int StarRating { get; set; }
    }

}
