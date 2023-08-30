using GadgetHub.Entities;
using GadgetHub.Entities.Identity;
using GadgetHub.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GadgetHub.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
}
