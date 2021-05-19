using System.Threading.Tasks;

namespace Metabase.Services
{
    public interface IEmailSender
    {
        public Task SendAsync(
            string to,
            string subject,
            string body
        );
    }
}