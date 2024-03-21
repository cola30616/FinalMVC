using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class CreatePushMsgViewModel
    {
        public string FPushType { get; set; }
        public string FPushContent { get; set; }
        public IFormFile FPushImagePath { get; set; }

    }
}
