using Microsoft.AspNetCore.Identity;

namespace GadgetHub.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; }
}
