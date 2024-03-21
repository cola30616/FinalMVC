using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class AdminTCheckVM
    {
        public int TeacherId { get; set; }
        [Display(Name = "可開課科目")]
        public string SubjectName { get; set; }
        [Display(Name = "老師頭像")]
        public byte[] TeacherProfilePic { get; set; }
        [Display(Name = "欲使用老師名稱")]
        public string TeacherName { get; set; }
        [Display(Name = "真實姓名")]
        public string RealName { get; set; }
        //public string Email { get; set; }
        [Display(Name = "備註")]
        public string Note { get; set; }
        }
}
