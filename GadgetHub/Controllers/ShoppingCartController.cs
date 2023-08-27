using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities;
using GadgetHub.Services.Abstractions;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GadgetHub.Controllers;
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartRepository _cartRepo;
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartRepository cartRepo, IShoppingCartService shoppingCartService)
    {
        _cartRepo = cartRepo;
        _shoppingCartService = shoppingCartService;
    }

    public IActionResult Index()
    {
        var cart = _cartRepo.GetCart(HttpContext);

        ShoppingCartViewModel scvm = new();

        if (cart is null)
        {
            return View("ShoppingCart", scvm);
        }

        scvm = new ShoppingCartViewModel
        {
            Id = cart.Id,
            Items = cart.Items.Select(x => new ShoppingCartItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                Image = x.Image,
                SubTotal = x.Quantity * x.Price,
                CategoryId = x.CategoryId,
            }).ToList()
        };

        return View("ShoppingCart", scvm);
    }

    public async Task<RedirectToActionResult> AddItemToCart(int id)
    {
        var shoppingCart = _cartRepo.GetCart(HttpContext);

        string cartId = Guid.NewGuid().ToString();

        string basePath = Request.Scheme + "://" + Request.Host;

        shoppingCart = await _shoppingCartService.AddItemToCart(shoppingCart, id, basePath);

        _cartRepo.UpdateShoppingCart(HttpContext, shoppingCart);

        return RedirectToAction("Index");
    }

    public async Task<RedirectToActionResult> AddItemToCartWithQuantity(int id)
    {
        int itemQuantity = 1;

        if (Request.Form is not null && Request.Form.Count > 0)
        {
            var itemQuantityValues = Request.Form["itemQuantity"];

            int.TryParse(itemQuantityValues[0], out itemQuantity);
        }

        var shoppingCart = _cartRepo.GetCart(HttpContext);

        string basePath = Request.Scheme + "://" + Request.Host;

        shoppingCart = await _shoppingCartService.AddItemToCart(shoppingCart, id, basePath, itemQuantity);

        var updatedCart = _cartRepo.UpdateShoppingCart(HttpContext, shoppingCart);

        return RedirectToAction("Index");
    }

    public async Task<RedirectToActionResult> DecrementItemFromCart(int id)
    {
        var cart = _cartRepo.GetCart(HttpContext);

        string basePath = Request.Scheme + "://" + Request.Host;

        cart = await _shoppingCartService.DecrementItemFromCart(cart, id, basePath);

        _cartRepo.UpdateShoppingCart(HttpContext, cart);

        return RedirectToAction("Index");

    }

    public async Task<RedirectToActionResult> DeleteItemFromCart(int id)
    {
        var cart = _cartRepo.GetCart(HttpContext);

        string basePath = Request.Scheme + "://" + Request.Host;

        cart = await _shoppingCartService.DeleteItemFromCart(cart, id, basePath);

        cart = _cartRepo.UpdateShoppingCart(HttpContext, cart);

        return RedirectToAction("Index");
    }
}
