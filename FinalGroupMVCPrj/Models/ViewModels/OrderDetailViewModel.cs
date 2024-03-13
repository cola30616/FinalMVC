using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public TOrder order { get; set; }
        public TOrderDetail orderDetail { get; set; }


        public string FRealName { get; set; }
        public string FPhone { get; set; }

        public string FName { get; set; }

        public string FDescription { get; set; }

        public string FEmail { get; set; }

        public TLessonCourse lessonCourse { get; set; }
        public TMember member { get; set; }

    }
}
