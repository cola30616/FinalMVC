using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinalGroupMVCPrj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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





    }
}
