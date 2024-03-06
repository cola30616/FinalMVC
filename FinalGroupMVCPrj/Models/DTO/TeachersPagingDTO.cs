using Microsoft.Build.Framework;

namespace FinalGroupMVCPrj.Models.DTO
{
    public class TeachersPagingDTO
    {
        public int TotalPages { get; set; }
        public List<TeacherInfo>? CardsResult { get; set; }
    }
    public class TeacherInfo
    {
        public int FTeacherId { get; set; }
        public string? FTeacherName { get; set; }
        public string? TeacherProfilePicURL { get; set; }
        public List<string>? SubjectNames { get; set; }
    }

}
