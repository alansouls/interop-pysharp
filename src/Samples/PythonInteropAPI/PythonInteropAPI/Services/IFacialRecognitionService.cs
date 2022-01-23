using System.Threading.Tasks;

namespace PythonInteropAPI.Services
{
    public interface IFacialRecognitionService
    {
        Task<bool> CheckFacesAsync(string knownImagePath, string unknownImagePath);
    }
}