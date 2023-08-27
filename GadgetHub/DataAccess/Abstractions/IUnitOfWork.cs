namespace GadgetHub.DataAccess.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }
    Task<int> Complete();
}
