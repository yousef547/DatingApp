using DatingApp_API.DTOs;
using DatingApp_API.Entities;
using DatingApp_API.Extensions;
using DatingApp_API.Helpers;
using DatingApp_API.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.Data
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;
        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddLike(UserLike like)
        {
            try
            {
                await _context.Likes.AddAsync(like);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likeUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likeUserId);
        }

        public async Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();
            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(users => new LikeDto
            {
                UserName = users.UserName,
                KnowAs = users.KnownAs,
                Age = users.DateOfBirth.CalculatedAge(),
                City = users.City,
                PhotoUrl = users.Photos.FirstOrDefault(p => p.IsMain).Url,
                Id = users.Id,
            });
            return await PageList<LikeDto>.CreateAsync(likedUsers, likesParams.pageNumber, likesParams.pageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int UserId)
        {
            return await _context.Users
                 .Include(x => x.LikedByUsers)
                 .FirstOrDefaultAsync(x => x.Id == UserId);
        }
    }
}
