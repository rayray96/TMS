using BLL.Interfaces;
using BLL.Services;
using DAL.Entities;
using DAL.EF;
using DAL.Interfaces;
using DAL.Identity;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

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
