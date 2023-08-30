using System.ComponentModel.DataAnnotations;

namespace GadgetHub.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Display name is required")]
    public string DisplayName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }
}
