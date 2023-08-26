namespace GadgetHub.Entities;

public class ShoppingCartItem
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public float Price { get; set; }

    public int Quantity { get; set; }

    public int CategoryId { get; set; }


}
