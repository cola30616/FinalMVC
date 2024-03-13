using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Versioning;

namespace FinalGroupMVCPrj.Controllers
{
	public class PortfolioController : UserInfoController
	{
        private readonly LifeShareLearnContext _context;
        public PortfolioController(LifeShareLearnContext context)
        {
            _context = context;
        }
        //PortfolioListDTO portfolioRead = new PortfolioListDTO();
        List<PortfolioListDTO> portfolioRead = new List<PortfolioListDTO>();
        //回傳作品清單，https://localhost:7031/Portfolio/List
        [HttpGet]
        public IActionResult List(int id = 1)
        {
            PortfolioListDTO portfolioListDTO = new PortfolioListDTO();

            IEnumerable<PortfolioListDTO> portfolioList =
                new List<PortfolioListDTO>(_context.TCourseworks
                .Where(m => m.FCourseworkId == id)
                .Select(c => new PortfolioListDTO
                {
                    FName = c.FName,
                    FDescrpition = c.FDescrpition,
                }));

            return View(portfolioList);
        }
        //    [HttpGet]
        //public IActionResult List(int id)
        //{

        //    var result = portfolioRead.
        //        Where(m => m.FMemberId==id);

        //    if(result != null)
        //    {
        //        //var result2 = _context.TCourseworks
        //        //    .Select(c => new List<PortfolioListDTO>
        //        //    {
        //        //         FName = c.FName,
        //        //        FDescrpition = c.FDescrpition,
        //        //    });
        //            IEnumerable<PortfolioListDTO> portfolioList =
        //            new List<PortfolioListDTO>(_context.TCourseworks
        //                .Select(c => new PortfolioListDTO
        //                    {
        //                        FName = c.FName,
        //                        FDescrpition = c.FDescrpition,
        //                    }));
        //        }
        //    return View(result);
        //}
        //[HttpPost]
        //public IActionResult data(int id) 
        //{
        //    while (true) { 
        //        if (portfolioRead.FMemberId == null) 
        //            break; 
        //    }
        //    portfolioRead.FMemberId = id;
        //    return View();
        //}
        //public RedirectToRouteResult List(int id)
        //{
        //    portfolioRead.FMemberId = id;
        //    return  RedirectToAction("portfolioList", new {id=id});
    }
}
