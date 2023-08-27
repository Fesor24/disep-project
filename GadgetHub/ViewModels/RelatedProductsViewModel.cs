namespace GadgetHub.ViewModels;

public class RelatedProductsViewModel
{
    public ProductsViewModel Product { get; set; } = new();

    public List<ProductsViewModel> RelatedProducts { get; set; } = new();
}
