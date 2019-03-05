using System;
using DAL.UnitOfWork;
using BLL.Services;
using BLL.DTO;

namespace PL_Console_
{
    class Program
    {
        static void Main(string[] args)
        {
            EmailService emailService = new EmailService();
            PersonService person = new PersonService(new UnitOfWork(), emailService);

            //person.AddPersonsToTeam();
        }
    }
}
