using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class AdminMemberController : Controller
    {
        private readonly LifeShareLearnContext _context;
        public AdminMemberController(LifeShareLearnContext context) 
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult ListDataJson([FromBody] MemberFilterDTO? memberFilterData)
        {
            var memberList = _context.TMembers.AsQueryable();
            if (memberFilterData != null)
            {

            }
            IEnumerable<MemberBasicViewModel> mBasicVMCollection =
                  new List<MemberBasicViewModel>(
                            memberList.Select(m => new MemberBasicViewModel
                            {
                                MemberId = m.FMemberId,
                                Email = m.FEmail,
                                EmailVerification = m.FEmailVerification ? "已驗證" : "未驗證",
                                RealName = m.FRealName,
                                ShowName = m.FShowName,
                                GetCampInfo = m.FGetCampaignInfo ? "是" : "否",
                                RegisterDateTime = m.FRegisterDatetime.ToString("yyyy/MM/dd HH:mm"),
                                Status = m.FStatus == true ? "正常" : "停權中"
                            })); ;
            return Json(new{data = mBasicVMCollection});
        }
        public IActionResult LoginHistory(int memberId, int toSkip, DateTime? lastDate)
        {
            var q = _context.TMemberLoginLogs.Where(l => l.FMemberId == memberId);
            if(lastDate != null)
            {
                q = q.Where(l => l.FLoginDateTime <= lastDate);
            }

            var   loginHistory =  q.Skip(toSkip).Take(5).ToList();

            return Json(loginHistory);
        }
    }
}
