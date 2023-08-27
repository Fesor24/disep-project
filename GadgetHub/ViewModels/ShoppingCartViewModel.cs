using GadgetHub.Entities;

namespace GadgetHub.ViewModels;

public class ShoppingCartViewModel
{
    public string Id { get; set; }

    public List<ShoppingCartItemViewModel> Items { get; set; } = new();

    public float SubTotals { get; set; }

    public string StrSubTotals { get; set; }  

    public float DeliveryCharges => 3000f;

    public string StrDeliveryFee { get; set; }

    public string StrTotals { get; set; }

    public float Totals => SubTotals + DeliveryCharges;
}
