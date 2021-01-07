using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metabase.Tests.Integration
{
    public sealed class CollectingSmsSender
      : Services.ISmsSender
    {
        private readonly List<Sms> _smses;

        public IReadOnlyCollection<Sms> Smses
        {
            get { return _smses.AsReadOnly(); }
        }

        public CollectingSmsSender()
        {
            _smses = new List<Sms>();
        }

        public void Clear()
        {
            _smses.Clear();
        }

        public Task SendSmsAsync(string number, string message)
        {
            _smses.Add(
                new Sms(
                    Number: number,
                    Message: message
                )
            );
            return Task.FromResult(0);
        }

        public record Sms(
            string Number,
            string Message
        );
    }
}
