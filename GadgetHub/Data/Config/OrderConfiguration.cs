using GadgetHub.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GadgetHub.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasMany(x => x.OrderItems).WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(x => x.DeliveryAddress, io =>
        {
            io.WithOwner();
        });

        builder.Property(x => x.OrderStatus)
            .HasConversion(c => c.ToString(),
            p => (OrderStatus)Enum.Parse(typeof(OrderStatus), p));
    }
}
