using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class LessonReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
