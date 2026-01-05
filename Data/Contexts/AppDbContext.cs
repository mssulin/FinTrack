using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<UserEntity>(options)
{
    public virtual DbSet<IncomeEntity> Incomes { get; set; }
    
    public virtual DbSet<ExpenseEntity> Expenses { get; set; }
    
    public virtual DbSet<SubscriptionEntity> Subscriptions { get; set; }
    
    public virtual DbSet<SavingEntity> Savings { get; set; }
    
    public virtual DbSet<SavingHistoryEntity> SavingHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<SavingHistoryEntity>()
            .HasOne(s => s.Saving)
            .WithMany(s => s.SavingHistory)
            .HasForeignKey(s => s.SavingId);
    }
}
