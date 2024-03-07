using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    public class OrderController : UserInfoController
    {

        //■ ==========================     Apple 作業區      ==========================■
        public IActionResult Details(int? id)
        {
            return View();
        }

    }
}
