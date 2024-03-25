using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework.Profiler;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class LessonReviewController : Controller
    {
        private readonly LifeShareLearnContext _context;
        public LessonReviewController(LifeShareLearnContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAvgEvalScore(int CourseId)
        {
            var FCode = _context.TLessonCourses.FirstOrDefault(l =>l.FLessonCourseId == CourseId).FCode ;
            var detail = await _context.TLessonEvaluations
            .Include(order => order.FOrderDetail)
                .ThenInclude(evaluation => evaluation.FLessonCourse)
            .Where(eval => eval.FOrderDetail.FLessonCourse.FCode == FCode)
            .Select(querystring => new LessonAvgEvalViewModel
            {
                FAvgScore = querystring.FScore
            }).ToListAsync();

            double averageScore = Math.Round(detail.Select(x => x.FAvgScore).DefaultIfEmpty(0).Average(),1);

            var avgScore = new LessonAvgEvalViewModel();
            avgScore.FAvgScore = averageScore;

            return PartialView("_BValuateAvgPartial", avgScore);
        }

        [HttpGet]
        public async Task<IActionResult> GetEvalList(int? CourseId)
        {
            var FCode = _context.TLessonCourses.FirstOrDefault(l => l.FLessonCourseId == CourseId).FCode;
            var query = await _context.TLessonEvaluations
                        .Include(order => order.FOrderDetail)
                             .ThenInclude(course => course.FLessonCourse)
                         .Include(order => order.FOrderDetail)
                            .ThenInclude(od =>od.FOrder)
                            .ThenInclude(o => o.FMember)
                        .Where(eval => eval.FOrderDetail.FLessonCourse.FCode == FCode)
                        .Select(evaluation => new LessonEvaluationsViewModel
                        {
                            FLessonEvalId = evaluation.FLessonEvalId,
                            FMemberId = evaluation.FOrderDetail.FOrder.FMemberId,
                            FShowName = evaluation.FOrderDetail.FOrder.FMember.FShowName,
                            FOrderDetailId = evaluation.FOrderDetailId,
                            FLessonCourseId = evaluation.FOrderDetail.FLessonCourseId,
                            FName = evaluation.FOrderDetail.FLessonCourse.FName,
                            FScore = evaluation.FScore,
                            FComment = evaluation.FComment,
                            FCommentDate = evaluation.FCommentDate,
                            FCommentUpdateDate = evaluation.FCommentUpdateDate,
                            FDisplayStatus = evaluation.FDisplayStatus
                        }).OrderByDescending(eval=>eval.FCommentDate).ToListAsync();
            int star1 = 0, star2 = 0, star3 = 0, star4 = 0, star5 = 0;

            foreach (var item in query)
            {
                if (item.FScore == 1) star1++;
                else if (item.FScore == 2) star2++;
                else if (item.FScore == 3) star3++;
                else if (item.FScore == 4) star4++;
                else if (item.FScore == 5) star5++;
            }

            int totalCount = star1 + star2 + star3 + star4 + star5;
            int percentStar1 = 0, percentStar2 = 0, percentStar3 = 0, percentStar4 = 0, percentStar5 = 0;

            if (totalCount > 0)
            {
                percentStar1 = (int)Math.Round((double)star1 / totalCount * 100);
                percentStar2 = (int)Math.Round((double)star2 / totalCount * 100);
                percentStar3 = (int)Math.Round((double)star3 / totalCount * 100);
                percentStar4 = (int)Math.Round((double)star4 / totalCount * 100);
                percentStar5 = (int)Math.Round((double)star5 / totalCount * 100);
            }
            ViewBag.totalCount = totalCount;
            ViewBag.percentStar1 = percentStar1;
            ViewBag.percentStar2 = percentStar2;
            ViewBag.percentStar3 = percentStar3;
            ViewBag.percentStar4 = percentStar4;
            ViewBag.percentStar5 = percentStar5;

            return PartialView("_BEvalListPartial", query);
        }

        [HttpGet]
        public async Task<IActionResult> GetEvalDetail(int FOrderDetailId)
        {
            var details = await _context.TLessonEvaluations.Where(eval => eval.FOrderDetailId == FOrderDetailId)
            .Select(eval => new SingleEvaluationViewModel
            {
                FOrderDetailId = FOrderDetailId,
                FScore = eval.FScore,
                FComment = eval.FComment,
                FCommentDate = eval.FCommentDate,
                FCommentUpdateDate = eval.FCommentUpdateDate,
            }).ToListAsync();
            if (details.IsNullOrEmpty())
            {
                return Content("");
            }
            var detail = details.FirstOrDefault();
            return PartialView("_BEvalDetailPartial", detail);
        }

        [HttpGet]
        //獲取照片
        public async Task<FileResult> GetPicture(int FMemberId)
        {
            Models.TMember? member = await _context.TMembers.FindAsync(FMemberId);
            byte[]? Content = member?.FMemberProfilePic;
            return File(Content, "image/jpeg");
        }

        [HttpGet]
        public async Task<IActionResult> isEvaluated(int FOrderDetailId)
        {
            bool isExisting = await _context.TLessonEvaluations.AnyAsync(eval => eval.FOrderDetailId == FOrderDetailId);

            return Json(new { isExisting = isExisting });
        }

        [HttpGet]
        public async Task<IActionResult> canEvaluated(int FOrderDetailId)
        {

            var od = await _context.TOrderDetails
                .Include(od =>od.FLessonCourse)
                .FirstOrDefaultAsync(od => od.FOrderDetailId == FOrderDetailId);
            var isValid = od.FOrderValid;
            if(od.FLessonCourse.FLessonDate > DateTime.Now)
            {
                isValid = false;
            }
            return Json(new { isValid = isValid });
        }


        [HttpGet]
        public async Task<IActionResult> CreateEvaluation(int FOrderDetailId)
        {
            return PartialView("_BCreateEvaluationPartial");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvaluation([FromBody] SingleEvaluationViewModel evaldata)
        {
            if (!(_context.TLessonEvaluations.Any(eval => eval.FOrderDetailId == evaldata.FOrderDetailId)))
            {
                TLessonEvaluation evaluation = new TLessonEvaluation();
                var FmemberId = await _context.TOrderDetails
                    .Include(eval => eval.FOrder)
                    .Where(eval => eval.FOrderDetailId == evaldata.FOrderDetailId)
                    .Select(eval => eval.FOrder.FMemberId).FirstOrDefaultAsync();
                evaluation.FMemberId = FmemberId;
                evaluation.FOrderDetailId = evaldata.FOrderDetailId;
                evaluation.FScore = evaldata.FScore;
                evaluation.FComment = evaldata.FComment;
                evaluation.FCommentDate = DateTime.Now;
                evaluation.FDisplayStatus = true;
                if (ModelState.IsValid)
                {
                    _context.Add(evaluation);
                    await _context.SaveChangesAsync();
                    return Ok("Success");
                }
            }
            return BadRequest("Evaluation creation failed");
        }

        [HttpGet]
        public async Task<IActionResult> EditEvaluation(int FOrderDetailId)
        {
            if (FOrderDetailId == null || _context.TLessonEvaluations == null)
            {
                return NotFound();
            }
            var evaluation = await _context.TLessonEvaluations.FirstOrDefaultAsync(eval => eval.FOrderDetailId == FOrderDetailId);
            if (evaluation == null)
            {
                evaluation = new TLessonEvaluation
                {
                    FScore = 0,
                    FComment = ""
                };
            }
            return PartialView("_BEditEvaluationPartial", evaluation);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvaluation([FromBody] SingleEvaluationViewModel evaldata)
        {
            if (_context.TLessonEvaluations.Any(eval => eval.FOrderDetailId == evaldata.FOrderDetailId))
            {
                var evaluation = await _context.TLessonEvaluations
                    .Include(eval => eval.FOrderDetail)
                        .ThenInclude(eval => eval.FOrder)
                    .Where(eval => eval.FOrderDetailId == evaldata.FOrderDetailId).FirstOrDefaultAsync();
                evaluation.FScore = evaldata.FScore;
                evaluation.FComment = evaldata.FComment;
                evaluation.FCommentUpdateDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _context.Update(evaluation);
                    await _context.SaveChangesAsync();
                    return Ok("Success");
                }
            }
            return BadRequest("Evaluation creation failed");
        }


        [HttpGet]
        public async Task<IActionResult> GetEvalScore(int FOrderDetailId)
        {
            if (FOrderDetailId == null || _context.TLessonEvaluations == null)
            {
                return NotFound();
            }

            var evaluation = await _context.TLessonEvaluations.FirstOrDefaultAsync(eval => eval.FOrderDetailId == FOrderDetailId);
            if (evaluation == null)
            {
                evaluation = new TLessonEvaluation
                {
                    FScore = 0
                };
            }
            return Ok(evaluation.FScore);
        }

    }
}
