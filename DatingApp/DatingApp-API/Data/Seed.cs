﻿using DatingApp_API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatingApp_API.Data
{
    public class Seed
    {

        public static async Task UserSeed(UserManager<AppUser> userManger,RoleManager<AppRole> roleManager)
        {
            if (await userManger.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;
            var roles = new List<AppRole>
            {
                new AppRole{ Name = "Member"},
                new AppRole{ Name = "Admin"},
                new AppRole{ Name = "Moderator"},
            };

            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManger.CreateAsync(user, "P@$$W0rd");
                await userManger.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };
            await userManger.CreateAsync(admin, "P@$$W0rd");
            await userManger.AddToRolesAsync(admin, new[] { "Admin","Moderator" });


        }
    }
}
