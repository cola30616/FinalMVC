using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class MemberBasicViewModel
    {
        [Display(Name = "編號")]
        public int? MemberId { get; set; }
        [Display(Name = "註冊時間")]
        public string RegisterDateTime { get; set; }
        [Display(Name = "姓名")]
        public string? RealName { get; set; }
        [Display(Name = "顯示名稱")]
        public string? ShowName { get; set; }
        [Display(Name = "電子信箱")]
        public string? Email { get; set; }
        [Display(Name = "驗證")]
        public string? EmailVerification { get; set; }
        [Display(Name = "優惠通知")]
        public string GetCampInfo { get; set; }
        [Display(Name = "狀態")]
        public string Status { get; set; }

    }
}
