using GadgetHub.Data;
using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.DataAccess.Implementation;

public class PaymentTransactionRepository : GenericRepository<PaymentTransaction>, IPaymentTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentTransactionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PaymentTransaction> GetByReferenceAsync(string reference)
    {
        return await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.Reference == reference);
    }
}
