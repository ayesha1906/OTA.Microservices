using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelService.Domain;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Infrastructure.Data;

public class HotelDbContext : DbContext
{
    //public HotelDbContext(DbContextOptions<HotelDbContext> options)
    //    : base(options) { }

    //public DbSet<Hotel> Hotels => Set<Hotel>();

    public HotelDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Hotel> Hotels { get; set; }
}
