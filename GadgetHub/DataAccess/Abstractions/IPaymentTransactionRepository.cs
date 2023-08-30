using GadgetHub.Entities;

namespace GadgetHub.DataAccess.Abstractions;

public interface IPaymentTransactionRepository : IGenericRepository<PaymentTransaction>
{
    Task<PaymentTransaction> GetByReferenceAsync(string reference);
}
