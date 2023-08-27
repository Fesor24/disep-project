using GadgetHub.Entities;

namespace GadgetHub.DataAccess.Abstractions;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<List<Product>> GetHighlyRatedProducts();

    Task<List<Product>> GetNewReleases();

    Task<List<Product>> Search(string searchParam);

    Task<List<Product>> GetRelatedProducts(int categoryId);
}
