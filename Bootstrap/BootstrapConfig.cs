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

namespace Bootstrap
{
    public static class BootstrapConfig
    {
        private const string ServiceNamespace = "BLL.Services";

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
            var serviceAssembly = Assembly.Load("BLL");

            var serviceRegistrations = serviceAssembly
                .GetExportedTypes()
                .Where(type => type.Namespace == ServiceNamespace && type.GetInterfaces().Any())
                .Select(type => new
                {
                    Interface = type.GetInterfaces().Single(),
                    Implementation = type
                });

            foreach (var reg in serviceRegistrations)
                services.AddTransient(reg.Interface, reg.Implementation);

            // Adding Identity for working with users and with their roles.
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
