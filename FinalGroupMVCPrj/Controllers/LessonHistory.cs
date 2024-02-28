using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    public class LessonHistory : UserInfoController
    {
        // GET: LessonHistory/List
        //動作簡述：回傳課程記錄清單的頁面
        [HttpGet]
        public IActionResult List()
        {
            return View("AList");
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
