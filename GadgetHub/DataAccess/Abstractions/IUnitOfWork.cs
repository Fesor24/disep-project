namespace GadgetHub.DataAccess.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }

    IOrderRepository OrderRepository { get; }

    IPaymentTransactionRepository PaymentTransactionRepository { get; }
    Task<int> Complete();
}
