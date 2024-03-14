using CsvHelper.Configuration.Attributes;

namespace FinalGroupMVCPrj.Models.DTO
{
    public class AdminMemberCSV
    {
        [Name("會員編號")]
        public int MemberId { get; set; } = 0;
        [Name("註冊日期")]
        public string RegDate { get; set; } = "";
        [Name("真實姓名")]
        public string? RealName { get; set; } = "";
        [Name("顯示名稱")]
        public string? ShowName { get; set; } = "";
        [Name("電子信箱")]
        public string? Email { get; set; } = "";
        [Name("信箱驗證")]
        public string? EmailVerification { get; set; } = "";
        [Name("接收優惠通知")]
        public string? GetCampInfo { get; set; } = "";
        [Name("電話")]
        public string? Phone { get; set; } = "";
        [Name("生日")]
        public string? Birth { get; set; } = "";
        [Name("性別")]
        public string? Gender { get; set; } = "";
        [Name("職業類別")]
        public string? Job { get; set; } = "";
        [Name("最高學歷")]
        public string? Education { get; set; } = "";
        [Name("連結的帳戶")]
        public string? Note { get; set; } = "";
        [Name("生活縣市")]
        public string? Cities { get; set; }
        [Name("想學習的領域")]
        public string? WishFields { get; set; }
    }
}
