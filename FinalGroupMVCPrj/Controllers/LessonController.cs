using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class LessonController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public LessonController(LifeShareLearnContext context)
        {
            _context = context;
        }
        //■ ==========================     子謙作業區      ==========================■
        // GET: LessonHistory/List
        //動作簡述：回傳課程記錄清單的頁面
        public IActionResult Index()
        {
            return View();
        }



        //■ ==========================     翊妏 作業區      ==========================■
        [HttpGet]
        public IActionResult Details()
        {
            return View("LDetails");
        }
    }
}
