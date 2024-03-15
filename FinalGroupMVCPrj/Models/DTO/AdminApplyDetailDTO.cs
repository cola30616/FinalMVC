namespace FinalGroupMVCPrj.Models.DTO
{
    public class AdminApplyDetailDTO
    {
        public int FApplyLogId { get; set; }

        public string? ApplyDatetime { get; set; }

        public int? FMemberId { get; set; }

        public string MemberRealName {  get; set; }
        public string MemberEmail { get; set; }

        public string TeacherName { get; set; }

        public string RealName { get; set; }

        public string ContactInfo { get; set; }

        public string Introduction { get; set; }

        public string Reason { get; set; }

        public string PdfLink { get; set; }

        public string TeacherPlanLink { get; set; }

        public string ProgressStatus { get; set; }

        public bool? FReviewResult { get; set; }

        public string? ReviewDatetime { get; set; } = "";

        public string Note { get; set; } = "";
    }
}
