using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class TeacherController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;  //資料庫
        public TeacherController(LifeShareLearnContext context)
        {
            _context = context;
        }

        // GET: Teacher/List
        //動作簡述：回傳老師清單的頁面
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        // GET: Teacher/List
        //動作簡述：回傳單一老師資訊的頁面
        [HttpGet]
        public IActionResult Info()
        {
            return View();
        }
         // GET: Teacher/TeacherPhoto
        //動作簡述：回傳老師頭像的url
        [HttpGet]
        public async Task<IActionResult> TeacherPhoto(int? id)
        {
            string blobDataURL = "";
            TTeacher? teacher = await _context.TTeachers.FirstOrDefaultAsync(t => t.FTeacherId == id);
            byte[]? image = teacher?.FTeacherProfilePic;
            if (image == null || image.Length == 0)
            {
                blobDataURL = "";

            }
            else
            {
                string base64String = Convert.ToBase64String(image);
                blobDataURL = $"data:image/jpeg;base64,{base64String}";
            }
            return Content(blobDataURL);
        }
    }
}
