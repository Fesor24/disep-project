using GadgetHub.Data;
using GadgetHub.Models;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.Controllers;
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ViewResult> GetProducts(string category)
    {
        ProductsCategoryViewModel products = new();

        (products.Products, products.Category) = category switch
        {
            "highly-rated" => (await _context.Products.Where(x => x.Ratings > 3)
            .Select(x => new ProductsViewModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                NewRelease = x.NewRelease,
                Ratings = x.Ratings
            })
            .ToListAsync(), "Highly Rated Products"),

            "new-release" => (await _context.Products.Where(x => x.NewRelease)
            .Select(x => new ProductsViewModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                NewRelease = x.NewRelease,
                Ratings = x.Ratings
            })
            .ToListAsync(), "New Releases"),

             _ => (await _context.Products
            .Select(x => new ProductsViewModel
            {   
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                NewRelease = x.NewRelease,
                Ratings = x.Ratings
            }).ToListAsync(), "All Products")
        };


        return View("Products",products);
    }

    public async Task<ViewResult> GetProduct(int id)
    {
        RelatedProductsViewModel relatedProduct = new();

        relatedProduct.Product = await _context.Products
            .Where(x => x.Id == id)
            .Select(x => new ProductsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                NewRelease = x.NewRelease,
                Ratings = x.Ratings,
                CategoryId = x.CategoryId
            })
            .FirstOrDefaultAsync();

        if(relatedProduct.Product is null)
        {
            return View("NotFound");
        }

        relatedProduct.RelatedProducts = await _context.Products
            .Where(x => x.CategoryId == relatedProduct.Product.CategoryId)
            .Select(x => new ProductsViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                CategoryId = x.CategoryId,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                Ratings = x.Ratings,
                Id = x.Id,
                NewRelease = x.NewRelease
            })
            .Take(4)
            .ToListAsync();

        return View("Product", relatedProduct);
    }

    [HttpPost]
    public async Task<ViewResult> Search()
    {
        ProductsCategoryViewModel products = new();

        string searchName = Request.Form["Name"];

        products.Products = await _context.Products
            .Where(x => string.IsNullOrWhiteSpace(searchName) || x.Name.Contains(searchName))
            .Select(x => new ProductsViewModel
            {
                Name = x.Name,
                CategoryId = x.CategoryId,
                Id = x.Id,
                Price = x.Price,
                Description = x.Description,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                NewRelease = x.NewRelease,
                Ratings = x.Ratings
            })
            .ToListAsync();

        products.Category = string.IsNullOrWhiteSpace(searchName) ? "All Products" : 
            $"Products related to '{searchName}'";

        return View("Products",products);
    }
}
