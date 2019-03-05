using System.Net;
using System.Net.Mail;
using System.Configuration;
using BLL.Interfaces;

namespace BLL.Services
{
    public class EmailService : IEmailService
    {
        public const string SUBJECT_NEW_TEAM_MEMBER = "A new team member!";

        public const string BODY_NEW_TEAM_MEMBER =
            "<p>Hello {0},</p>" +
            "<br />" +
            "<p>You have just joined to the team \"{1}\"! My congratulations!</p>" +
            "<p>Please, login and manage your tasks.</p>" +
            "<br />" +
            "<p>Have a nice day,<br />" +
            "{2}</p>";

        public void Send(string from, string to, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            var smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["smtp.host"];
            smtp.Port = int.Parse(ConfigurationManager.AppSettings["smtp.port"]);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtp.user"],
                                                     ConfigurationManager.AppSettings["smtp.password"]);

            smtp.Send(mail);
        }
    }
}
