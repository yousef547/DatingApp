using DatingApp_API.Data;
using DatingApp_API.DTOs;
using DatingApp_API.Entities;
using DatingApp_API.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp_API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _itokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService TokenService)
        {
            _itokenService = TokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username))
            {
                return BadRequest("Username is taken");
            }
            //using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDTO.Username.ToLower(),
                //PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                //PasswordSalt = hmac.Key
            };
            var result = await _userManager.CreateAsync(user,registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if(!roleResult.Succeeded) return BadRequest(result.Errors);
            return new UserDto
            {
                Username = user.UserName, 
                Token=await _itokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
        {
            var user = await _userManager.Users.Include(x=>x.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid username");
            var result = await _signInManager
                .CheckPasswordSignInAsync(user,loginDto.Password,false);
            if(!result.Succeeded) return Unauthorized();
            return new UserDto
            {
                Username = user.UserName,
                Token = await _itokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(c => c.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
