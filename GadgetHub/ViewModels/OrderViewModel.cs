using GadgetHub.Entities.OrderAggregate;

namespace GadgetHub.ViewModels;

public class OrderViewModel
{
    public OrderStatus OrderStatus { get; set; }

    public AddressViewModel Address { get; set; }

    public string Subtotal { get; set; }

    public string DeliveryCharges { get; set; }

    public string StrTotal { get; set; }

    public float Total { get; set; }

    public int OrderId { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public List<OrderItemViewModel> OrderItems { get; set; } = new();
}
