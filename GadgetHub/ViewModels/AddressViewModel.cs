using System.ComponentModel.DataAnnotations;

namespace GadgetHub.ViewModels;

public class AddressViewModel
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Street is required")]
    public string Street { get; set; }

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }

    [Required(ErrorMessage = "State is required")]
    public string State { get; set; }

    public string Email { get; set; } = string.Empty;
}
