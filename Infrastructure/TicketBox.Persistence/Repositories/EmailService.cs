using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.DTOs;
using TicketBox.Application.Interfaces;

namespace TicketBox.Persistence.Repositories
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body, List<EmailAttachmentDto>? attachments = null)
        {
            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress FromMailboxAddress = new MailboxAddress("TicketBox", "email");
            mimeMessage.From.Add(FromMailboxAddress);

            MailboxAddress ToMailboxAddress = new MailboxAddress("User", to);
            mimeMessage.To.Add(ToMailboxAddress);

            mimeMessage.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment.FileName, attachment.Content, new ContentType("image", "png"));
                }
            }
            mimeMessage.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("email", "apikey");
            await smtp.SendAsync(mimeMessage);
            await smtp.DisconnectAsync(true);
        }
    }
}
