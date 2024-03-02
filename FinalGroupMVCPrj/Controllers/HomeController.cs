using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using FinalGroupMVCPrj.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using FinalGroupMVCPrj.Models.DTO;




namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class HomeController : UserInfoController
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





        //■ ==========================     歐文作業區      ==========================■
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login(TMember member, string? actionName, string? controllerName, object? routeValues)
        {
            var dbMember = _context.TMembers
                .Where(m => m.FEmail == member.FEmail && m.FPassword == member.FPassword)
                .SingleOrDefault();
            if (dbMember == null)
            {
                return Content("帳號密錯誤");
            }
            else if (!dbMember.FEmailVerification)
            {
                return Content("信箱未驗證");
            }
            else
            {
                var claims = new List<Claim>{
                  new Claim("MemberId", dbMember.FMemberId.ToString()),
                new Claim(ClaimTypes.Name, dbMember.FShowName),
                new Claim(ClaimTypes.Email, dbMember.FEmail)
                };
                if (dbMember.FQualifiedTeacher)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Teacher"));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
        public IActionResult testAPI()
        {
            MemberInfoDTO member = new MemberInfoDTO { Email = "ssdasdad@sdasdas", MemberId = 5, ShowName = "2222", RealName = "5555" };
            return Json(member);
        }
        [HttpPost]
        public IActionResult teacherApply([FromBody] TTeacherApplyLog data)
        {
            data.FApplyDatetime = DateTime.Now;
            return Ok(new { Message = "操作成功", Data = data });
        }





    }
}
