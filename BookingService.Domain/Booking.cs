using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string UserEmail { get; set; } = null!;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; } = "CONFIRMED";
    }

}
