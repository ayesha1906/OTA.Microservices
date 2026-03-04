using BookingService.Application.Interfaces;
using BookingService.Domain;
using BookingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext dbContext;

        public BookingRepository(BookingDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            dbContext.Bookings.Add(booking);
            await dbContext.SaveChangesAsync();
            return booking;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await dbContext.Bookings.AsNoTracking().ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await dbContext.Bookings.FindAsync(id);
        }

        public async Task<List<Booking>> GetByUserAsync(string email)
        {
            return await dbContext.Bookings
                .Where(b => b.UserEmail == email)
                .ToListAsync();
        }
        public async Task UpdateAsync(Booking booking)
        {
            dbContext.Bookings.Update(booking);
            await dbContext.SaveChangesAsync();
        }

        public async Task CancelAsync(int id)
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null) return;

            booking.Status = "CANCELLED";
            await dbContext.SaveChangesAsync();
        }
    }
}
