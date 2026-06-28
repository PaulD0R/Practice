using MailKit.Net.Smtp;
using EmailService.Application.Interfaces.Services;
using EmailService.Domain.Models;
using MimeKit;

namespace EmailService.Infrastructure.MailKit;

public class MailKitSender : IEmailSender
{
    public async Task SendAsync(EmailMessage message, SmtpOptions options)
    {
        using var mimeMessage = new MimeMessage();
 
        mimeMessage.From.Add(new MailboxAddress(options.Name, options.RealEmail));
        mimeMessage.To.Add(new MailboxAddress(string.Empty, message.Email));
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message.Body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(options.Host, options.Port);
        await client.AuthenticateAsync(options.RealEmail, options.Password);
        
        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }
}