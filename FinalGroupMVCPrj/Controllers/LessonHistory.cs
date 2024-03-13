using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    public class LessonHistory : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public LessonHistory(LifeShareLearnContext context)
        {
            _context = context;
        }
        //■ ==========================     子謙作業區      ==========================■
        // GET: LessonHistory/List
        //動作簡述：回傳課程記錄清單的頁面
        [HttpGet]
        public IActionResult LearningRecord()
        {
            return View();
        }



        //■ ==========================     Apple 作業區      ==========================■
        public IActionResult Details()
        {
            ViewBag.FOrderDetailId = 5;
            return View();
        }
    }
}
