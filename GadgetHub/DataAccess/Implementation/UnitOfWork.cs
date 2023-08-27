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
    }

    public IProductRepository ProductRepository { get; set; }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
