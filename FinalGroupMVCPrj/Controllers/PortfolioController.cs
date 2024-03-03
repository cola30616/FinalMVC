using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
	public class PortfolioController : UserInfoController
	{
		public IActionResult List()
		{
			
			return View();
		}
	}
}
