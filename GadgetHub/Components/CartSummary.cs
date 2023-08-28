using GadgetHub.DataAccess.Abstractions;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GadgetHub.Components;

public class CartSummary : ViewComponent
{
    private readonly IShoppingCartRepository _shoppingCartRepository;

    public CartSummary(IShoppingCartRepository shoppingCartRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }

    public IViewComponentResult Invoke()
    {
        var cart = _shoppingCartRepository.GetCart(HttpContext);

        ShoppingCartViewModel scvm = new();

        if (cart == null)
        {
            return View(scvm);
        }

        scvm = new ShoppingCartViewModel
        {
            Id = cart.Id,
            Items = cart.Items.Select(x => new ShoppingCartItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Quantity = x.Quantity,
                Image = x.Image,
                Price = x.Quantity * x.Price,
                CategoryId = x.CategoryId,
            }).ToList()
        };

        return View(scvm);


    }
}
