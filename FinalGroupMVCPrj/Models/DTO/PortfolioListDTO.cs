namespace FinalGroupMVCPrj.Models.DTO
{
    public class PortfolioListDTO
    {
        public int FCourseworkId { get; set; }

        public int FOrderDetailId { get; set; }

        public string FName { get; set; }
        public string FDescrpition { get; set; }

        public DateTime FLastModifyTime { get; set; }

        public int? FMemberId { get; set; }

        public string FShareAudience { get; set; }

        public string FComment { get; set; }

        public DateTime? FCommentTime { get; set; }

        public virtual TOrderDetail FOrderDetail { get; set; }
    }
}
