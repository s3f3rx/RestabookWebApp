namespace RestabookWebApp.Services;

public interface IEmailManager
{
    void SendEmail(string email, string body, string subject,string text);
}