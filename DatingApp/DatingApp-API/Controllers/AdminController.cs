using DatingApp_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUserWithRoles()
        {
            var user = await _userManager.Users.Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();
            return Ok(user);

        }

        [HttpPost("edit-roles/{username}")]

        public async Task<ActionResult> EditRoles(string username , [FromQuery] string roles)
        {
            var selectedRole = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);
            if(user == null) return NotFound("Cluld not find user");
            var userRoles = await _userManager.GetRolesAsync(user);
            var addRole = selectedRole.Except(userRoles);
            var result = await _userManager.AddToRolesAsync(user, addRole) ;
            if (!result.Succeeded) return BadRequest("Failed To Remover from roles");
            var removeRole = userRoles.Except(selectedRole);

            result = await _userManager.RemoveFromRolesAsync(user, removeRole);
            if (!result.Succeeded) return BadRequest("Faild To Remove From roles");
            return Ok(await _userManager.GetRolesAsync(user));
        }



        [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("photos-to-Moderator")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Only admins or roderator  can see this");
        }
    }
}
