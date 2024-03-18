using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public IActionResult List(int memberId = 9)
        {
            PortfolioListDTO portfolioListDTO = new PortfolioListDTO();

            //IEnumerable<PortfolioListDTO> portfolioList =
            //    new List<PortfolioListDTO>(_context.TCourseworks
            //    .Where(m => m.FCourseworkId == id)
            //    .Select(c => new PortfolioListDTO
            //    {
            //        FName = c.FName,
            //        FDescrpition = c.FDescrpition,
            //        //FShowName = _context.TMembers.
            //    }));


            IEnumerable<PortfolioListDTO> portfolioList = new List<PortfolioListDTO>(_context.TCourseworks.Include(c => c.FOrderDetail).ThenInclude(o => o.FOrder).ThenInclude(o => o.FMember).Include(c => c.FOrderDetail).ThenInclude(o => o.FLessonCourse).ThenInclude(c => c.FSubject).ThenInclude(t => t.FField).ThenInclude(a => a.TMemberWishFields)

                .Where(c => c.FMemberId == memberId)
                .Select(c => new PortfolioListDTO
                {
                    FMemberId = c.FMemberId,
                    FShowName = c.FOrderDetail.FOrder.FMember.FShowName,
                    FName = c.FName,
                    FComment = c.FComment,
                    FDescrpition = c.FDescrpition,
                    FLessonName = c.FOrderDetail.FLessonCourse.FName,
                    FLastModifyTime = c.FLastModifyTime,
                    FSubjectName = c.FOrderDetail.FLessonCourse.FSubject.FSubjectName,
                    FFieldName = c.FOrderDetail.FLessonCourse.FSubject.FField.FFieldName,
                    FLessonCourseDescrpition = c.FOrderDetail.FLessonCourse.FDescription,
                }));
            return View(portfolioList);
            //return Ok(portfolioList);
        }
        [HttpGet]
        public IActionResult Package()
        {

            TCourseField? member = (TCourseField?)_context.TCourseFields.Select(m => m.FFieldName);
            //var Model = new CreateOrder { };
            ViewBag.name = member;
            return View("List", member);
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
        //public RedirectToRouteResult List(int id)
        //{
        //    portfolioRead.FMemberId = id;
        //    return  RedirectToAction("portfolioList", new {id=id});
    }
}
