using HotelService.Application.Interfaces;
using HotelService.Domain;
using HotelService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly HotelDbContext dbContext;

    public HotelRepository(HotelDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<Hotel>> GetAllAsync()
        => await dbContext.Hotels.AsNoTracking().ToListAsync();

    public async Task<Hotel?> GetByIdAsync(int id)
        => await dbContext.Hotels.FindAsync(id);

    public async Task AddAsync(Hotel hotel)
    {
        dbContext.Hotels.Add(hotel);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Hotel hotel)
    {
        dbContext.Hotels.Update(hotel);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var hotel = await dbContext.Hotels.FindAsync(id);
        if (hotel == null) return;

        dbContext.Hotels.Remove(hotel);
        await dbContext.SaveChangesAsync();
    }
}
