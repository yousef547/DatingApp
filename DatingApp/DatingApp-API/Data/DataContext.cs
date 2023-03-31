using DatingApp_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_API.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
