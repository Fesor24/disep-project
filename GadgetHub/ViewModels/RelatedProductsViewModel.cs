namespace GadgetHub.ViewModels;

public class RelatedProductsViewModel
{
    public ProductsViewModel Product { get; set; }

    public List<ProductsViewModel> RelatedProducts { get; set; }
}
