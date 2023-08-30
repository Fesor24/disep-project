using GadgetHub.Models;

namespace GadgetHub.Services.Abstractions;

public interface IPaymentService
{
    Task<PaymentResult> InitializeAsync(Payment payment);

    Task<PaymentResult> VerifyAsync(string reference);
}
