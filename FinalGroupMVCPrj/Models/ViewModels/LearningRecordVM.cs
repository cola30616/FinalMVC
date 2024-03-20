namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LearningRecordVM
    {
        public Dictionary<int, TLessonCourse> SuccessRecord { get; set; }

        public Dictionary<int, TLessonCourse> CancelRecord { get; set; }
       
    }
}
