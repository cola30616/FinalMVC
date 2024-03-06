namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LessonDetailViewModel
    {
        public int FLessonCourseId { get; set; }

        public string? FName { get; set; }

        public int? FSubjectId { get; set; }

        public string? FCode { get; set; }

        public int FTeacherId { get; set; }

        public string? FEditorDes { get; set; }

        public string? FDescription { get; set; }

        public string? FRequirement { get; set; }

        public byte[] FPhoto { get; set; }

        public decimal? FPrice { get; set; }

        public string? FHomeworkDescription { get; set; }

        public int? FMaxPeople { get; set; }

        public int? FMinPeople { get; set; }

        public DateTime? FLessonDate { get; set; }

        public TimeSpan? FStartTime { get; set; }

        public TimeSpan? FEndTime { get; set; }

        public bool? FVenueType { get; set; }

        public string? FOnlineLink { get; set; }

        public string? FVenueName { get; set; }

        public int? FDistrictId { get; set; }

        public string? FAddressDetail { get; set; }

        public DateTime? FRegDeadline { get; set; }

        public string? FStatus { get; set; }

        public string? FStatusNote { get; set; }

        public virtual TCourseSubject FSubject { get; set; }

        public virtual TTeacher FTeacher { get; set; }
    }
}
