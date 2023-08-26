using GadgetHub.Models;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options): base(options)
    {
        
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach(var tracker in ChangeTracker.Entries<BaseEntity>())
        {
            tracker.Entity.DateUpdated = DateTimeOffset.UtcNow;

            if(tracker.State == EntityState.Added)
            {
                tracker.Entity.DateCreated = DateTimeOffset.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();
}
