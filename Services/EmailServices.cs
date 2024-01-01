using Humanizer;
using System.Net;
using System.Net.Mail;

namespace XtraBlogWebsite.Services
{
    public interface IEmailService
    {
        public Task SendMail(string email,string subject,string message);
    }
    public class EmailServices : IEmailService
    {
        public async Task SendMail(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com",587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("ayxan.kerimli.20037@gmail.com", "hlpapcictxhmdohg")
            };
            await client.SendMailAsync(new MailMessage(from: "ayxan.kerimli.20037@gmail.com", to: email, subject, message));
        }
    }
}
