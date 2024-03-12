using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class TeacherBasicViewModel
    {
        ////////////////////////// TTeacher ///////////////////////////
        public int? TeacherId { get; set; }
        [Display(Name = "老師名稱")]
        public string? TeacherName { get; set; }
        //public DateTime JoinDatetime { get; set; }
        [Display(Name = "老師頭像")]
        public byte[]? TeacherProfilePic { get; set; }
        public string? TeacherProfilePicURL { get; set; }
        [Display(Name = "關於我")]
        public string? Introduction { get; set; }
        [Display(Name = "聯絡方式")]
        public string? ContactInfo { get; set; }
        [Display(Name = "備註")]
        public string? Note { get; set; }
        [Display(Name = "已開科目")]
        public IEnumerable<string>? SubjectName { get; set; }
        public TTeacher? TeacherModel { get; set; }



        ////////////////////////TTeacherImage/////////////////////////////
        public int? TeacherImagesId { get; set; }
        public string? ImageName { get; set; }
        public byte[]? ImageLink { get; set; }
        public string? Category { get; set; }
        public TTeacherImage? TeacherImageModel { get; set; }
    }
}
