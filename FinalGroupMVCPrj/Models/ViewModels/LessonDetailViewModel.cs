using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LessonDetailViewModel
    {
        public int FLessonCourseId { get; set; }

        public string? FName { get; set; }

        public string? FFiled { get; set; }

        public string? FTime { get; set; }
        public int FRegPeople {get;set;} 

        public string? FEditorDes { get; set; }

        public string? FDescription { get; set; }

        public string? FRequirement { get; set; }

        public byte[]? FPhoto { get; set; }

        public decimal? FPrice { get; set; }

        public int? FMaxPeople { get; set; }

        public int? FMinPeople { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? FLessonDate { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan? FStartTime { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan? FEndTime { get; set; }

        public bool? FVenueType { get; set; }

        public string? FOnlineLink { get; set; }
        
        public string? FVenueName { get; set; }

        public string? FCity { get; set; }
        public string? FDistrict { get; set; }
        public string? FAddressDetail { get; set; }
        
        public string? AllAddress { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? FRegDeadline { get; set; }

        public string? FStatus { get; set; }

        public string? FStatusNote { get; set; }

        public virtual TCourseSubject FSubject { get; set; }

        public virtual TTeacher FTeacher { get; set; }
      

        //評價的部分
        public double FAvgScore { get; set; }
    }
}
