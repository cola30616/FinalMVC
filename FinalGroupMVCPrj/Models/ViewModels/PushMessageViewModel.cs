using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class PushMessageViewModel
    {
        [Display(Name = "編號")]
        public int FPushMessageId { get; set; }

        [Display(Name = "推播類型")]
        public string FPushType { get; set; }

        [Display(Name = "推播模組")]
        public int FPushLayoutId { get; set; }

        [Display(Name = "推播時間")]
        public DateTime FPushStartDate { get; set; }

        [Display(Name = "結束時間")]
        public DateTime FPushEndDate { get; set; }

        [Display(Name = "推播內容")]
        public string FPushContent { get; set; }

        [Display(Name = "圖片")]
        public byte[] FPushImagePath { get; set; }

        [Display(Name = "建立時間")]
        public DateTime FPushCreatedTime { get; set; }

        [Display(Name = "修改時間")]
        public DateTime? FPushLastUpdatedTime { get; set; }
    }
}
