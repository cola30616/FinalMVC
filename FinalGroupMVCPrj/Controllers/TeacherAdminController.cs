using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Pkcs;

namespace FinalGroupMVCPrj.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherAdminController : UserInfoController
    {
        private readonly LifeShareLearnContext _lifeShareLearnContext;
        public TeacherAdminController(LifeShareLearnContext lifeShareLearnContext)
        {
            _lifeShareLearnContext = lifeShareLearnContext;
        }
        //■ ==========================     翊妏作業區      ==========================■
        // GET: TeacherAdmin/LessonList
        //動作簡述：回傳老師課程清單資訊
        [HttpGet]
        public IActionResult LessonList( )
        {
            return View();
        }
        //■ ==========================     東霖作業區      ==========================■

        // GET: TeacherAdmin/L
        //動作簡述：回傳老師基本資訊
        [HttpGet]
        public IActionResult BasicInfo( )
        {
            //取得所有db老師資料放到ViewModel
            IEnumerable<Class> vBasicVMCollection = new List<Class>(
                    _lifeShareLearnContext.TTeachers
                    .Select(tr =>new Class {
                        TeacherName = tr.FTeacherName,
                        TeacherProfilePic = tr.FTeacherProfilePic,
                        Introduction = tr.FIntroduction,
                        ContactInfo = tr.FContactInfo,
                        Note = tr.FNote,
                    })
                );
            return View("Basicinfo", vBasicVMCollection);
        }
        // GET: TeacherAdmin/
        //動作簡述：回傳老師相關圖片
        [HttpGet]
        public IActionResult RelatedPic()
        {
            return View();
        }
        //■ ==========================     子謙作業區      ==========================■

    }
}
