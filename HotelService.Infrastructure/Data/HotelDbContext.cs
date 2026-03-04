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
    public DbSet<Hotel> Hotels { get; set; }

    public DbSet<RoomInventory> RoomInventories { get; set; }

    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }
}

