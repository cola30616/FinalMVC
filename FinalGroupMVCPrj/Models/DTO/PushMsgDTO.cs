namespace FinalGroupMVCPrj.Models.DTO
{
    public class PushMsgDTO
    {
        public int pushMsgId { get; set; }
        public List<int> selectedMembers { get; set; }
        public int pushDelay { get; set; }
    }
}
