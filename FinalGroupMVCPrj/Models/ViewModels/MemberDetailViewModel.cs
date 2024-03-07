using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class MemberDetailViewModel
    {
        public TMember Member { get; set; }
        [Display(Name = "生活縣市")]
        public IEnumerable<int>? Cities { get; set; }
        [Display(Name = "想學習的領域")]
        public IEnumerable<int>? WishFields { get; set; }

        public static IEnumerable<SelectListItem>  GetJobSelectList()
        {
            IEnumerable<SelectListItem> JobSelectList = new List<SelectListItem>
                {
                new SelectListItem { Text = "未提供", Value = null },
                new SelectListItem { Text = "自由業", Value = "自由業" },
     new SelectListItem  { Text = "學生", Value = "學生" },
new SelectListItem { Text = "經營／人資", Value = "經營／人資" },
new SelectListItem { Text = "行政／總務／法務", Value = "行政／總務／法務" },
new SelectListItem { Text = "財會／金融專業", Value = "財會／金融專業" },
new SelectListItem { Text = "行銷／企劃／專案管理", Value = "行銷／企劃／專案管理" },
new SelectListItem { Text = "客服／門市／業務／貿易", Value = "客服／門市／業務／貿易" },
new SelectListItem { Text = "餐飲／旅遊 ／美容美髮", Value = "餐飲／旅遊 ／美容美髮" },
new SelectListItem { Text = "資訊軟體", Value = "資訊軟體" },
new SelectListItem { Text = "操作／技術／維修", Value = "操作／技術／維修" },
new SelectListItem { Text = "資材／物流／運輸", Value = "資材／物流／運輸" },
new SelectListItem { Text = "營建／製圖", Value = "營建／製圖" },
new SelectListItem { Text = "傳播藝術／設計", Value = "傳播藝術／設計" },
new SelectListItem { Text = "文字／傳媒工作", Value = "文字／傳媒工作" },
new SelectListItem { Text = "醫療／保健服務", Value = "醫療／保健服務" },
new SelectListItem { Text = "學術／教育／輔導", Value = "學術／教育／輔導" },
new SelectListItem { Text = "體育／休閒活動", Value = "體育／休閒活動" },
new SelectListItem { Text = "研發相關", Value = "研發相關" },
new SelectListItem { Text = "生產製造／品管／環衛", Value = "生產製造／品管／環衛" },
new SelectListItem { Text = "軍警消／保全", Value = "軍警消／保全" },
new SelectListItem { Text = "農林漁牧", Value = "農林漁牧" },
new SelectListItem { Text = "家事服務／清潔", Value = "家事服務／清潔" },
new SelectListItem { Text = "其他新興產業", Value = "其他新興產業" }
                 };
            return JobSelectList;
        }
    }
}


