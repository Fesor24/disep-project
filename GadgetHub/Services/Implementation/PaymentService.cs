using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Models;
using GadgetHub.Services.Abstractions;
using PayStack.Net;

namespace GadgetHub.Services.Implementation;

public class PaymentService : IPaymentService
{
    private readonly PayStackApi _paystack;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(PayStackApi paystack, IUnitOfWork unitOfWork, ILogger<PaymentService> logger)
    {
        _paystack = paystack;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaymentResult> InitializeAsync(Payment payment)
    {
        PaymentResult result = new();

        var response = _paystack.Transactions.Initialize(new TransactionInitializeRequest
        {
            AmountInKobo = (int)payment.Amount * 100,
            CallbackUrl = payment.CallbackUrl,
            Currency = payment.Currency,
            Email = payment.Email,
            Reference = Guid.NewGuid().ToString()
        });

        if (response.Status)
        {
            result.AuthorizationUrl = response.Data.AuthorizationUrl;
            result.Reference = response.Data.Reference;
            result.Message = response.Message;
            result.Successful = true;
        }
        else
        {
            result.Successful = false;
            _logger.LogError(response.Message);
        }

        await _unitOfWork.PaymentTransactionRepository.AddAsync(new Entities.PaymentTransaction
        {
            Reference = response.Data.Reference,
            Amount = payment.Amount,
            CustomerEmail = payment.Email,
            Verified = false,
            OrderId = payment.OrderId
        });

        await _unitOfWork.Complete();

        return result;
    }

    public Task<PaymentResult> VerifyAsync(string reference)
    {
        PaymentResult result = new();

        var response = _paystack.Transactions.Verify(reference);

        if (response.Status)
        {
            result.Successful = true;
            result.Message = response.Message;
        }
        else
        {
            result.Successful = false;
            result.Message = response.Message;
        }

        return Task.FromResult(result);
    }
}
