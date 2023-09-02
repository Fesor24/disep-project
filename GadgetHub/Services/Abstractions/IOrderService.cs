using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities;
using GadgetHub.ViewModels;

namespace GadgetHub.Services.Abstractions;

public interface IOrderService
{
    Task<(bool OrderCreated, OrderViewModel orderViewModel)> CreateOrderAsync(IUnitOfWork unitOfWork, ShoppingCart cart,
        AddressViewModel address, string customerEmail);
}
