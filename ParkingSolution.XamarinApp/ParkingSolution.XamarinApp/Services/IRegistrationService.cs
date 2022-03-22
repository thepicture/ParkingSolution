using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public interface IRegistrationService<TIdentity>
    {
        string ValidationResult { get; }
        Task<bool> IsRegisteredAsync(TIdentity identity);
    }
}
