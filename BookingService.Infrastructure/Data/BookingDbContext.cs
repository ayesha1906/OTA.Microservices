using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Data
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Booking> Bookings => Set<Booking>();

        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options)
        {
        }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<Booking>()
    //.Property(b => b.TotalPrice)
    //.HasColumnType("decimal(18,2)");
    //    }
    }
}