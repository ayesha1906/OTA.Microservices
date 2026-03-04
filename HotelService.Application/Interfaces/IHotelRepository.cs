using HotelService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel?> GetByIdAsync(int id);
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(int id);
        Task<bool> CheckAvailabilityAsync(int hotelId, DateTime checkIn, DateTime checkOut);

        Task ReserveRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut);

        Task RestoreRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut);
    }
}
