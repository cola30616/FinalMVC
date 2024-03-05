using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherAdminController : UserInfoController
    {
        //■ ==========================     翊妏作業區      ==========================■
        // GET: TeacherAdmin/LessonList
        //動作簡述：回傳老師課程清單資訊
        private readonly LifeShareLearnContext _context;
        public TeacherAdminController(LifeShareLearnContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult LessonList( )
        {
            return View();
        }
        //■ ==========================     東霖作業區      ==========================■



        //■ ==========================     子謙作業區      ==========================■
        [HttpGet]
        public IActionResult VideoIntro()
        {           
            return View();
        }
        [HttpGet]
        public IActionResult VEdit(int? id)
        {
            var videoList = _context.TVideoUploadUrls
            .Where(t => t.FVideoUploadUrlId == id)           
            .ToList();
            return View(videoList);
        }

        //API Get
        //獲得目前老師的影片資訊
        [HttpGet]
        public IActionResult GetVideoData()
        {
            //TVideoUploadUrl videoUploadUrl = _context.TVideoUploadUrls.FirstOrDefault(v => v.);
            var videoList = _context.TVideoUploadUrls
            .Where(t => t.FTeacher.FMemberId == GetCurrentMemberId())
            .Select(u => new {
                fVideoName = u.FVideoName,
                fVideoPath = u.FVideoPath,
                fUploadTime = u.FUploadTime,
                fVideoUploadUrlId = u.FVideoUploadUrlId
            })
            .ToList();
            return Json(new { data = videoList });
        }

        //API Delete
        //刪除目前影片
        [HttpDelete]
        public async Task<IActionResult> DeleteVideoData(int? id)
        {
            var video = await _context.TVideoUploadUrls.FirstOrDefaultAsync(v => v.FVideoUploadUrlId == id);
            if (video != null)
            {
                _context.TVideoUploadUrls.Remove(video);
                await _context.SaveChangesAsync(); 
            }
            return Json(new { success = true, message = "刪除成功" });
        }


    }
}
