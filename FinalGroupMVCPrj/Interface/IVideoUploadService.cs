using CloudinaryDotNet.Actions;

namespace FinalGroupMVCPrj.Interface
{
    public interface IVideoUploadService
    {
        Task<VideoUploadResult> AddVideoAsync(IFormFile file);

        Task<DeletionResult> DeleteVideoAsync(string id);
    }
}
