using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public TOrder order { get; set; }
        public TOrderDetail orderDetail { get; set; }
        public TLessonCourse lessonCourse { get; set; }
        public TMember member { get; set; }

    }
}
