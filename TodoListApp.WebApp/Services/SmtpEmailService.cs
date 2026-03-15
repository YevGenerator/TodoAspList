using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace TodoListApp.WebApp.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration configuration;

    public SmtpEmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var smtpSettings = this.configuration.GetSection("SmtpSettings");
        var from = new MailAddress(smtpSettings["SenderEmail"] ?? string.Empty, smtpSettings["SenderName"] ?? string.Empty);
        using var mailMessage = new MailMessage
        {
            From = from,
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);

        using var client = new SmtpClient(smtpSettings["Server"], int.Parse(smtpSettings["Port"] ?? "587", CultureInfo.CurrentCulture))
        {
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true"),
        };

        await client.SendMailAsync(mailMessage);
    }
}
