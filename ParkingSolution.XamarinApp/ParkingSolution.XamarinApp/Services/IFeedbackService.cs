using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public interface IFeedbackService
    {
        Task Inform(string message);
        Task<bool> Ask(string question);
        Task Warn(string warning);
        Task InformError(string description);
    }
}
