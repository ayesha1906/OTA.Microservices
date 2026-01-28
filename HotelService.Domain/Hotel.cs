using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Domain
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int StarRating { get; set; }
    }
}
