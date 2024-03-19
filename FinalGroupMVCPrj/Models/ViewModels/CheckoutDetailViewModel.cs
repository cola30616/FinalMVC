using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class CheckoutDetailViewModel
    {        
        public string FName { get; set; }
        public string FDescription { get; set; }
        public decimal FPrice { get; set; }
        public byte[] FPhoto { get; set; }
        public string FRealName { get; set; }
        public string FPhone { get; set; }
        public string FEmail { get; set; }
        public int FLessonCourseId { get; set; }
        public int FMemberId { get; set; }
    }
}

