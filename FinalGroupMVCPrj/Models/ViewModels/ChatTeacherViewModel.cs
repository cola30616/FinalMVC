using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class ChatTeacherViewModel
    {
        public int FMemberId { get; set; }
        public string ?FShowName { get; set; }
        public byte[]? FMemberProfilePic { get; set; }
        public int FTeacherId { get; set; }
        public string? FTeacherName { get; set; }
        public byte[]? FTeacherProfilePic { get; set; }
    }
}
