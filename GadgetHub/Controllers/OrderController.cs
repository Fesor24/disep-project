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
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IShoppingCartRepository shoppingCartRepo, IOrderService orderService,
        IPaymentService paymentService, IUnitOfWork unitOfWork)
    {
        _shoppingCartRepo = shoppingCartRepo;
        _orderService = orderService;
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
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
        string redirectUrl = "";

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
            var result = await _paymentService.InitializeAsync(new Models.Payment
            {
                Amount = orderViewModel.Total,
                CallbackUrl = $"{Request.Scheme}://{Request.Host}/order/orderconfirmed",
                Email = email,
                OrderId = orderViewModel.OrderId
            });

            redirectUrl = result.AuthorizationUrl;

            _shoppingCartRepo.DeleteShoppingCart(HttpContext);
        }

        HttpContext.Session.SetString("order", JsonSerializer.Serialize(orderViewModel));

        return Redirect(redirectUrl);
    }

    public async Task<IActionResult> OrderConfirmed(string trxref = null, string reference = null)
    {
        var result = await _paymentService.VerifyAsync(reference);

        OrderViewModel orderViewModel = new();

        if (result.Successful)
        {
            var paymentTransaction = await _unitOfWork.PaymentTransactionRepository.GetByReferenceAsync(reference);

            if (paymentTransaction is null)
            {
                return View("NotFound");
            }

            var orderId = paymentTransaction.OrderId;

            var order = await _unitOfWork.OrderRepository.GetById(orderId);

            if(order is null)
            {
                return View("NotFound");
            };

            order.PaymentStatus = Entities.OrderAggregate.PaymentStatus.Paid;

            order.OrderStatus = Entities.OrderAggregate.OrderStatus.Shipped;

            paymentTransaction.Verified = true;

            _unitOfWork.PaymentTransactionRepository.Update(paymentTransaction);

            _unitOfWork.OrderRepository.Update(order);

            _unitOfWork.Complete();

            var ovm = HttpContext.Session.GetString("order");

            if (ovm == null)
            {
                return View();
            }

            orderViewModel = JsonSerializer.Deserialize<OrderViewModel>(ovm);

            orderViewModel.PaymentStatus = order.PaymentStatus;

            orderViewModel.OrderStatus = order.OrderStatus;
        }

        else
        {
            return View("Error");
        }

        return View(orderViewModel);
    }
}
