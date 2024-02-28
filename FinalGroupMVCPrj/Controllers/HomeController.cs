using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;




namespace FinalGroupMVCPrj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LifeShareLearnContext _context;

        public HomeController(ILogger<HomeController> logger, LifeShareLearnContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courseList = await _context.TLessonCourses
                .Select(course => new LessonCourseVM
                {
                    lessonCourse = course,
                    // 老師名稱
                    teacherName = _context.TTeachers
                        .Where(teacher => teacher.FTeacherId == course.FTeacherId)
                        .Select(teacher => teacher.FTeacherName)
                        .FirstOrDefault() ?? "找不到當前老師",
                    // 科目名稱
                    subjectName = _context.TCourseSubjects
                        .Where(sub => sub.FSubjectId == course.FSubjectId)
                        .Select (sub => sub.FSubjectName)
                        .FirstOrDefault() ?? "找不到科目名稱"
                })
                .ToListAsync();

            return View(courseList);
        }
        public IActionResult Test()
        {
            return View();
        }





    }
}
