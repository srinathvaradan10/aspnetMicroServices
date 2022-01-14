using OrderingApplication.Models;
using System.Threading.Tasks;

namespace OrderingApplication.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
