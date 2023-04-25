using DatingApp_API.DTOs;
using DatingApp_API.Entities;
using DatingApp_API.Extensions;
using DatingApp_API.Helpers;
using DatingApp_API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp_API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepository;
        public LikesController(IUserRepository userRepository ,ILikeRepository likeRepository)
        {
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var likeUser = await _userRepository.GetUserByUserNameAsync(username);
            var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);
            if(likeUser == null) return NotFound();
            if (sourceUser.UserName == username) return BadRequest("You connot like yourself");
            var userLike = await _likeRepository.GetUserLike(sourceUserId, likeUser.Id);
            if (userLike != null) return BadRequest("You already like this user");
            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likeUser.Id,
            };
            await _likeRepository.AddLike(userLike);
            if(await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed To Like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users =  await _likeRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalPages,users.TotalPages);
            return Ok(users);
        }
    }
}
