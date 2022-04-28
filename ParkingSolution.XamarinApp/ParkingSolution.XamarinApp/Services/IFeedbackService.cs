using System.Threading.Tasks;

namespace ParkingSolution.XamarinApp.Services
{
    public interface IFeedbackService
    {
        Task Inform(object message);
        Task<bool> Ask(object question);
        Task Warn(object warning);
        Task InformError(object description);
    }
}
