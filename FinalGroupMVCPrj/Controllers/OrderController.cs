using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModel;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    public class OrderController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public OrderController(LifeShareLearnContext context)
        {
            _context = context;
        }

        //■ ==========================     Apple 作業區      ==========================■

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 取得TLessonCourse資料
            var lessonCourse = _context.TLessonCourses.FirstOrDefault(lc => lc.FLessonCourseId == id);
            if (lessonCourse == null)
            {
                return NotFound();
            }

            // 取得TMember資料
            TMember? member = _context.TMembers.FirstOrDefault(m => m.FMemberId == GetCurrentMemberId()); 
            if (member == null)
            {
                return NotFound();
            }

            // 將得到的資料放到OrderDetailViewModel
            var OrderDetailViewModel = new OrderDetailViewModel
            {
                FRealName= member.FRealName,              
                FDescription = lessonCourse.FDescription
            };
            return View("Detail", OrderDetailViewModel);
        }                  
    }
}