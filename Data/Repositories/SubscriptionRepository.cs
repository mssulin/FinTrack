using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<SubscriptionEntity>(context), ISubscriptionRepository;