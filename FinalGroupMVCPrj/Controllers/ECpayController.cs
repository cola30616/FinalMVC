using FinalGroupMVCPrj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class ECpayController : Controller
    {
        private readonly LifeShareLearnContext _context;
        public ECpayController(LifeShareLearnContext context)
        {
            _context = context;
        }

        public IActionResult ECpayCheckout(int orderDetailId)
        {
            var orderDetail = _context.TOrderDetails.Include(o => o.FLessonCourse).Include(o=>o.FOrder).FirstOrDefault(o=>o.FOrderDetailId == orderDetailId);
            if (orderDetail == null) 
            {
                return NotFound();
            }
            //用來儲存綠界金流所需的不同參數。
            var ECpayOrder = new Dictionary<string, string>
    {
        //綠界需要的參數
        { "MerchantTradeNo",  orderDetail.FOrder.FOrderNumber}, 
        { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
        { "TotalAmount",  ((int)orderDetail.FLessonPrice).ToString()}, 
        { "TradeDesc",  "購買平台課程"},
        { "ItemName",  orderDetail.FLessonCourse.FName},  //
        { "ReturnURL",  $"{Url.Action("ECpayResult","ECpay")}"}, 
        { "OrderResultURL", "https://localhost:7031/ECpay/ECpayResult" }, //client端，回到LessonHistory/Detail/id
        { "MerchantID",  "3002607"},
        { "PaymentType",  "aio"}, 
        { "ChoosePayment",  "Credit"},
        { "EncryptType",  "1"},
    };
            //檢查碼，用於確保訂單資訊的完整性
            ECpayOrder["CheckMacValue"] = GetCheckMacValue(ECpayOrder);
            return View(ECpayOrder);
        }

        //訂單交易成功與否及該筆訂單編號，RtnCode=1為成功
        public IActionResult ECpayResult(int RtnCode, string MerchantTradeNo)
        {
            if (RtnCode == 1)
            {
                // 從 TOrder 中找到對應的那筆
                var order = _context.TOrders.FirstOrDefault(od => od.FOrderNumber == MerchantTradeNo);

                if (order != null)
                {
                    // 根據 TOrderDetail 記錄中的 TOrderId 找到 TOrder中的 fOrderNumber
                    var orderDetail = _context.TOrderDetails.FirstOrDefault(o => o.FOrderId == order.FOrderId);

                    if (orderDetail != null)
                    {
                        // 更新 TOrderDetail 記錄
                        orderDetail.FOrderValid = true;
                        orderDetail.FModificationDescription = null; // 或者您可以將其設置為空字符串，取決於您的需求

                        // 保存更改
                        _context.SaveChanges();

                        return View();
                    }
                    else
                    {
                        // 找不到對應的 TOrderDetail，跳錯誤
                        return BadRequest("找不到相應的訂單記錄");
                    }
                }
                else
                {
                    // 找不到對應的TOrder，跳錯誤
                    return BadRequest("找不到相應的訂單詳細記錄");
                }
            }
            else
            {
                // RtnCode 不為 1，跳錯誤
                return BadRequest("系統出現錯誤");
            }
        }

        //JObject 是 Newtonsoft.Json 套件中的類型，它表示一個動態的、可變的 JSON 物件。
        public IActionResult ECpayResult2(JObject info)
        {
            return Content("");
        }

        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            //測試用的 HashKey
            var hashKey = "pwFHCqoQZGmho4w6";
            //測試用的 HashIV
            var HashIV = "EkRm7iFT261dpevs";
            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }

        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}

