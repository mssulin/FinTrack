using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ExpenseRepository(AppDbContext context)
    : BaseRepository<ExpenseEntity>(context), IExpenseRepository;