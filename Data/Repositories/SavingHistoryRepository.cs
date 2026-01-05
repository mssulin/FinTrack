using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class SavingHistoryRepository(AppDbContext context) 
    : BaseRepository<SavingHistoryEntity>(context), ISavingHistoryRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<SavingHistoryEntity>> GetAllAsync(string userId)
    {
        return await _context.SavingHistory
            .Include(h => h.Saving) 
            .Where(h => h.Saving.UserId == userId)
            .ToListAsync();
    }
}