namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LessonHistoryDetailViewModel
    {
        public string? FName { get; set; }
        public string? FDescription { get; set; }
        public byte[]? FPhoto { get; set; }

        public int FOrderId { get; set; }
        public bool FOrderValid { get; set; }
        public DateTime FOrderDate { get; set; }
        public decimal? FLessonPrice { get; set; }

        public string FModificationDescription { get; set; }

        public int FLessonCourseId { get; set; }

        public DateTime? FLessonDate { get; set; }

        public TOrder order { get; set; }
        public TOrderDetail orderDetail { get; set; }
        public TLessonCourse lessonCourse { get; set; }
    }
}
