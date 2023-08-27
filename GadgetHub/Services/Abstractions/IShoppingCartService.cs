using GadgetHub.Entities;

namespace GadgetHub.Services.Abstractions;

public interface IShoppingCartService
{
    Task<ShoppingCart> AddItemToCart(ShoppingCart cart,int productId, string basePath, int quantity = 1);

    Task<ShoppingCart> DecrementItemFromCart(ShoppingCart cart, int productId, string basePath);

    Task<ShoppingCart> DeleteItemFromCart(ShoppingCart cart, int productId, string basePath);

    Task DeleteShoppingCart();
}
