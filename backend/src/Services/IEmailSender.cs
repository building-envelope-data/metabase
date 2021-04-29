using System.Threading.Tasks;

namespace Metabase.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string address, string subject, string message);
    }
}