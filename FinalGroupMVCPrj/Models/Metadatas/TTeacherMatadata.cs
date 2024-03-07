using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.Metadatas
{
    public class TTeacherMatadata
    {
        [Display(Name = "老師名稱")]
        public string? FTeacherName { get; set; }
        [Display(Name = "老師頭像")]
        public byte[]? FTeacherProfilePic { get; set; }
        [Display(Name = "關於我")]
        public string? FIntroduction { get; set; }
        [Display(Name = "聯絡方式")]
        public string? FContactInfo { get; set; }
        [Display(Name = "備註")]
        public string? FNote { get; set; }
    }
}
