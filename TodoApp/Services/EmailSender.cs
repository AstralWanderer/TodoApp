using MailKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using MimeKit.Text;

namespace TodoApp.Services;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }
    
    // Add user secrets for sender email address and app password 
    // In the terminal enter the commands, remembering to omit the "<>":
    // "dotnet user-secrets init"
    // "dotnet user-secrets set "EmailSettings:SmtpUser" "<email@domain.com>"
    // "dotnet user-secrets set "EmailSettings:SmtpPass" "<app-password>"
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("TodoApp", _config["EmailSettings:SmtpUser"]));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;
        message.Body = new TextPart("html"){ Text = htmlMessage };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _config["EmailSettings:SmtpServer"],
            int.Parse(_config["EmailSettings:SmtpPort"]),
            SecureSocketOptions.StartTls
        );
        await client.AuthenticateAsync(
            _config["EmailSettings:SmtpUser"],
            _config["EmailSettings:SmtpPass"]
        );
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}