using GadgetHub.Entities;
using GadgetHub.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace GadgetHub.Services.Implementation;

public class ShoppingCartRepository : IShoppingCartRepository
{
    public ShoppingCart GetCart(HttpContext context, string basketId = "basketId")
    {
        var shoppingCart = context.Session.GetString(basketId);

        return shoppingCart.IsNullOrEmpty() ? null : JsonSerializer.Deserialize<ShoppingCart>(shoppingCart);
    }

    public ShoppingCart UpdateShoppingCart(HttpContext context, ShoppingCart cart, string basketId = "basketId")
    {
        context.Session.SetString(basketId, JsonSerializer.Serialize(cart));

        return GetCart(context);
    }

    public bool DeleteShoppingCart(HttpContext context, string basketId = "basketId")
    {
        context.Session.Remove(basketId);

        return true;
    }

}
