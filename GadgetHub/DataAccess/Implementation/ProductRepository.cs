using GadgetHub.Data;
using GadgetHub.DataAccess.Abstractions;
using GadgetHub.Entities;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.DataAccess.Implementation;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetHighlyRatedProducts()
    {
        return await _context.Products.Where(x => x.Ratings > 3)
            .ToListAsync();
    }

    public async Task<List<Product>> GetNewReleases()
    {
        return await _context.Products.Where(x => x.NewRelease)
           .ToListAsync();
    }

    public async Task<List<Product>> GetRelatedProducts(int categoryId)
    {
        return await _context.Products
            .Where(x => x.CategoryId == categoryId)
            .Take(4)
            .ToListAsync();
    }

    public async Task<List<Product>> Search(string searchParam)
    {
        return await _context.Products
            .Where(x => string.IsNullOrWhiteSpace(searchParam) || x.Name.Contains(searchParam))
            .ToListAsync();
    }
}
