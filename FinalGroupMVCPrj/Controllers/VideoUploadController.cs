using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using FinalGroupMVCPrj.Interface;
namespace FinalGroupMVCPrj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoUploadController : ControllerBase
    {
        private IVideoUploadService _videoUploadService;
        public VideoUploadController(IVideoUploadService videoUploadService)
        {
            _videoUploadService = videoUploadService;
        }
        [HttpPost]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            var result = await _videoUploadService.AddVideoAsync(file);

            return Ok(result);
        }

        
    }
}
