using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
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
        PortfolioListDTO portfolioRead = new PortfolioListDTO();

        //回傳作品清單，https://localhost:7031/Portfolio/AllWorks
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
                    FCourseworkId = c.FCourseworkId,
                    FFieldId = c.FOrderDetail.FLessonCourse.FSubject.FField.FFieldId,
                    FFileLink = c.TCourseworkFiles.FirstOrDefault(f => f.FCourseworkId == c.FCourseworkId).FFileLink
                })).Distinct();
            return View(portfolioList);
        }


        [HttpGet]
        public IActionResult List(int Id)
        {
            PortfolioListDTO portfolioListDTO = new PortfolioListDTO();
            IEnumerable<PortfolioListDTO> portfolioList = new List<PortfolioListDTO>(_context.TCourseworks.Include(c => c.FOrderDetail).ThenInclude(o => o.FOrder).ThenInclude(o => o.FMember).Include(c => c.FOrderDetail).ThenInclude(o => o.FLessonCourse).ThenInclude(c => c.FSubject).ThenInclude(t => t.FField).ThenInclude(a => a.TMemberWishFields).Include(c => c.TCourseworkFiles)

                .Where(c => c.FCourseworkId == Id)
                .Select(c => new PortfolioListDTO
                {
                    FCourseworkId = c.FCourseworkId,
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
                    FFileLink = c.TCourseworkFiles.FirstOrDefault(f => f.FCourseworkId == c.FCourseworkId).FFileLink
                }));

            return View(portfolioList);
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
        public IActionResult Search(string category = null)
        {
            IQueryable<TCoursework> query = _context.TCourseworks.Include(c => c.FOrderDetail).ThenInclude(o => o.FOrder).ThenInclude(o => o.FMember).Include(c => c.FOrderDetail).ThenInclude(o => o.FLessonCourse).ThenInclude(c => c.FSubject).ThenInclude(t => t.FField).ThenInclude(a => a.TMemberWishFields);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(c => c.FOrderDetail.FLessonCourse.FSubject.FField.FFieldName == category);
            }

            var portfolioList = query
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
                })
                .ToList();

            return View("AllWorks", portfolioList);
        }
        public IActionResult Page(int page = 1, int pageSize = 10)
        {
            var totalCount = _context.TCourseworks.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var portfolioList = _context.TCourseworks
                .OrderBy(c => c.FCourseworkId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View("AllWorks", portfolioList);
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
    }
}
