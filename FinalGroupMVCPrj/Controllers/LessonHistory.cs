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
            int orderId = _context.TOrders.Where(od => od.FMemberId == GetCurrentMemberId()).FirstOrDefault().FOrderId;
            LearningRecordVM learningRecord = new()
            {
                SuccessRecord = successRecord,
                CancelRecord = cancelRecord,
                FOrderId = orderId
            };
            return View(learningRecord);
        }
        //■ ==========================     Apple 作業區      ==========================■
        public IActionResult Detail(int? id)
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
                    FOrderValid = od.FOrderValid,
                    FOrderDate = od.FOrder.FOrderDate,
                    FLessonPrice = od.FLessonPrice,
                }).ToList().FirstOrDefault();
         
            return View("Detail", order);
        }

        [HttpPost]
        public IActionResult CancelOrder(int? id, string reason)
        {
            string modificationDescription = null; //目前無法顯示取消原因，並且btn要變成灰色不能點選
            switch (reason)
            {
                case "option1":
                    modificationDescription = "事假";
                    break;
                case "option2":
                    modificationDescription = "病假";
                    break;
            }

            var orderDetail = _context.TOrderDetails.FirstOrDefault(od => od.FOrderId == id);
            if (orderDetail != null)
            {
                orderDetail.FOrderValid = false;
                orderDetail.FModificationDescription = modificationDescription; //將取消原因文字寫入資料庫
                _context.SaveChanges();
                return Ok(orderDetail);
            }
            return NotFound();
        }
    }
}
