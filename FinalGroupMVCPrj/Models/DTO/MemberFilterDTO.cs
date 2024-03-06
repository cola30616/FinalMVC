namespace FinalGroupMVCPrj.Models.DTO
{
    public class MemberFilterDTO
    {
        public DateTime? DateRangeStart { get; set; }
        public DateTime? DateRangeEnd { get; set; }
        public bool? EmailConfirmed { get; set; } = null;
        public bool? GetCampInfo { get; set; } = null;
        public bool? Status { get; set; } = null;

    }
}
