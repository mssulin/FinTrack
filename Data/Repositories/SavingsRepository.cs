using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class SavingsRepository(AppDbContext context) 
    : BaseRepository<SavingEntity>(context), ISavingRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<SavingEntity>> GetAllAsync(string userId)
    {
        return await _context.Savings
            .Where(s => s.UserId == userId)
            .Include(s => s.SavingHistory)
            .ToListAsync();
    }
}