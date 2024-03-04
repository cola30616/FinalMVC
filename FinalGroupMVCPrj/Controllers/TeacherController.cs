using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Pkcs;

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
            IEnumerable<TeacherBasicViewModel> tBasicVMCollection = new List<TeacherBasicViewModel>(
                _context.TTeachers
                .Include(t => t.TTeacherSubjects)
                .ThenInclude(t =>t.FSubject)
                .Where(t => t.FTeacherId == id)
                .Select(t => new TeacherBasicViewModel {
                    TeacherName = t.FTeacherName,
                    TeacherProfilePicURL = (t.FTeacherProfilePic != null) ? GetImageDataURL(t.FTeacherProfilePic) : "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png",
                    Introduction = t.FIntroduction,
                    ContactInfo = t.FContactInfo,
                    Note = t.FNote,
                    SubjectName = t.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName),
                })
                );
            //var tr = _context.TTeachers.FindAsync(id);
            return View("Info", tBasicVMCollection);
        }


        // ============== apicontroller ============== // 

        // GET: Teacher/AllTrInfo
        //動作簡述：在List.cshtml的老師卡片使用中
        [HttpGet]
        public IActionResult AllTrInfo()
        {
            var tr = _context.TTeachers
                .Include(t => t.TTeacherSubjects)
                .ThenInclude(t => t.FSubject)
            .Select(t => new {
                t.FTeacherId,
                t.FTeacherName,
                TeacherProfilePicURL = (t.FTeacherProfilePic != null) ? GetImageDataURL(t.FTeacherProfilePic) : "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png",
                SubjectNames = t.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName)
                //SubjectName = t.TTeacherSubjects.Select(ts => ts.FSubject).Select(t => new { t.FSubjectName })
            });
            return Json(tr);
        }

        private static string GetImageDataURL(byte[] image)
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
        //動作簡述：回傳所有老師頭像的url(沒有使用到)
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
