
using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LessonListViewModel
    {
        [Display(Name = "代號")]
        public string FCode { get; set; }
        [Display(Name = "課程名稱")]
        public string FName { get; set; }
        [Display(Name = "領域")]
        public string FFiled { get; set; }
        [Display(Name = "售價")]
        public int FPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "開課日期")]
        public DateTime? FLessonDate { get; set; }
        [Display(Name = "總時數")]
        public string FTime {  get; set; }
        [Display(Name = "人數上限")]
        public int? FMaxPeople { get; set; }
        [Display(Name = "報名人數")]
        public int FRegPeople { get; set;}
        [Display(Name = "狀態")]
        public string FStatus { get; set; }
        [Display(Name = "場地類型")]
        public string FVenueType { get; set; }

    }
}
