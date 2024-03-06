using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    public class MessageController : UserInfoController
    {
        public IActionResult ChatTeacher()
        {
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
