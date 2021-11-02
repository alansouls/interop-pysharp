using System.Threading.Tasks;

namespace SampleInterop.Services
{
    public interface IFacialRecognitionService
    {
        Task<bool> CheckFacesAsync(string knownImagePath, string unknownImagePath);
    }
}