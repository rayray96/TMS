using DAL.EF;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
            CreateStatuses(context);
            CreatePriorities(context);
            CreateTasks(context);
            CreateTeams(context);
            CreatePersons(context);

            context.SaveChanges();
        }

        #region Creating a test data.

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
                },
                new ApplicationUser
                {
                    Id = "4d",
                    FName = "Freddy",
                    LName = "Krueger",
                    Email = "krueger@tms.com",
                    NormalizedEmail = "KRUEGER@TMS.COM",
                    UserName = "Killer",
                    NormalizedUserName = "KILLER",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = "5e",
                    FName = "Bubba",
                    LName = "Sawyer",
                    Email = "sawyer@tms.com",
                    NormalizedEmail = "SAWYER@TMS.COM",
                    UserName = "Assassin",
                    NormalizedUserName = "ASSASSIN",
                    SecurityStamp = Guid.NewGuid().ToString()
                }};

                context.Users.AddRange(users);

                foreach (var user in users)
                    user.PasswordHash = hasher.HashPassword(user, "qwerty123!");

                store.AddToRoleAsync(users[0], "ADMIN");
                store.AddToRoleAsync(users[1], "MANAGER");
                store.AddToRoleAsync(users[2], "WORKER");
                store.AddToRoleAsync(users[3], "WORKER");
                store.AddToRoleAsync(users[4], "MANAGER");
            }
        }

        private static void CreateRefreshTokens(ApplicationDbContext context)
        {
            if (!context.RefreshTokens.Any())
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

        private static void CreatePersons(ApplicationDbContext context)
        {
            if (!context.People.Any())
            {
                Person[] people = {
                new Person
                {
                    Id = 2,
                    FName = "Jason",
                    LName = "Voorhees",
                    Email = "slasher@tms.com",
                    UserName = "Slasher",
                    Role = "Manager",
                    UserId = "2b",
                    TeamId = 7
                },
                new Person
                {
                    Id = 3,
                    FName = "Mike",
                    LName = "Myers",
                    Email = "murder@tms.com",
                    UserName = "Murder",
                    Role = "Worker",
                    UserId = "3c"
                },
                new Person
                {
                    Id = 4,
                    FName = "Freddy",
                    LName = "Krueger",
                    Email = "krueger@tms.com",
                    UserName = "Killer",
                    Role = "Worker",
                    UserId = "4d",
                    TeamId = 7
                },              
                new Person
                {
                    Id = 5,
                    FName = "Bubba",
                    LName = "Sawyer",
                    Email = "sawyer@tms.com",
                    UserName = "Assassin",
                    Role = "Manager",
                    UserId = "5e"
                }};

                context.People.AddRange(people);
            }
        }

        private static void CreateTasks(ApplicationDbContext context)
        {
            if (!context.TaskInfos.Any())
            {
                TaskInfo[] tasks =
                {
                    new TaskInfo
                    {
                        Id = 2,
                        AssigneeId = 3,
                        AuthorId = 2,
                        Deadline = DateTime.Now.AddDays(8),
                        StartDate = DateTime.Now.AddDays(1),
                        Description = "Bla bla bla... DAL",
                        Name = "Develope DAL",
                        StatusId = 1,
                        PriorityId = 1,
                        Progress = 0
                    },
                    new TaskInfo
                    {
                        Id = 3,
                        AssigneeId = 3,
                        AuthorId = 2,
                        Deadline = DateTime.Now.AddDays(7),
                        StartDate = DateTime.Now.AddDays(1),
                        Description = "Bla bla bla... BLL",
                        Name = "Develope BLL",
                        StatusId = 2,
                        PriorityId = 2,
                        Progress = 20
                    },
                    new TaskInfo
                    {
                        Id = 4,
                        AssigneeId = 3,
                        AuthorId = 2,
                        Deadline = DateTime.Now.AddDays(10),
                        StartDate = DateTime.Now.AddDays(1),
                        Description = "Bla bla bla... UI",
                        Name = "Develope UI",
                        StatusId = 5,
                        PriorityId = 3,
                        Progress = 80
                    },
                };

                context.TaskInfos.AddRange(tasks);
            }
        }

        private static void CreateTeams(ApplicationDbContext context)
        {
            if (!context.Teams.Any())
            {
                context.Teams.Add(
                    new Team
                    {
                        Id = 7,
                        TeamName = "Cups"
                    });
            }
        }

        private static void CreateStatuses(ApplicationDbContext context)
        {
            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(
                new Status[]
                {
                    new Status{ Id = 1, Name = "Not Started" },
                    new Status{ Id = 2, Name = "In Progress"},
                    new Status{ Id = 3, Name = "Test"},
                    new Status{ Id = 4, Name = "Almost Ready"},
                    new Status{ Id = 5, Name = "Executed"},
                    new Status{ Id = 6, Name = "Completed"},
                    new Status{ Id = 7, Name = "Canceled"}
                });
            }
        }

        private static void CreatePriorities(ApplicationDbContext context)
        {
            if (!context.Priorities.Any())
            {
                context.Priorities.AddRange(
                new Priority[]
                {
                    new Priority{ Id = 1, Name = "Low" },
                    new Priority{ Id = 2, Name = "Middle" },
                    new Priority{ Id = 3, Name = "High" }
                });
            }
        }
 
        #endregion
    }
}