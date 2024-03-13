using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LessonCreateViewModel
    {
        public int FLessonCourseId { get; set; }

        //[Required(ErrorMessage = "課程講師為必填欄位")]
        [Display(Name = "課程講師")]
        public int FTeacherId { get; set; }

        [Required(ErrorMessage = "課程領域為必選欄位")]
        [Display(Name = "課程領域")]
        public int? FFiledid { get; set; }
        public string? FFiled { get; set; }

        [Required(ErrorMessage = "課程科目為必填欄位")]
        [Display(Name = "科目")]
        public int? FSubjectId { get; set; }
        public string? FSubject { get; set; }

        //[Required(ErrorMessage = "課程代號為必填欄位")]
        [Display(Name = "代號")]
        public string? FCode { get; set; }

        [Required(ErrorMessage = "課程名稱為必填欄位")]
        [Display(Name = "課程名稱")]
        public string? FName { get; set; }

        [Display(Name = "課程介紹")]
        public string? FDescription { get; set; }

        [Display(Name = "課程要求")]
        public string? FRequirement { get; set; }

        [Display(Name = "封面")]
        public byte[]? FPhoto { get; set; }

        //[Required(ErrorMessage = "課程售價為必填欄位")]
        [Display(Name = "售價")]
        public decimal? FPrice { get; set; }

        //[Required(ErrorMessage = "課程作業說明為必填欄位")]
        [Display(Name = "作業說明")]
        public string? FHomeworkDescription { get; set; }

        //[Required(ErrorMessage = "課程人數上限為必填欄位")]
        [Display(Name = "課程人數上限")]
        public int? FMaxPeople { get; set; }

        //[Required(ErrorMessage = "課程人數下限為必填欄位")]
        [Display(Name = "課程人數下限")]
        public int? FMinPeople { get; set; }

        //[Required(ErrorMessage = "課程開始日期為必填欄位")]
        [Display(Name = "開課日期")]
        public DateTime? FLessonDate { get; set; }

        //[Required(ErrorMessage = "課程開始時間為必填欄位")]
        [Display(Name = "開課時間")]
        public TimeSpan? FStartTime { get; set; }

        //[Required(ErrorMessage = "課程結束時間為必填欄位")]
        [Display(Name = "結束時間")]
        public TimeSpan? FEndTime { get; set; }

        //[Required(ErrorMessage = "場地類型為必填欄位")]
        [Display(Name = "場地類型")]
        public bool? FVenueType { get; set; }


        //[Required(ErrorMessage = "場地資訊為必填欄位")]
        [Display(Name = "場地資訊")]
        public string? FOnlineLink { get; set; }

        public string? FVenueName { get; set; }

        public int? FDistrictId { get; set; }

        public string? FAddressDetail { get; set; }

        //[Required(ErrorMessage = "課程報名截止日期為必填欄位")]
        [Display(Name = "課程報名截止日期")]
        public DateTime? FRegDeadline { get; set; }

        [Display(Name = "狀態")]
        public bool? FStatus { get; set; }

        public string? FStatusNote { get; set; }
      
    }
}
