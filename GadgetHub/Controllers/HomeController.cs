using GadgetHub.Data;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var newReleases = await _context.Products
            .Where(x => x.NewRelease)
            .Select(x => new ProductsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                Price = x.Price,
                Description = x.Description,
                NewRelease = x.NewRelease
            })
            .Take(8)
            .ToListAsync();

        return View(newReleases);
    }

    public IActionResult About()
    {
        return View();
    }
}
