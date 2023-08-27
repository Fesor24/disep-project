using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities;
using GadgetHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GadgetHub.Controllers;
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ViewResult> GetProducts(string category)
    {
        ProductsCategoryViewModel products = new();

        List<Product> product = new();

        (product, products.Category) = category switch
        {
            "highly-rated" => (await _unitOfWork.ProductRepository.GetHighlyRatedProducts() , "Highly Rated Products"),

            "new-release" => (await _unitOfWork.ProductRepository.GetNewReleases(), "New Releases"),

             _ => (await _unitOfWork.ProductRepository.GetAll(), "All Products")
        };

        products.Products = product.Select(x => new ProductsViewModel
        {
            Id = x.Id,
            Description = x.Description,
            Name = x.Name,
            Price = x.Price,
            StrPrice = FormatPrice(x.Price),
            Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
            NewRelease = x.NewRelease,
            Ratings = x.Ratings
        }).ToList();


        return View("Products",products);
    }

    public async Task<ViewResult> GetProduct(int id)
    {
        RelatedProductsViewModel relatedProduct = new();

        Product product = await _unitOfWork.ProductRepository.GetById(id);

        if (product is null)
        {
            return View("NotFound");
        }

        relatedProduct.Product.NewRelease = product.NewRelease;
        relatedProduct.Product.Name = product.Name;
        relatedProduct.Product.Price = product.Price;
        relatedProduct.Product.StrPrice = FormatPrice(product.Price);
        relatedProduct.Product.Id = product.Id;
        relatedProduct.Product.Ratings = product.Ratings;
        relatedProduct.Product.Image = Request.Scheme + "://" + Request.Host + product.Image.TrimStart('~');
        relatedProduct.Product.CategoryId = product.CategoryId;
        relatedProduct.Product.Description = product.Description;

        List<Product> relatedProducts = await _unitOfWork.ProductRepository.GetRelatedProducts(product.CategoryId);

        relatedProduct.RelatedProducts = relatedProducts
            .Select(x => new ProductsViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                StrPrice = FormatPrice(x.Price),
                CategoryId = x.CategoryId,
                Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
                Ratings = x.Ratings,
                Id = x.Id,
                NewRelease = x.NewRelease
            })
            .ToList();

        return View("Product", relatedProduct);
    }

    [HttpPost]
    public async Task<ViewResult> Search()
    {
        ProductsCategoryViewModel products = new();

        string searchName = Request.Form["Name"];

        string orderValue = Request.Form["Order"];

        List<Product> productList = await _unitOfWork.ProductRepository.Search(searchName);

        products.Products = productList.Select(x => new ProductsViewModel
        {
            Name = x.Name,
            CategoryId = x.CategoryId,
            Id = x.Id,
            Price = x.Price,
            StrPrice = FormatPrice(x.Price),
            Description = x.Description,
            Image = Request.Scheme + "://" + Request.Host + x.Image.TrimStart('~'),
            NewRelease = x.NewRelease,
            Ratings = x.Ratings
        }).ToList();

        products.Products = orderValue switch
        {
            "desc" => products.Products.OrderByDescending(x => x.Price),
            "asc" => products.Products.OrderBy(x => x.Price),
            _ => products.Products.OrderBy(x => x.Price)
        };

        products.Category = string.IsNullOrWhiteSpace(searchName) ? "All Products" : 
            $"Products related to '{searchName}'";

        return View("Products",products);
    }

    private static string FormatPrice(float price)
    {
        return $"{price:n0}";
    }
}
