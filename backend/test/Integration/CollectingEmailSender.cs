using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metabase.Tests.Integration;

public sealed class CollectingEmailSender
    : Services.IEmailSender
{
    private readonly List<Email> _emails;

    public IReadOnlyCollection<Email> Emails => _emails.AsReadOnly();

    public CollectingEmailSender()
    {
        _emails = new List<Email>();
    }

    public void Clear()
    {
        _emails.Clear();
    }

    public Task SendAsync(
        (string name, string address) recipient,
        string subject,
        string body
    )
    {
        _emails.Add(
            new Email
            (
                recipient,
                subject,
                body
            )
        );
        return Task.FromResult(0);
    }

    public sealed record Email(
        (string name, string address) Recipient,
        string Subject,
        string Body
    );
}