namespace BLL.Interfaces
{
    /// <summary>
    /// Service for sending email notifications
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sending email notifications to system clients
        /// </summary>
        /// <param name="from"> Sender </param>
        /// <param name="to"> Recipent </param>
        /// <param name="subject"> A topic of a message </param>
        /// <param name="body"> Content of a message </param>
        void Send(string from, string to, string subject, string body);
    }
}
