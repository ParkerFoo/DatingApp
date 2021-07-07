using API.Entites;

namespace API.interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);

    }
}