namespace GadgetHub.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public string CustomerEmail { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public Address DeliveryAddress { get; set; }

    public float SubTotal { get; set; }

    public float DeliveryCharges { get; set; }

    public List<OrderItem> OrderItems { get; set; }
}
