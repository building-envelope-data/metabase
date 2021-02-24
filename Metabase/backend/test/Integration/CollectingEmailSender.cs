using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metabase.Tests.Integration
{
    public sealed class CollectingEmailSender
      : Services.IEmailSender
    {
        private readonly List<Email> _emails;

        public IReadOnlyCollection<Email> Emails
        {
            get { return _emails.AsReadOnly(); }
        }

        public CollectingEmailSender()
        {
            _emails = new List<Email>();
        }

        public void Clear()
        {
            _emails.Clear();
        }

        public Task SendEmailAsync(string address, string subject, string message)
        {
            _emails.Add(
                new Email
                (
                    Address: address,
                    Subject: subject,
                    Message: message
                 )
            );
            return Task.FromResult(0);
        }

        public record Email(
            string Address,
            string Subject,
            string Message
        );
    }
}