namespace FinalGroupMVCPrj.Models.ViewModel
{
    public class LessonCourseVM
    {
        public TLessonCourse lessonCourse { get; set; }

        public string teacherName { get; set; } // 添加教師名稱屬性

        public string subjectName {  get; set; }// 添加科目名稱屬性課程

        public List<string> fields {  get; set; } // 所有領域資料
        public byte[] imageData { get; set; } 
        
        public TTeacher teacher { get; set; }
     
    }

}
