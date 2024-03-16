using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.DTO
{
    public class AdminMemberDTO
    {
        public int MemberId { get; set; } = 0;

        public string RegDate { get; set; } = "";
        public string? RealName { get; set; } = "";
        public string? ShowName { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? EmailVerification { get; set; } = "";
        public string? GetCampInfo { get; set; } = "";
        public string? Phone { get; set; } = "";
        public string? Birth { get; set; } = "";
        public string? Gender { get; set; } = "";
        public string? Job { get; set; } = "";
        public string? Education { get; set; } = "";
        public string? Note { get; set; } = "";
        public string? Status{ get; set; } = "";
        public IEnumerable<string>? Cities { get; set; }
        public IEnumerable<string>? WishFields { get; set; }
    }
}
