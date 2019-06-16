using DAL.EF;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Bootstrap
{
    public static class TestDataGenerator
    {
        public static void Initialize(ApplicationDbContext context)
        {
            CreateUsers(context);
            CreateRefreshTokens(context);

            context.SaveChanges();
        }

        private static void CreateUsers(ApplicationDbContext context)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var store = new UserStore<ApplicationUser>(context);

            if (!context.Users.Any())
            {
                ApplicationUser[] users = {
                new ApplicationUser
                {
                    Id = "1a",
                    FName = "Stephen",
                    LName = "King",
                    Email = "nightmare@tms.com",
                    NormalizedEmail = "NIGHTMARE@TMS.COM",
                    UserName = "Nightmare",
                    NormalizedUserName = "NIGHTMARE",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = "2b",
                    FName = "Jason",
                    LName = "Voorhees",
                    Email = "slasher@tms.com",
                    NormalizedEmail = "SLASHER@TMS.COM",
                    UserName = "Slasher",
                    NormalizedUserName = "SLASHER",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = "3c",
                    FName = "Mike",
                    LName = "Myers",
                    Email = "murder@tms.com",
                    NormalizedEmail = "MURDER@TMS.COM",
                    UserName = "Murder",
                    NormalizedUserName = "MURDER",
                    SecurityStamp = Guid.NewGuid().ToString()
                }};
                context.Users.AddRange(users);

                foreach (var user in users)
                    user.PasswordHash = hasher.HashPassword(user, "qwerty123!");
                
                store.AddToRoleAsync(users[0], "ADMIN");
                store.AddToRoleAsync(users[1], "MANAGER");
                store.AddToRoleAsync(users[2], "WORKER");
            }
        }

        private static void CreateRefreshTokens(ApplicationDbContext context)
        {
            context.RefreshTokens.Add(
                new RefreshToken
                {
                    Id = 12,
                    UserId = "2b",
                    Token = "1a2b3c4d5e",
                    Expires = DateTime.Now.AddDays(1),
                });
        }
    }
}