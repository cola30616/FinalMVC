using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
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
        PortfolioListDTO portfolioRead = new PortfolioListDTO();
        //List<PortfolioListDTO> portfolioRead = new List<PortfolioListDTO>();
        //回傳作品清單，https://localhost:7031/Portfolio/List
        [HttpGet]
        public IActionResult AllWorks()
        {
            PortfolioListDTO portfolioListDTO = new PortfolioListDTO();
            IEnumerable<PortfolioListDTO> portfolioList = new List<PortfolioListDTO>(_context.TCourseworks.Include(c => c.FOrderDetail).ThenInclude(o => o.FOrder).ThenInclude(o => o.FMember).Include(c => c.FOrderDetail).ThenInclude(o => o.FLessonCourse).ThenInclude(c => c.FSubject).ThenInclude(t => t.FField).ThenInclude(a => a.TMemberWishFields)
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
            return View(portfolioListDTO);
        }
        [HttpGet]
        public IActionResult List(int? id)
        {
            int memberId = 9;
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
        [HttpPost]
        public IActionResult data() 
        {
            IEnumerable<PortfolioListDTO> portfolioList = new List<PortfolioListDTO>(_context.TCourseworks.Include(c => c.FOrderDetail).ThenInclude(o => o.FOrder).ThenInclude(o => o.FMember).Include(c => c.FOrderDetail).ThenInclude(o => o.FLessonCourse).ThenInclude(c => c.FSubject).ThenInclude(t => t.FField).ThenInclude(a => a.TMemberWishFields)
                
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
                })).ToList();
            return Json(portfolioList); 
        }
        [HttpPost]
        public IActionResult Create(PortfolioListDTO portfolioList)
        {
            if (ModelState.IsValid)
            {
                // 将数据转换成实体对象
                var newPortfolio = new TCoursework                  
                    {
                    FCommentPerson = portfolioList.FCommentPerson,
                    FComment = portfolioList.FComment,
                    };
            

                // 添加到数据库并保存更改
                _context.TCourseworks.Add(newPortfolio);
                _context.SaveChanges();

                // 返回新建的数据
                return Json(newPortfolio);
            }

            // 如果模型验证失败，则返回错误信息
            return BadRequest(ModelState);
        }
        //public IActionResult Create(string itemName, string itemDescription)
        //{
        //    // 在这里执行新增逻辑，比如将数据保存到数据库

        //    // 假设这里只是返回一个成功的消息
        //    return Json(new { message = "Item added successfully." });
        //}
        //[HttpGet]
        //public IActionResult Package()
        //{

        //    TCourseField? member = (TCourseField?)_context.TCourseFields.Select(m => m.FFieldName);
        //    //var Model = new CreateOrder { };
        //    ViewBag.name = member;
        //    return View("List", member);
        //}

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
