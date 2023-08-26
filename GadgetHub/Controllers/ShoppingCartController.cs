using GadgetHub.Data;
using GadgetHub.Entities;
using GadgetHub.Services.Abstractions;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.Controllers;
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartRepository _cartRepo;
    private readonly ApplicationDbContext _context;

    public ShoppingCartController(IShoppingCartRepository cartRepo, ApplicationDbContext context)
    {
        _cartRepo = cartRepo;
        _context = context;
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

        if (shoppingCart is null)
        {
            shoppingCart = new ShoppingCart(cartId);
        }

        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if(product is null)
        {
            return RedirectToAction("NotFound", "Home");
        }

        ShoppingCartItem cartItem = new ShoppingCartItem
        {
            Id = product.Id,
            Name = product.Name,
            Image = Request.Scheme + "://" + Request.Host + product.Image.TrimStart('~'),
            Price = product.Price,
            CategoryId = product.CategoryId,
            Quantity = 1
        };

        shoppingCart = AddOrUpdateShoppingCartItem(shoppingCart, cartItem);

        var updatedCart = _cartRepo.UpdateShoppingCart(HttpContext, shoppingCart);

        ShoppingCartViewModel scvm = new ShoppingCartViewModel
        {
            Id = shoppingCart.Id,
            Items = shoppingCart.Items.Select(x => new ShoppingCartItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                Image = x.Image,
                SubTotal = x.Quantity * x.Price,
                CategoryId=x.CategoryId,
            }).ToList()
        };

        return RedirectToAction("Index");
    }

    private ShoppingCart AddOrUpdateShoppingCartItem(ShoppingCart cart, ShoppingCartItem item)
    {
        var cartItem = cart.Items.Find(x => x.Id == item.Id);

        if(cartItem is null)
        {
            cart.Items.Add(item);

            return cart;
        }

        cart.Items.First(x => x.Id == item.Id).Quantity++;

        return cart;
    }
}
