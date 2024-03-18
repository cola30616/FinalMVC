
using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LessonListViewModel
    {
        [Display(Name = "代號")]
        public string Code { get; set; }
        [Display(Name = "課程名稱")]
        public string Name { get; set; }
        [Display(Name = "領域")]
        public string Filed { get; set; }
        [Display(Name = "售價")]
        public int Price { get; set; }
      
        [Display(Name = "開課日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? LessonDate { get; set; }
        [Display(Name = "總時數")]
        public string Time {  get; set; }
        [Display(Name = "人數上限")]
        public int? MaxPeople { get; set; }
        [Display(Name = "報名人數")]
        public int RegPeople { get; set;}
        [Display(Name = "狀態")]
        public string Status { get; set; }
        [Display(Name = "場地類型")]
        public string VenueType { get; set; }

        public int lessonid { get; set; }

    }
}
