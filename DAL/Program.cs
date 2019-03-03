using System;
using System.Linq;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=TMSDatabase; Trusted_Connection=True;").Options;

            using (ApplicationContext con = new ApplicationContext(options))
            {
                var statuses = con.Statuses.ToList();

                foreach (var status in statuses)
                {
                    Console.WriteLine($"Status: {status.Name}");
                } 
            }
            Console.ReadKey();
        }
    }
}
