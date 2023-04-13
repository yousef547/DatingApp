using DatingApp_API.DTOs;
using DatingApp_API.Entities;
using DatingApp_API.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp_API.Interface
{
    public interface IUserRepository
    {
        void Update(AppUser uers);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string username);
        Task<MemberDto> GetMemberAsync(string username);
        Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
    }
}
