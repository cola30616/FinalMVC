using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Models.DTO
{
    public class MemberInfoDTO 
    {
        public int MemberId { get; set; } = 0;
        public string RealName { get; set; } = "";
        public string ShowName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";

    }
}
