namespace GadgetHub.Entities;

public class PaymentTransaction : BaseEntity
{
    public bool Verified { get; set; }

    public int OrderId { get; set; }

    public string CustomerEmail { get; set; }

    public string Reference { get; set; }

    public float Amount { get; set; }
}
