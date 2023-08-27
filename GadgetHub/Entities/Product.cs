namespace GadgetHub.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public float Price { get; set; }

    public string Image { get; set; }

    public bool NewRelease { get; set; }

    public int Ratings { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }
}
