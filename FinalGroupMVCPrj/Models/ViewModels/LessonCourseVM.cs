namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class LessonCourseVM
    {
        public TLessonCourse lessonCourse { get; set; }

        public string teacherName { get; set; } // 添加教師名稱屬性

        public string subjectName { get; set; }// 添加科目名稱屬性課程

        public List<string> fields { get; set; } // 所有領域資料

        public List<CourseSubjectData> fieldWithSubjects { get; set; } // 所有領域資料

        public string fieldName { get; set; }// 取得領域資料

        public int fieldNumber { get; set; }// 取得當前領域資料
        public byte[] imageData { get; set; }

        public TTeacher teacher { get; set; }
        public byte[] TeacherImage { get; set; }

    }



    public class CourseSubjectData
    {
        public int FieldId { get; set; }
        public string? FieldName { get; set; }
        public List<string>? SubjectNames { get; set; }


    }

}
