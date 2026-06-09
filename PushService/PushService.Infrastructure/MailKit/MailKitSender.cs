using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using PushService.Application.Interfaces.Services;
using PushService.Domain.Models;
using PushService.Infrastructure.Options;

namespace PushService.Infrastructure.MailKit;

public class MailKitSender(IOptions<MailKitOptions> options) : IPushSender
{
    public async Task SendAsync(PushMessage message)
    {
        using var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(options.Value.Name, options.Value.DisplayEmail));
        mimeMessage.To.Add(new MailboxAddress(message.Name ?? string.Empty, message.Email));
        mimeMessage.Subject = message.Subject ?? string.Empty;
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message.Body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(options.Value.Host, options.Value.Port);
        await client.AuthenticateAsync(options.Value.RealEmail, options.Value.Password);

        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }
}