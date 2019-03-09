using System;
using System.Linq;
using DAL.EF;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// This Program.cs for testing system only!
// TODO: Don't forget to delete before publishing!
namespace DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.Load("DAL");
            Console.WriteLine(assembly.FullName);

            Console.ReadKey();
        }
    }
}
