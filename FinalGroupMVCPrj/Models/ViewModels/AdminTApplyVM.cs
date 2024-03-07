using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class AdminTApplyVM
    {
            public int ApplyLogId { get; set; }
        [Display(Name = "申請日期")]
        public string ApplyDatetime { get; set; }

            public int MemberId { get; set; }

        [Display(Name = "欲使用老師名稱")]
        public string TeacherName { get; set; }
        [Display(Name = "真實姓名")]
        public string RealName { get; set; }
        [Display(Name = "審核進度")]
        public string ProgressStatus { get; set; }

        [Display(Name = "更新日期")]
        public string ReviewDatetime { get; set; }
        [Display(Name = "說明")]
        public string Note { get; set; }
        }
}
