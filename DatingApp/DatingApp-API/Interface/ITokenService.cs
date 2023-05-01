using DatingApp_API.Entities;
using System.Threading.Tasks;

namespace DatingApp_API.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser usser);
    }
}
