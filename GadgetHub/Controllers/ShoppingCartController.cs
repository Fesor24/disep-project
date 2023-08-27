using GadgetHub.Data;
using GadgetHub.Entities;
using GadgetHub.Models;
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

        var itemQuantityValues = Request.Form["itemQuantity"];

        int itemQuantity = 1;

        int.TryParse(itemQuantityValues[0], out itemQuantity);

        if (shoppingCart is null)
        {
            shoppingCart = new ShoppingCart(cartId);
        }

        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if(product is null)
        {
            return RedirectToAction("NotFound", "Home");
        }

        ShoppingCartItem cartItem = MapProductToShoppingCartItem(product, itemQuantity);

        shoppingCart = AddOrUpdateShoppingCartItem(shoppingCart, cartItem, itemQuantity);

        var updatedCart = _cartRepo.UpdateShoppingCart(HttpContext, shoppingCart);

        return RedirectToAction("Index");
    }

    public async Task<RedirectToActionResult> DecrementItemFromCart(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if(product is null)
        {
            return RedirectToAction("NotFound", "Home", id);
        }

        var cart = _cartRepo.GetCart(HttpContext);

        string cartId = Guid.NewGuid().ToString();

        if(cart is null)
        {
            cart = new ShoppingCart(cartId);
        }

        ShoppingCartItem shoppingCartItem = MapProductToShoppingCartItem(product);

        cart = DecrementOrRemoveItemFromCart(cart, shoppingCartItem);

        cart = _cartRepo.UpdateShoppingCart(HttpContext, cart);

        return RedirectToAction("Index");

    }

    public async Task<RedirectToActionResult> DeleteItemFromCart(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if(product is null)
        {
            return RedirectToAction("NotFound", "Home", id);
        }

        var cart = _cartRepo.GetCart(HttpContext);

        if(cart is null)
        {
            cart = new ShoppingCart(Guid.NewGuid().ToString());
        }

        ShoppingCartItem shoppingCartItem = MapProductToShoppingCartItem(product);

        cart.Items = cart.Items.Where(x => x.Id != shoppingCartItem.Id).ToList();

        cart = _cartRepo.UpdateShoppingCart(HttpContext, cart);

        return RedirectToAction("Index");
    }

    private ShoppingCart AddOrUpdateShoppingCartItem(ShoppingCart cart, ShoppingCartItem item, int quantity = 1)
    {
        var cartItem = cart.Items.Find(x => x.Id == item.Id);

        if(cartItem is null)
        {
            cart.Items.Add(item);

            return cart;
        }

        cart.Items.First(x => x.Id == item.Id).Quantity+=quantity;

        return cart;
    }

    private ShoppingCart DecrementOrRemoveItemFromCart(ShoppingCart cart, ShoppingCartItem item)
    {
        var cartItem = cart.Items.FirstOrDefault(x => x.Id == item.Id);

        if (cartItem is not null)
        {
            if(cartItem.Quantity > 1)
            {
                cart.Items.FirstOrDefault(x => x.Id == item.Id).Quantity--;

                return cart;
            }
            else
            {
                cart.Items.Remove(cartItem);

                return cart;
            }
        }

        return cart;
    }

    private ShoppingCartItem MapProductToShoppingCartItem(Product product, int quantity = 1)
    {
        ShoppingCartItem cartItem = new()
        {
            Id = product.Id,
            Name = product.Name,
            Image = Request.Scheme + "://" + Request.Host + product.Image.TrimStart('~'),
            Price = product.Price,
            CategoryId = product.CategoryId,
            Quantity = quantity
        };

        return cartItem;
    }
}
