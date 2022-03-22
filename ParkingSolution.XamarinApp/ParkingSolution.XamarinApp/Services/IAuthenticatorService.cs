using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public interface IAuthenticatorService
    {
        string Role { get; }
        Task<bool> IsCorrectAsync(string phoneNumber, string password);
    }
}