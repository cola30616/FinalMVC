using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class TestECpayController : Controller
    {
        public IActionResult Index2()
        {
            var orderId = "ecpay20240309191038";
            var order = new Dictionary<string, string>
    {
        //綠界需要的參數
        { "MerchantTradeNo",  orderId},
        { "MerchantTradeDate",  "2024/03/09 19:10:10"},
        { "TotalAmount",  "100"},
        { "TradeDesc",  "測試"},
        { "ItemName",  "商品名稱測試"},
        { "ReturnURL",  "https://localhost:7031/Login"},
         { "ClientBackURL",  "https://localhost:7031"},
        { "MerchantID",  "3002607"},
        { "PaymentType",  "aio"},
        { "ChoosePayment",  "Credit"},
        { "EncryptType",  "1"},
    };
            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);
            return View(order);
        }
        public IActionResult ECpayResult(JObject info)
        {
            return Content("");
        }        

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
