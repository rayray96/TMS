using BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace BLL.Services
{
    public class EmailService : IEmailService
    {
        public const string SUBJECT_NEW_TEAM_MEMBER = "A new team member!";
        public const string SUBJECT_NEW_TASK = "You have a new task!";
        public const string SUBJECT_EXECUTED_TASK = "Your team member has finished task!";

        public const string BODY_NEW_TASK =
            "<p>Hello {0},</p>" +
            "<br />" +
            "<p>You have just received the task \"{1}\" from your manager! Let's begin to work!</p>" +
            "<br />" +
            "<p>Have a nice day,<br />" +
            "{2}</p>";

        public const string BODY_NEW_TEAM_MEMBER =
            "<p>Hello {0},</p>" +
            "<br />" +
            "<p>You have just joined to the team \"{1}\"! My congratulations!</p>" +
            "<p>Please, login and manage your tasks.</p>" +
            "<br />" +
            "<p>Have a nice day,<br />" +
            "{2}</p>";

        public const string BODY_EXECUTED_TASK =
            "<p>Hello {0},</p>" +
            "<br />" +
            "<p>Your team member {1} has just executed task \"{2}\"! Let's check it out!</p>" +
            "<br />" +
            "<p>Have a nice day,<br />" +
            "Task Management System</p>";

        IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", true)
                   .Build();

        public void Send(string from, string to, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            var smtp = new SmtpClient();
            smtp.Host = configuration.GetSection("Email:smtp.host").Value;
            smtp.Port = int.Parse(configuration.GetSection("Email:smtp.port").Value);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(configuration.GetSection("Email:smtp.user").Value,
                                                     configuration.GetSection("Email:smtp.password").Value);

            smtp.Send(mail);
        }
    }
}
