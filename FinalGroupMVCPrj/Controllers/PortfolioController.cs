using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
	public class PortfolioController : UserInfoController
	{
        private readonly LifeShareLearnContext _context;
        public PortfolioController(LifeShareLearnContext context)
        {
            _context = context;
        }
        //回傳作品清單
        [HttpGet]
        public async Task<IActionResult> ListAsync()
		{
            var portfolioList = _context.TCourseworks.FirstOrDefault(m => m.FMemberId == GetCurrentMemberId());
             
            return View(portfolioList);
		}
	}
}
