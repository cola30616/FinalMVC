using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    public class MessageController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public MessageController(LifeShareLearnContext context)
        {
            _context = context;
        }
        public IActionResult ChatTeacher()
        {
            int teacherId = GetCurrentTeacherId();
            var query = _context.TChatMessageTeachers
                .Include(msg => msg.FMember)
                .Include(msg => msg.FTeacher)
                .Where(msg => msg.FTeacherId == teacherId)
                .Select(msg => new ChatTeacherViewModel
                {
                    FTeacherId = msg.FTeacherId,
                    FTeacherName =msg.FTeacher.FTeacherName,
                    FTeacherProfilePic = msg.FTeacher.FTeacherProfilePic
                });
            ViewBag.teacherId = teacherId;
            return View();
        }
        public IActionResult rate()
        {
            return View();
        }
        public IActionResult Valuate()
        {
            return PartialView("_BValuatePartial");
        }
    }
}
