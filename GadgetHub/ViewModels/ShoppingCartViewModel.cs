using GadgetHub.Entities;

namespace GadgetHub.ViewModels;

public class ShoppingCartViewModel
{
    public string Id { get; set; }

    public List<ShoppingCartItemViewModel> Items { get; set; } = new();
}
