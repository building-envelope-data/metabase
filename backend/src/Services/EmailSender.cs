using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Metabase.Services;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "About to send email to `{Recipient}` with subject `{Subject}` and body `{Body}`")]
    public static partial void AboutToSendEmail(
        this ILogger<EmailSender> logger,
        (string name, string address) Recipient,
        string Subject,
        string Body
    );
}

public sealed class EmailSender(
    AppSettings appSettings,
    ILogger<EmailSender> logger
)
: IEmailSender
{
    public Task SendAsync(
        (string name, string address) recipient,
        string subject,
        string body
    )
    {
        logger.AboutToSendEmail(recipient, subject, body);
        var message = new MimeMessage();
        message.From.Add(
            new MailboxAddress(
                "Metabase",
                $"metabase@{appSettings.Uri.Host}"
            )
        );
        message.To.Add(
            new MailboxAddress(
                recipient.name,
                recipient.address
            )
        );
        message.Subject = subject;
        message.Body = new TextPart("plain")
        {
            Text = body
        };
        using (var client = new SmtpClient())
        {
            client.Connect(
                appSettings.Email.SmtpHost,
                appSettings.Email.SmtpPort,
                SecureSocketOptions.StartTlsWhenAvailable
            );
            // client.Authenticate("joey", "password");
            client.Send(message);
            client.Disconnect(true);
        }

        return Task.FromResult(0);
    }
}