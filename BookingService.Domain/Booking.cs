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

        public decimal TotalPrice { get; set; }

        public decimal RefundAmount { get; set; } = 0;

        public string Status { get; set; } = "PENDING_PAYMENT";
        // PENDING_PAYMENT | CONFIRMED | FAILED | CANCELLED

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
