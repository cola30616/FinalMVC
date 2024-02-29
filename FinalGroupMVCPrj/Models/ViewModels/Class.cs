using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class Class
    {
        [Display(Name = "名字")]
        public string? TeacherName { get; set; }
        //public DateTime JoinDatetime { get; set; }
        [Display(Name = "廠商ID")]
        public byte[]? TeacherProfilePic { get; set; }
        [Display(Name = "關於我")]
        public string? Introduction { get; set; }
        [Display(Name = "聯絡方式")]
        public string? ContactInfo { get; set; }
        [Display(Name = "備註")]
        public string? Note { get; set; }
    }
}
