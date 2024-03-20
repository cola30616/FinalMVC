using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FinalGroupMVCPrj.Interface;
using FinalGroupMVCPrj.Models;
using Microsoft.Extensions.Options;

namespace FinalGroupMVCPrj.APIServices
{
    public class VideoUploadServices : IVideoUploadService
    {
        private readonly Cloudinary _cloudinary;
        public VideoUploadServices(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
                (
                    config.Value.CloudName,
                    config.Value.ApiKey,
                    config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<VideoUploadResult> AddVideoAsync(IFormFile file)
        {

            // 从 IFormFile 中获取文件流
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(file.FileName, stream), // 使用文件流初始化 FileDescription 对象
                    EagerTransforms = new List<Transformation>()
            {
                new EagerTransformation().Width(300).Height(300).Crop("pad").AudioCodec("none"),
                new EagerTransformation().Width(160).Height(100).Crop("crop").Gravity("south").AudioCodec("none")
            },
                    EagerAsync = true
                };

                var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);

                return uploadResult;
            }
        }

        public async Task<DeletionResult> DeleteVideoAsync(string publicid)
        {
            var deleteParams = new DeletionParams(publicid);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
