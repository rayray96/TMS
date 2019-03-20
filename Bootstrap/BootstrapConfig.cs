using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.EF;
using DAL.Interfaces;
using DAL.UnitOfWork;
using BLL.Services;
using BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using BLL.DTO;
using DAL.Identity;

namespace Bootstrap
{
    public static class BootstrapConfig
    {
        public static void RegisterApplicationServices(this IServiceCollection services, string nameConnection)
        {
            // Creating connection with our database.
            IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

            var connectionString = configuration.GetConnectionString(nameConnection);
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // Adding unit of work.
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IIdentityUnitOfWork, IdentityUnitOfWork>();

            // Adding services.
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IPriorityService, PriorityService>();
            services.AddTransient<IStatusService, StatusService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();

            // Adding Identity for working with users and with their roles.
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;

                opts.Password.RequiredLength = 6;
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
