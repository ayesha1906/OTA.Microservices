using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);

        Task<Booking?> GetByIdAsync(int id);

        Task<List<Booking>> GetByUserAsync(string email);

        Task UpdateAsync(Booking booking);

       // Task<Booking> CreateAsync(Booking booking);
        Task<List<Booking>> GetAllAsync();
       // Task<Booking?> GetByIdAsync(int id);
        Task CancelAsync(int id);
    }
}
