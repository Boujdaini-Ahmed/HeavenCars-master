using System.Threading.Tasks;

namespace HeavenCars.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}