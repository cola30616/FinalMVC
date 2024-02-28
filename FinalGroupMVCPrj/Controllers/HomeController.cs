using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }
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




    }
}
