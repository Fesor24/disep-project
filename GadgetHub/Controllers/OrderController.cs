using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Services.Abstractions;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace GadgetHub.Controllers;
public class OrderController : Controller
{
    private readonly IShoppingCartRepository _shoppingCartRepo;
    private readonly IOrderService _orderService;

    public OrderController(IShoppingCartRepository shoppingCartRepo, IOrderService orderService)
    {
        _shoppingCartRepo = shoppingCartRepo;
        _orderService = orderService;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder(AddressViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View("Index",model);
        }

        var cart = _shoppingCartRepo.GetCart(HttpContext);

        string email = User.FindFirstValue(ClaimTypes.Email);

        var (orderCreated, orderViewModel) = await _orderService.CreateOrderAsync(cart, model, email);

        orderViewModel.Address.Email = email;

        if (orderCreated)
        {
            _shoppingCartRepo.DeleteShoppingCart(HttpContext);
        }

        HttpContext.Session.SetString("order", JsonSerializer.Serialize(orderViewModel));

        return RedirectToAction("OrderConfirmed");
    }

    public IActionResult OrderConfirmed()
    {
        var order = HttpContext.Session.GetString("order");

        if(order == null)
        {
            return View();
        }

        var orderViewModel = JsonSerializer.Deserialize<OrderViewModel>(order);

        return View(orderViewModel);
    }
}
