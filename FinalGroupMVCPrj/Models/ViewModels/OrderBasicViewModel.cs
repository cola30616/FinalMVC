using FinalGroupMVCPrj.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class OrderBasicViewModel
    {
        [Display(Name = "訂單編號")]
        public string FOrderNumber { get; set; }

        //public int FMemberId { get; set; } //顯示會員名
        [Display(Name = "會員姓名")]
        public string? FRealName { get; set; }

        [Display(Name = "會員電話")]
        public string? FPhone { get; set; }

        [Display(Name = "會員信箱")]
        public string? FEmail { get; set; }

        [Display(Name = "訂單成立時間")]
        public DateTime FOrderDate { get; set; }

        //public int FLessonCourseId { get; set; } //顯示課名
        [Display(Name = "課程名稱")]
        public string? FName { get; set; }

        [Display(Name = "課程費用")]
        public decimal? FLessonPrice { get; set; }

        [Display(Name = "訂單是否成立")]
        public bool FOrderValid { get; set; }
        //public string DisplayFOrderValid { get; set; }  => FOrderValid ? "是" : "否";

        [Display(Name = "訂單狀態描述")]
        public string? FModificationDescription { get; set; }
    }
}