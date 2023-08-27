namespace GadgetHub.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
    public ProductItemOrdered ItemOrdered { get; set; }

    public int Quantity { get; set; }

    public float Price { get; set; }

}
