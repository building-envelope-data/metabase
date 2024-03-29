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

        public Task SendAsync(
            (string name, string address) to,
            string subject,
            string body
        )
        {
            _emails.Add(
                new Email
                (
                    To: to,
                    Subject: subject,
                    Body: body
                 )
            );
            return Task.FromResult(0);
        }

        public sealed record Email(
            (string name, string address) To,
            string Subject,
            string Body
        );
    }
}