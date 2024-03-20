namespace FinalGroupMVCPrj.Models.DTO
{
    public class PortfolioListDTO
    {
        public int FCourseworkId { get; set; } = 0;

        public string FName { get; set; } = "";
        public string FShowName { get; set; } = "";
        public string FSubjectName { get; set; } = "";
        public string FDescrpition { get; set; } = "";
        public string FLessonCourseDescrpition { get; set; } = "";
        public string FFieldName { get; set; } = "";
        public DateTime FLastModifyTime { get; set; }= DateTime.Now;

        public int? FMemberId { get; set; }= 0;

        public string FShareAudience { get; set; } = "";

        public string FComment { get; set; } = "";
        public string FCommentPerson { get; set; } = "";
        public string FLessonName { get; set; } = "";

        public DateTime? FCommentTime { get; set; }=DateTime.Now;

        public virtual TOrderDetail FOrderDetail { get; set; } = null;
    }
}
