using DatingApp_API.Entities;

namespace DatingApp_API.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser usser);
    }
}
