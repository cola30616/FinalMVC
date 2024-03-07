using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class AdminTeacherController : Controller
    {
        private readonly LifeShareLearnContext _context;

        public AdminTeacherController(LifeShareLearnContext context)
        {
            _context = context;
        }

        public IActionResult ApplyList()
        {
            return View();
        }
        public IActionResult ListDataJson([FromBody] TApplyFilterDTO? tApplyFilterDTO)
        {
            var tApplyList = _context.TTeacherApplyLogs.AsQueryable();
            if (tApplyFilterDTO != null)
            {

            }
            IEnumerable<AdminTApplyVM> tApplyVMCollection =
                  new List<AdminTApplyVM>(
                            tApplyList.Select(a => new AdminTApplyVM
                            {
                                ApplyLogId =a.FApplyLogId,
                                ApplyDatetime = a.FApplyDatetime.ToString("yyyy/MM/dd HH:mm"),
                                MemberId =a.FMemberId,
                                TeacherName = a.FTeacherName,
                                RealName = a.FRealName,
                                ProgressStatus = a.FProgressStatus,
                                ReviewDatetime = a.FReviewDatetime!= null?((DateTime) a.FReviewDatetime).ToString("yyyy/MM/dd HH:mm"): "-",
                                Note = a.FNote??"-"
                            })); ;
            return Json(new { data = tApplyVMCollection });
        }
    }
}
