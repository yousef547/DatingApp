using DatingApp_API.DTOs;
using DatingApp_API.Entities;
using DatingApp_API.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp_API.Interface
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likeUserId);
        Task<bool> AddLike(UserLike like);
        Task<AppUser> GetUserWithLikes(int UserId);
        Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams);

    }
}
