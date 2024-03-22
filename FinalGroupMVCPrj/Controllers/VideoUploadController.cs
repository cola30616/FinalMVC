using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using FinalGroupMVCPrj.Interface;
using FinalGroupMVCPrj.Models;
namespace FinalGroupMVCPrj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoUploadController : ControllerBase
    {
        private IVideoUploadService _videoUploadService;
        private readonly LifeShareLearnContext _lifeShareLearnContext;
        public VideoUploadController(IVideoUploadService videoUploadService, LifeShareLearnContext lifeShareLearnContext)
        {
            _videoUploadService = videoUploadService;
            _lifeShareLearnContext = lifeShareLearnContext;
        }

        [HttpPost]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            var result = await _videoUploadService.AddVideoAsync(file);

            return Ok(result);
        }

       


    }
}
