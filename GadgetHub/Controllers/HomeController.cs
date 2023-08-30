using GadgetHub.DataAccess.Abstractions;
using GadgetHub.DataAccess.Implementation;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GadgetHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var newReleases = await _unitOfWork.ProductRepository.GetNewReleases();

        var dataToReturn = newReleases
            .Select(x => new ProductsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                Price = x.Price,
                StrPrice = FormatPrice(x.Price),
                Ratings = x.Ratings,
                Description = x.Description,
                NewRelease = x.NewRelease
            })
            .Take(8)
            .ToList();

        return View(dataToReturn);
    }

    public IActionResult About()
    {
        return View();
    }

    private static string FormatPrice(float price)
    {
        return $"{price:n0}";
    }
}
