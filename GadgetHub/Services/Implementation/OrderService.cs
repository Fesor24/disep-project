using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities;
using GadgetHub.Entities.OrderAggregate;
using GadgetHub.Services.Abstractions;
using GadgetHub.ViewModels;

namespace GadgetHub.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool OrderCreated, OrderViewModel orderViewModel)> CreateOrderAsync(ShoppingCart cart, 
        AddressViewModel address, string customerEmail)
    {
        Order order = new();

        order.OrderItems = new();

        foreach(var item in cart.Items)
        {
            var product = _unitOfWork.ProductRepository.GetById(item.Id);

            if(product is null)
            {
                continue;
            }

            OrderItem orderItem = new()
            {
                Price = item.Price,
                ItemOrdered = new ProductItemOrdered
                {
                    Name = item.Name,
                    Image = item.Image,
                    Id = item.Id,
                },
                Quantity = item.Quantity
            };

            order.OrderItems.Add(orderItem);
        }

        order.OrderStatus = OrderStatus.Pending;

        order.SubTotal = cart.Items.Sum(x => x.Price * x.Quantity);

        order.CustomerEmail = customerEmail;

        order.DeliveryCharges = 3000f;

        order.DeliveryAddress = new Address
        {
            FirstName = address.FirstName,
            LastName = address.LastName,
            State = address.State,
            Street = address.Street,
            City = address.City
        };

        await _unitOfWork.OrderRepository.AddAsync(order);

        var changes = await _unitOfWork.Complete();

        if(changes > 1)
        {
            OrderViewModel orderViewModel = new()
            {
                OrderId = order.Id,
                Address = new AddressViewModel
                {
                    City = order.DeliveryAddress.City,
                    State = order.DeliveryAddress.State,
                    Street = order.DeliveryAddress.Street,
                    FirstName = order.DeliveryAddress.FirstName,
                    LastName = order.DeliveryAddress.LastName
                },
                OrderStatus = order.OrderStatus,
                Subtotal = FormatPrice(order.SubTotal),
                DeliveryCharges = FormatPrice(order.DeliveryCharges),
                Total = order.SubTotal + order.DeliveryCharges,
                StrTotal = FormatPrice(order.SubTotal + order.DeliveryCharges),
            };

            foreach(var item in order.OrderItems)
            {
                OrderItemViewModel oivm = new()
                {
                    Price = FormatPrice(item.Price),
                    ProductName = item.ItemOrdered.Name,
                    Image = item.ItemOrdered.Image,
                    Quantity = item.Quantity
                };

                orderViewModel.OrderItems.Add(oivm);
            }

            return (true, orderViewModel);
        }

        return (false, new OrderViewModel());
    }

    private static string FormatPrice(float price)
    {
        return $"{price:n0}";
    }
}
