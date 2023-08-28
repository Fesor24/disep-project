using GadgetHub.Entities.Identity;

namespace GadgetHub.Services.Abstractions;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}
