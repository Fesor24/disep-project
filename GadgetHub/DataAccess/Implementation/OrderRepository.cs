using GadgetHub.Data;
using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities.OrderAggregate;

namespace GadgetHub.DataAccess.Implementation;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}
