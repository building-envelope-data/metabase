using System.Threading.Tasks;

namespace Metabase.Services
{
    public interface IEmailSender
    {
        public Task SendAsync(
            (string name, string address) to,
            string subject,
            string body
        );
    }
}