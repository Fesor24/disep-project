using GadgetHub.Entities.OrderAggregate;

namespace GadgetHub.ViewModels;

public class OrderViewModel
{
    public OrderStatus OrderStatus { get; set; }

    public AddressViewModel Address { get; set; }

    public string Subtotal { get; set; }

    public string DeliveryCharges { get; set; }

    public string Total { get; set; }

    public List<OrderItemViewModel> OrderItems { get; set; } = new();
}
