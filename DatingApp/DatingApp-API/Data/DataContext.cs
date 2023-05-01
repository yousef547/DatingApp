using DatingApp_API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_API.Data
{
    public class DataContext:IdentityDbContext<AppUser,AppRole,int,
        IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>, IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }
        //public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder bulider) {
            base.OnModelCreating(bulider);

            bulider.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            bulider.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            bulider.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            bulider.Entity<UserLike>()
                .HasOne(s =>s.SourceUser)
                .WithMany(l=>l.LikedUsers)
                .HasForeignKey(s=>s.SourceUserId)
                .OnDelete(DeleteBehavior.Restrict);

            bulider.Entity<Message>()
                .HasOne(k => k.Recipient)
                .WithMany(m => m.MessageReceived)
                .OnDelete(DeleteBehavior.Restrict);

            bulider.Entity<Message>()
               .HasOne(k => k.Sender)
               .WithMany(m => m.MessageSent)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
