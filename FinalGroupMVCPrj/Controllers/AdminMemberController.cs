using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class AdminMemberController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
