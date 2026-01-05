using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class IncomeRepository(AppDbContext context) 
    : BaseRepository<IncomeEntity>(context), IIncomeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<IncomeEntity>> GetAllAsync(string userId)
    {
        return await _context.Incomes
            .Where(i => i.UserId == userId)
            .ToListAsync();
    }
}