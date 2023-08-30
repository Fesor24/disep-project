using GadgetHub.Data;
using GadgetHub.DataAccess.Abstractions;

namespace GadgetHub.DataAccess.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        ProductRepository = new ProductRepository(context);
        OrderRepository = new OrderRepository(context);
        PaymentTransactionRepository = new PaymentTransactionRepository(context);
    }

    public IProductRepository ProductRepository { get; set; }
    public IOrderRepository OrderRepository { get; set; }

    public IPaymentTransactionRepository PaymentTransactionRepository { get; set; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
