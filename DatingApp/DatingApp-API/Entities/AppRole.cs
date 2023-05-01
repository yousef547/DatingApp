using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DatingApp_API.Entities
{
    public class AppRole :IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
