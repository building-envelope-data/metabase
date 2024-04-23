using System.Threading.Tasks;

namespace Metabase.Services;

public interface IEmailSender
{
    public Task SendAsync(
        (string name, string address) recipient,
        string subject,
        string body
    );
}