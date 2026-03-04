using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Domain
{
    public class RoomInventory
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        public DateTime Date { get; set; }

        public int AvailableRooms { get; set; }
    }
}
