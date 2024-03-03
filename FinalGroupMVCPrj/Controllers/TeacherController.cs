using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
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

        // GET: Teacher/Info
        //動作簡述：回傳單一老師資訊的頁面
        [HttpGet]
        public IActionResult Info(int id)
        {
            var tr = _context.TTeachers.FindAsync(id);
            return View(tr);
        }
        [HttpGet]
        public IActionResult AllTrInfo()
        {
            var tr = _context.TTeachers
            .Select(t =>  new{ 
                t.FTeacherId,
                t.FTeacherName,
                TeacherProfilePicURL = GetImageDataURL(t.FTeacherProfilePic),
            });
            return Json(tr);
        }

        private string GetImageDataURL(byte[] image)
        {
            string blobDataURL = "";
            if (image != null)
            {
                string base64String = Convert.ToBase64String(image);
                
                blobDataURL = $"data:image/jpeg;base64,{base64String}";
                return blobDataURL;
            }

            return null;
        }

        // GET: Teacher/AllTeachersPhotos
        //動作簡述：回傳所有老師頭像的url
        [HttpGet]
        public async Task<IActionResult> AllTeachersPhotos()
        {
            

            
            List<string> allTeacherData = new List<string>();
            List<TTeacher> allTeachers = await _context.TTeachers.ToListAsync();

            foreach (var teacher in allTeachers)
            { 
               
                byte[]? image = teacher?.FTeacherProfilePic;
                int? id = teacher?.FTeacherId;

                if (image != null && image.Length > 0)
                {
                    string base64String = Convert.ToBase64String(image);
                    string blobDataURL = $"data:image/jpeg;base64,{base64String}";
                    allTeacherData.Add((blobDataURL));
                }
                var tr = _context.TTeachers.Select(t => t.FTeacherProfilePic);
            }
           


            return Json(allTeacherData);
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
