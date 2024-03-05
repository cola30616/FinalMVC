using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherAdminController : UserInfoController
    {
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



        //■ ==========================     子謙作業區      ==========================■

    }
}
