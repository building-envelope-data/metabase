using System.Threading.Tasks;

namespace Metabase.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}