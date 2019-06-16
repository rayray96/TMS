using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using DAL.Entities;
using DAL.Identity;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Bootstrap
{
    public static class BootstrapConfig
    {
        public static void RegisterApplicationServices(this IServiceCollection services, string nameConnection, IHostingEnvironment HostingEnvironment)
        {
            // Creating connection with our database.
            if (HostingEnvironment.IsDevelopment() || HostingEnvironment.EnvironmentName == "Testing")
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    var contextOptions = options.UseInMemoryDatabase($"db-{Guid.NewGuid()}").Options;

                    using (var context = new ApplicationDbContext(contextOptions))
                    {
                        TestDataGenerator.Initialize(context);
                    }

                }, ServiceLifetime.Transient);
            }
            else
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile($"appsettings.{HostingEnvironment.EnvironmentName}.json", true)
                       .Build();

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString(nameConnection));
                });
            }

            // Registering repositories.
            services.AddScoped<IRepository<Person>, PersonRepository>();
            services.AddScoped<IRepository<TaskInfo>, TaskRepository>();
            services.AddScoped<IRepository<Team>, TeamRepository>();
            services.AddScoped<IRepository<Status>, StatusRepository>();
            services.AddScoped<IRepository<Priority>, PriorityRepository>();
            services.AddScoped<IRepository<RefreshToken>, RefreshTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Registering units of work.
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();

            // Registering services.
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            // Adding Identity for working with users and with their roles.
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;

                opts.Password.RequiredLength = 6;
                opts.Password.RequiredUniqueChars = 0;
                opts.Password.RequireDigit = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddDefaultTokenProviders();
        }
    }
}
