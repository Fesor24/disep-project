using GadgetHub.Entities;

namespace GadgetHub.Services.Abstractions;
public interface IShoppingCartRepository
{
    bool DeleteShoppingCart(HttpContext context, string basketId = "basketId");
    ShoppingCart GetCart(HttpContext context, string basketId = "basketId");
    ShoppingCart UpdateShoppingCart(HttpContext context, ShoppingCart cart, string basketId = "basketId");
}