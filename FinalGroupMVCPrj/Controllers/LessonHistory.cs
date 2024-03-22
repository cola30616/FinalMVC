using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    public class LessonHistory : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public LessonHistory(LifeShareLearnContext context)
        {
            _context = context;
        }
        //■ ==========================     子謙作業區      ==========================■
        // GET: LessonHistory/List
        //動作簡述：回傳課程記錄清單的頁面
        [HttpGet]
        public IActionResult LearningRecord()
        {            
            var successRecord = _context.TOrderDetails.Where(lr => lr.FOrder.FMemberId == GetCurrentMemberId() && lr.FOrderValid == true)
                .Select(lr => lr.FLessonCourse).Distinct().ToList();
            var cancelRecord = _context.TOrderDetails.Where(lr => lr.FOrder.FMemberId == GetCurrentMemberId() && lr.FOrderValid == false)
                .Select(lr => lr.FLessonCourse).Distinct().ToList();
            LearningRecordVM learningRecord = new()
            {
                SuccessRecord = successRecord,
                CancelRecord = cancelRecord
            };
            return View(learningRecord);
        }



        //■ ==========================     Apple 作業區      ==========================■
        public IActionResult Details(int? id)
        {
            ViewBag.FOrderDetailId = 5;

            if (id == null)
            {
                return NotFound();
            }

            // 取得TOrder、TOrderDetail資料
            var order = _context.TOrderDetails
                .Include(od => od.FOrder)
                .Include(od => od.FLessonCourse)
                .Where(od => od.FOrderId == id)
                .Select(od => new LessonHistoryDetailViewModel
                {
                    FName = od.FLessonCourse.FName,
                    FDescription = od.FLessonCourse.FDescription,
                    FOrderId = od.FOrderId,
                    FOrderNumber = od.FOrder.FOrderNumber,
                    FOrderValid = od.FOrderValid,
                    FOrderDate = od.FOrder.FOrderDate,
                    FLessonDate = od.FLessonCourse.FLessonDate,
                    FLessonPrice = od.FLessonPrice,
                    FOrderDetailId = od.FOrderDetailId,
                    FModificationDescription = od.FModificationDescription,
                }).ToList().FirstOrDefault();

            return View("Details", order);
        }
    }
}
