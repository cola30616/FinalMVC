namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LearningRecordVM
    {
        public List<TLessonCourse> SuccessRecord { get; set; }

        public List<TLessonCourse> CancelRecord { get; set; }
        public int FOrderId { get; set; }

        //public Dictionary<int, TLessonCourse> SuccessRecord { get; set; }

    }
}
