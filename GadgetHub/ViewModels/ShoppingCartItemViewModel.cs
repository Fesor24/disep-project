namespace GadgetHub.ViewModels;

public class ShoppingCartItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public float Price { get; set; }

    public float SubTotal { get; set; }

    public int Quantity { get; set; }

    public int CategoryId { get; set; }
}
