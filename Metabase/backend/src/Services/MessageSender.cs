using System.Threading.Tasks;

namespace Metabase.Services
{
    // TODO Implement properly
    public class MessageSender
      : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            System.Console.WriteLine($"{email} # {subject} # {message}");
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            System.Console.WriteLine($"{number} # {message}");
            return Task.FromResult(0);
        }
    }
}
