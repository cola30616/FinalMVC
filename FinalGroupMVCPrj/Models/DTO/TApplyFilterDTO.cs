namespace FinalGroupMVCPrj.Models.DTO
{
    public class TApplyFilterDTO
    {
        public DateTime? ApplyDateRangeStart { get; set; }
        public DateTime? ApplyDateRangeEnd { get; set; }
        public DateTime? UpdateDateRangeStart { get; set; }
        public DateTime? UpdateDateRangeEnd { get; set; }
        public string? ProgressStatus { get; set; } = null;
    }
}
