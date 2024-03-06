using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Pkcs;

namespace FinalGroupMVCPrj.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherAdminController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public TeacherAdminController(LifeShareLearnContext context)
        {
            _context = context;
        }
        //■ ==========================     翊妏作業區      ==========================■
        // GET: TeacherAdmin/LessonList
        //動作簡述：回傳老師課程清單資訊
        [HttpGet]
        public IActionResult LessonList( )
        {
            return View();
        }

        // GET: TeacherAdmin/LessonCreate
        [HttpGet]
        public IActionResult LessonCreate()
        {
            return View("LCreate");
        }

        // GET: TeacherAdmin/LessonEdit
        [HttpGet]
        public IActionResult LessonEdit()
        {
            return View("LEdit");
        }
        //■ ==========================     東霖作業區      ==========================■
        protected int GetCurrentMemberId()
        {
            var idText = HttpContext.User.Claims.Where(u => u.Type == "MemberId").FirstOrDefault();
            if (idText != null)
            {
                return Convert.ToInt32(idText.Value);
            }
            return 0;
        }
        protected int GetCurrentTeacherId()
        {
            int currentMemberId = GetCurrentMemberId();
            var dbMember = _context.TMembers.SingleOrDefault(m => m.FMemberId == currentMemberId);
            if (dbMember != null && dbMember.FQualifiedTeacher)
            {
                int teacherId = 0;
                var dbTeacher = _context.TTeachers.SingleOrDefault(t => t.FMemberId == currentMemberId);
                if (dbTeacher != null)
                {
                    teacherId = dbTeacher.FTeacherId;
                    return teacherId;
                }
            }
            return 0;
        }
        //方法簡述：將二進位資料轉URL
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
        // GET: TeacherAdmin/TBasicInfo
        //動作簡述：回傳老師基本資訊
        [HttpGet]
        public IActionResult TBasicInfo()
        {
            int id = GetCurrentTeacherId();
            IEnumerable<TeacherBasicViewModel> vBasicVMCollection = new List<TeacherBasicViewModel>(
                    _context.TTeachers
                      .Where(t => t.FTeacherId == id)
                    .Select(t =>new TeacherBasicViewModel {
                        TeacherId = id,
                        TeacherName = t.FTeacherName,
                        TeacherProfilePicURL = (t.FTeacherProfilePic != null) ? GetImageDataURL(t.FTeacherProfilePic) : "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png",
                        Introduction = t.FIntroduction,
                        ContactInfo = t.FContactInfo,
                        Note = t.FNote,
                        SubjectName = t.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName),
                        TeacherModel = t
                    })
                );
            return View("TBasicinfo", vBasicVMCollection);
        }

        public IActionResult Edit(int TeacherID, [Bind("fTeacherId,fTeacherName,fIntroduction,fContactInfo,fNote")] TTeacher teacher)
        {
            if (TeacherID != teacher.FTeacherId) { return NotFound(); }
            if (ModelState.IsValid)
            {
                _context.Update(teacher);
                _context.SaveChanges();
                //重導到首頁
                return RedirectToAction("TBasicInfo");
            }
            return View(teacher);
        }



        // GET: TeacherAdmin/TRelatedPic
        //動作簡述：回傳老師相關圖片
        [HttpGet]
        public IActionResult TRelatedPic()
        {
            return View();
        }
        //■ ==========================     子謙作業區      ==========================■

    }
}
