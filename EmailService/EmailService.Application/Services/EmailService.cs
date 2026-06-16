using EmailService.Application.Events;  
using EmailService.Application.Interfaces.Messages;
using EmailService.Application.Interfaces.Services;
using EmailService.Application.Mappers;
using EmailService.Domain.Models;

namespace EmailService.Application.Services;

public class EmailService(
    IEmailSender sender, 
    IMessageProducer<ApproveEmailEvent> approveProducer) 
    : IEmailService
{
    public async Task<EmailMessage> SendAsync(SendEmailEvent message)
    {
        var emailMessage = message.ToEmailMessage();    
        await sender.SendAsync(emailMessage);
        return emailMessage;
    }

    public async Task ApproveMessageAsync(EmailMessage message)
    {
        var approveEvent = message.ToApproveMessageEvent();
        await approveProducer.ProduceAsync(approveEvent);
    }
}