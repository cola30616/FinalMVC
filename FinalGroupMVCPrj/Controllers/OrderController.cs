using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    public class OrderController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public OrderController(LifeShareLearnContext context)
        {
            _context = context;
        }

        //■ ==========================     Apple 作業區      ==========================■

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 取得TLessonCourse資料
            var lessonCourse = _context.TLessonCourses.FirstOrDefault(lc => lc.FLessonCourseId == id);
            if (lessonCourse == null)
            {
                return NotFound();
            }

            // 取得TMember資料
            TMember? member = _context.TMembers.FirstOrDefault(m => m.FMemberId == GetCurrentMemberId());
            if (member == null)
            {
                return NotFound();
            }

            // 將得到的資料放到checkoutDetailViewModel
            var checkoutDetailViewModel = new CheckoutDetailViewModel
            {
                FName = lessonCourse.FName,
                FDescription = lessonCourse.FDescription,                
                FRealName = member.FRealName,
                FPhone= member.FPhone,
                FEmail = member.FEmail,
                FLessonCourseId = lessonCourse.FLessonCourseId,
                FMemberId = member.FMemberId,
                FPrice = (decimal)lessonCourse.FPrice,
                FPhoto = lessonCourse.FPhoto
            };
            return View("Detail", checkoutDetailViewModel);
        }

        [HttpPost]
        public IActionResult CheckOrder(CheckoutDetailViewModel checkoutDetailViewModel)
        {
            if(checkoutDetailViewModel.FMemberId != GetCurrentMemberId())
            {
                TempData["Error"] = "當前登入異常";
                return View("Detail", checkoutDetailViewModel);
            }
            // 第一步：新增到 TOrder
            int memberId = GetCurrentMemberId(); // 取得當前會員的 ID
            TOrder tempOrder = new TOrder
            {
                FOrderDate = DateTime.Now,
                FPaymentMethod = "信用卡",
                FMemberId = memberId,
            };
            _context.TOrders.Add(tempOrder);
            _context.SaveChanges();
            var order = _context.TOrders.FirstOrDefault(o=> o.FMemberId == tempOrder.FMemberId && o.FOrderDate == tempOrder.FOrderDate);
            if(order == null)
            {
                TempData["Error"] = "系統異常，請稍後再試";
                return View("Detail", checkoutDetailViewModel);
            }
            order.FOrderNumber = GetOrderNumber();

            //// 第二步：新增到 TOrderDetail
            TOrderDetail tempOrderDetail = new TOrderDetail
            {
                FOrderId = order.FOrderId,
                FLessonCourseId = checkoutDetailViewModel.FLessonCourseId,
                FLessonPrice = checkoutDetailViewModel.FPrice,
                FOrderValid = false,
                FModificationDescription = "付款未完成",
            };
            _context.TOrderDetails.Add(tempOrderDetail);
            _context.SaveChanges();
            var orderDetail = _context.TOrderDetails.FirstOrDefault(o => o.FOrderId == tempOrderDetail.FOrderId && o.FLessonCourseId == tempOrderDetail.FLessonCourseId);
            if (orderDetail == null)
            {
                TempData["Error"] = "系統異常，請稍後再試";
                return View("Detail", checkoutDetailViewModel);
            }
            return RedirectToAction("ECpayCheckout", "ECpay", new { orderDetailId = orderDetail.FOrderDetailId}); //導入綠界前檢查的畫面
        }

        // 獲取OrderNumber
        private string GetOrderNumber()
        {
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.Date.AddDays(1);
            int no = _context.TOrders.Where(o=>o.FOrderDate < end && o.FOrderDate > start).Count() + 1;
            string orderNumber = "LSLtset" +start.ToString("yyMMdd") + $"{no:D4}";
            return orderNumber;
        }  
    }
}
