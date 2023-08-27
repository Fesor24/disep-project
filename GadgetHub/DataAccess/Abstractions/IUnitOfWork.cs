namespace GadgetHub.DataAccess.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }

    IOrderRepository OrderRepository { get; }
    Task<int> Complete();
}
