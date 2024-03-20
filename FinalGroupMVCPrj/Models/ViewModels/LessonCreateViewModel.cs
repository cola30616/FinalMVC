using Microsoft.AspNetCore.Mvc.Rendering;
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

        public static IEnumerable<SelectListItem> GetCitySelectList()
        {
            IEnumerable<SelectListItem> CitySelectList = new List<SelectListItem>
                {
                new SelectListItem { Text =	"台北市", Value ="1" },
                new SelectListItem { Text =	"新北市", Value = "2"	 },
                new SelectListItem { Text = "基隆市" , Value = "3"    },
                new SelectListItem { Text =	"桃園市", Value = "4"	 },
                new SelectListItem { Text =	"新竹市", Value = "5" },
                new SelectListItem { Text =	"新竹縣", Value = "6"	 },
                new SelectListItem { Text =	"苗栗縣", Value = "7"	 },
                new SelectListItem { Text =	"台中市", Value = "8"	 },
                new SelectListItem { Text =	"彰化縣", Value = "9"	 },
                new SelectListItem { Text =	"南投縣", Value = "10"	 },
                new SelectListItem { Text =	"嘉義市", Value = "11"	 },
                new SelectListItem { Text =	"嘉義縣", Value = "12"	 },
                new SelectListItem { Text =	"雲林縣", Value = "13"       },
                new SelectListItem { Text =	"台南市", Value = "14"  },
                new SelectListItem { Text =	"高雄市", Value = "15"       },
                new SelectListItem { Text =	"屏東縣", Value = "16"       },
                new SelectListItem { Text =	"宜蘭縣", Value = "17"       },
                new SelectListItem { Text =	"花蓮縣", Value = "18"       },
                new SelectListItem { Text =	"台東縣", Value = "19"       },
                new SelectListItem { Text =	"澎湖縣", Value = "20"     },
                new SelectListItem { Text =	"金門縣", Value = "21"     },
                new SelectListItem { Text =	"連江縣", Value = "22"     }
                 };
            return CitySelectList;
        }

        //[Required(ErrorMessage = "課程代號為必填欄位")]
        [Display(Name = "代號")]
        public string? FCode { get; set; }

        [Required(ErrorMessage = "課程名稱為必填欄位")]
        [Display(Name = "課程名稱")]
        public string? FName { get; set; }

        [Display(Name = "課程介紹")]
        public string? FDescription { get; set; }
       
        public string? FEditorDes { get; set; }

        [Display(Name = "課程要求")]
        public string? FRequirement { get; set; }

        [Display(Name = "封面")]
        public IFormFile? FPhoto { get; set; }
        //public byte[]? FPhoto { get; set; }

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
        public string? FStatus { get; set; }

        public string? FStatusNote { get; set; }
      
    }
}
