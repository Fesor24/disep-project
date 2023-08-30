namespace GadgetHub.Models;

public class Payment
{
    public string Currency => "NGN";
    public float Amount { get; set; }

    public string Email { get; set; }

    public int OrderId { get; set; }

    public string CallbackUrl { get; set; }
}
