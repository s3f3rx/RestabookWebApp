using System.Net;
using System.Net.Mail;
using System.Text;

namespace RestabookWebApp.Services;

public class EmailManager(IConfiguration configuration) : IEmailManager
{
    public void SendEmail(string email, string body, string subject, string text)
    {
        SmtpClient client = new(configuration["EmailSetting:Host"],
            int.Parse(configuration["EmailSetting:Port"]));
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(configuration["EmailSetting:Email"],
            configuration["EmailSetting:Password"]);

        MailMessage mailMessage = new();
        mailMessage.From = new MailAddress(configuration["EmailSetting:Email"]);
        mailMessage.To.Add(email);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        StringBuilder mailBody = new();
        mailBody.AppendFormat($"<h1>{subject}</h1>");
        mailBody.AppendFormat("<br />");
        mailBody.Append($"<p>{body} <br /> {text} <a href='{body}'>Click here</a></p>");
        mailMessage.Body = mailBody.ToString();

        client.Send(mailMessage);
    }
}