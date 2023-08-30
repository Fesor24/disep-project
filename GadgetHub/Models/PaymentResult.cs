namespace GadgetHub.Models;

public class PaymentResult
{
    public string AuthorizationUrl { get; set; }
    public string Reference { get; set; }

    public string Message { get; set; }

    public bool Successful { get; set; }

}
