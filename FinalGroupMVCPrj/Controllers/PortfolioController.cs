using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
	public class PortfolioController : UserInfoController
	{
        private readonly LifeShareLearnContext _context;
        public PortfolioController(LifeShareLearnContext context)
        {
            _context = context;
        }
        //回傳作品清單，https://localhost:7031/Portfolio/List
        [HttpGet]
        public IActionResult List()
		{
            //PortfolioListDTO portfolioListDTO = new PortfolioListDTO();
            //var portfolioList = await _context.TCourseworks
            //    .select(m => m.FMemberId == GetCurrentMemberId());         
            IEnumerable<PortfolioListDTO> portfolioList =
                new List<PortfolioListDTO>(_context.TCourseworks
                .Select(c => new PortfolioListDTO
                {
                    FName = c.FName,
                    FDescrpition = c.FDescrpition,
                }));

            return View(portfolioList);
		}
	}
}
